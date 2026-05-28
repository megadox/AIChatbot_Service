# AWS ECR + Lightsail Docker 배포 가이드

이 문서는 `BAStudio.Chatbot.Api`를 Docker 이미지로 빌드하고, AWS ECR(Elastic Container Registry)에 push한 뒤, Lightsail 서버에서 pull하여 실행하는 절차를 정리한다.

## 전체 흐름

```text
개발 PC 또는 CI
├─ Docker 이미지 빌드
├─ AWS ECR 로그인
├─ ECR repository 생성
├─ 이미지 tag 지정
└─ docker push

AWS Lightsail 서버
├─ Docker / Docker Compose 설치
├─ AWS CLI 설치 및 권한 설정
├─ AWS ECR 로그인
├─ docker compose pull
└─ docker compose up -d
```

## 필요한 도구

개발 PC 또는 CI에는 다음 도구가 필요하다.

```text
Docker Desktop 또는 Docker Engine
AWS CLI v2
AWS 계정 및 ECR push 권한
```

Lightsail 서버에는 다음 도구가 필요하다.

```text
Docker Engine
Docker Compose plugin
AWS CLI v2
ECR pull 권한
```

Docker 이미지를 빌드하는 별도 전용 도구는 필요 없다. 이 프로젝트는 `deploy/Dockerfile`을 사용하므로 `docker build` 명령으로 이미지를 만들 수 있다.

## 배포 변수

아래 값은 예시이며 실제 AWS 계정에 맞게 바꾼다.

```powershell
$AWS_REGION = "ap-northeast-2"
$AWS_ACCOUNT_ID = "123456789012"
$ECR_REPOSITORY = "ba-chatbot-api"
$IMAGE_TAG = "20260526"
$ECR_REGISTRY = "$AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com"
$IMAGE_URI = "$ECR_REGISTRY/$ECR_REPOSITORY`:$IMAGE_TAG"
```

Linux/macOS 또는 Lightsail 서버에서는 다음처럼 사용한다.

```bash
AWS_REGION=ap-northeast-2
AWS_ACCOUNT_ID=123456789012
ECR_REPOSITORY=ba-chatbot-api
IMAGE_TAG=20260526
ECR_REGISTRY="${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_REGION}.amazonaws.com"
IMAGE_URI="${ECR_REGISTRY}/${ECR_REPOSITORY}:${IMAGE_TAG}"
```

## 1. AWS CLI 로그인 상태 확인

개발 PC에서 AWS CLI가 정상 설정되어 있는지 확인한다.

```powershell
aws sts get-caller-identity
```

아직 설정하지 않았다면 다음 명령으로 access key, secret key, 기본 region을 설정한다.

```powershell
aws configure
```

권장 region은 서울 리전이다.

```text
ap-northeast-2
```

## 2. ECR repository 생성

최초 1회만 실행한다.

```powershell
aws ecr create-repository `
  --repository-name ba-chatbot-api `
  --region ap-northeast-2
```

이미 repository가 있으면 이 단계는 생략한다.

생성 후 repository URI는 다음 형식이다.

```text
123456789012.dkr.ecr.ap-northeast-2.amazonaws.com/ba-chatbot-api
```

## 3. Docker 이미지를 로컬에서 빌드

프로젝트 루트에서 실행한다.

```powershell
docker build -f deploy/Dockerfile -t ba-chatbot-api:20260526 .
```

마지막의 `.`은 반드시 필요하다. 이 값은 Docker build context이며, 현재 프로젝트 루트 전체를 빌드 입력으로 사용한다는 뜻이다.

잘못된 예:

```powershell
docker build -f deploy/Dockerfile -t ba-chatbot-api:20260526
```

위처럼 `.`을 생략하면 다음 오류가 발생한다.

```text
ERROR: docker: 'docker buildx build' requires 1 argument
```

### 버전 정보 포함 빌드

챗봇 API 버전은 repository 루트의 `VERSION` 파일을 기준으로 관리한다.

```text
VERSION
```

Docker 이미지에는 다음 build arg가 들어간다.

```text
APP_VERSION
BUILD_DATE
GIT_SHA
```

수동 빌드 시 예:

```powershell
docker build `
  --build-arg APP_VERSION=0.1.0 `
  --build-arg IMAGE_TAG=20260528_02 `
  --build-arg BUILD_DATE=2026-05-28T10:00:00Z `
  --build-arg GIT_SHA=abcdef1 `
  -f deploy/Dockerfile `
  -t ba-chatbot-api:20260528_02 `
  .
```

`deploy/publish-ecr-lightsail.ps1`을 사용하면 `VERSION` 파일, 현재 UTC 빌드 시간, 현재 Git SHA가 자동으로 Docker 이미지와 `/health` 응답에 포함된다.

배포 후 확인:

```bash
curl https://chatbot-api.batem.com/health
```

응답 예:

```json
{
  "status": "ok",
  "version": "0.1.0",
  "imageTag": "20260528_02",
  "buildDate": "2026-05-28T10:00:00Z",
  "gitSha": "abcdef1",
  "kb": "loaded",
  "kbPath": "/app/ChatBot/ba_manual_vector.db",
  "timestamp": "2026-05-28T10:01:00Z"
}
```

이 빌드는 다음 파일들을 이미지 안에 포함한다.

```text
src/BAStudio.Chatbot.Api
src/BAStudio.Chatbot
src/BAStudio.Chatbot.Infra
ChatBot/ba_manual_vector.db
docs/product-manuals
```

따라서 Lightsail 서버에 소스 코드, KB DB, 매뉴얼 문서를 따로 복사하지 않아도 된다.

빌드 결과 확인:

```powershell
docker images ba-chatbot-api
```

## 4. 로컬에서 컨테이너 실행 테스트

ECR에 올리기 전에 로컬에서 먼저 API가 실행되는지 확인한다.

```powershell
docker run --rm -p 8080:8080 `
  -e ASPNETCORE_ENVIRONMENT=Production `
  -e Chatbot__KbPath=/app/ChatBot/ba_manual_vector.db `
  -e Chatbot__ModelPath="" `
  -e CHATBOT_API_TOKENS=local-test-token `
  ba-chatbot-api:20260526
```

다른 터미널에서 확인한다.

```powershell
curl http://localhost:8080/health
```

정상 응답 예:

```json
{
  "status": "ok"
}
```

### Docker Desktop에서 `localhost:8080` 연결이 안 될 때

다음 오류가 나오면 host의 8080 포트에 연결된 컨테이너가 없다는 뜻이다.

```text
curl: (7) Failed to connect to localhost port 8080
```

먼저 실행 중인 컨테이너를 확인한다.

```powershell
docker ps
```

`PORTS` 항목에 다음처럼 보여야 한다.

```text
0.0.0.0:8080->8080/tcp
```

컨테이너가 없거나 포트 매핑이 없다면 프로젝트 루트에서 compose로 다시 실행한다.

```powershell
docker compose -f deploy/docker-compose.yml up -d --build
```

실행 후 다시 확인한다.

```powershell
docker compose -f deploy/docker-compose.yml ps
curl http://localhost:8080/health
```

컨테이너는 있는데 바로 종료된다면 로그를 확인한다.

```powershell
docker logs ba-chatbot-api
```

Docker Desktop UI에서 직접 컨테이너를 실행했다면 `Ports` 설정에 host port `8080`, container port `8080`이 publish되어 있어야 한다.

## 5. ECR 로그인

개발 PC에서 Docker가 ECR에 push할 수 있도록 로그인한다.

```powershell
aws ecr get-login-password --region ap-northeast-2 `
  | docker login --username AWS --password-stdin 123456789012.dkr.ecr.ap-northeast-2.amazonaws.com
```

로그인이 성공하면 `Login Succeeded`가 출력된다.

## 6. ECR 주소로 Docker 이미지 tag 지정

로컬 이미지에 ECR repository 주소를 붙인다.

```powershell
docker tag ba-chatbot-api:20260526 123456789012.dkr.ecr.ap-northeast-2.amazonaws.com/ba-chatbot-api:20260526
```

운영에서 `latest`를 쓰고 싶다면 추가로 tag를 붙일 수 있다.

```powershell
docker tag ba-chatbot-api:20260526 123456789012.dkr.ecr.ap-northeast-2.amazonaws.com/ba-chatbot-api:latest
```

권장 방식은 날짜 또는 빌드 번호 tag를 운영 배포에 사용하는 것이다. `latest`는 어떤 버전이 배포되었는지 추적하기 어렵다.

## 7. ECR에 Docker 이미지 push

```powershell
docker push 123456789012.dkr.ecr.ap-northeast-2.amazonaws.com/ba-chatbot-api:20260526
```

`latest` tag도 사용한다면 함께 push한다.

```powershell
docker push 123456789012.dkr.ecr.ap-northeast-2.amazonaws.com/ba-chatbot-api:latest
```

ECR에 올라간 이미지 확인:

```powershell
aws ecr describe-images `
  --repository-name ba-chatbot-api `
  --region ap-northeast-2
```

## 8. Lightsail 서버 준비

Lightsail 인스턴스에 접속한다.

```bash
ssh ubuntu@your-lightsail-public-ip
```

### Docker가 이미 설치된 경우

Lightsail Ubuntu 인스턴스에 Docker가 이미 설치되어 있고 다른 서비스가 실행 중이라면 Docker를 재설치하지 않는다. 먼저 현재 상태만 확인한다.

```bash
docker --version
docker compose version
docker ps
```

다른 서비스가 사용 중인 포트도 확인한다.

```bash
sudo ss -ltnp
```

현재 `deploy/docker-compose.yml` 예시는 host의 `8080` 포트를 사용한다.

```yaml
ports:
  - "8080:8080"
```

기존 서비스가 이미 8080을 사용 중이면 host 포트를 바꾼다. 예를 들어 host의 18080 포트로 열려면 다음처럼 설정한다.

```yaml
ports:
  - "18080:8080"
```

이 경우 health check는 다음 주소로 확인한다.

```bash
curl http://localhost:18080/health
```

컨테이너 이름도 기존 서비스와 충돌하지 않아야 한다.

```yaml
container_name: ba-chatbot-api
```

이미 같은 이름의 컨테이너가 있으면 기존 챗봇 컨테이너인지 확인한 뒤 업데이트하거나, 새 이름을 사용한다.

```bash
docker ps -a --filter name=ba-chatbot-api
```

### Docker가 설치되지 않은 경우

Docker가 없는 신규 Ubuntu 인스턴스에서만 아래 설치 절차를 실행한다.

```bash
sudo apt-get update
sudo apt-get install -y ca-certificates curl gnupg
sudo install -m 0755 -d /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
sudo chmod a+r /etc/apt/keyrings/docker.gpg
echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
  $(. /etc/os-release && echo "$VERSION_CODENAME") stable" \
  | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
```

현재 사용자로 Docker를 실행하려면:

```bash
sudo usermod -aG docker $USER
```

이후 SSH를 재접속한다.

설치 확인:

```bash
docker --version
docker compose version
```

AWS CLI v2도 확인한다. Lightsail 이미지에 AWS CLI가 이미 있으면 설치를 생략한다.

```bash
aws --version
```

## 9. Lightsail 서버의 AWS 권한 설정

Lightsail 서버가 private ECR에서 이미지를 pull하려면 AWS 인증 정보가 필요하다.

간단한 방법은 서버에서 다음 명령으로 IAM access key를 설정하는 것이다.

```bash
aws configure
```

입력 값:

```text
AWS Access Key ID: ECR pull 권한이 있는 IAM access key
AWS Secret Access Key: 해당 secret key
Default region name: ap-northeast-2
Default output format: json
```

운영 권장 IAM 권한은 push 권한이 아니라 pull 권한만 주는 것이다.

최소 pull 권한 예:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "ecr:GetAuthorizationToken",
        "ecr:BatchCheckLayerAvailability",
        "ecr:GetDownloadUrlForLayer",
        "ecr:BatchGetImage"
      ],
      "Resource": "*"
    }
  ]
}
```

## 10. Lightsail 서버에서 ECR 로그인

```bash
aws ecr get-login-password --region ap-northeast-2 \
  | docker login --username AWS --password-stdin 123456789012.dkr.ecr.ap-northeast-2.amazonaws.com
```

정상 출력:

```text
Login Succeeded
```

## 11. Lightsail 배포 디렉터리 구성

서버에 배포 폴더를 만든다.

```bash
mkdir -p ~/ba-chatbot-api
cd ~/ba-chatbot-api
```

다른 Docker 서비스와 compose project 이름이 섞이지 않도록 이후 명령은 이 디렉터리에서 실행한다. 필요하면 `-p ba-chatbot-api` 옵션으로 compose project 이름을 명시한다.

운영용 `docker-compose.yml`을 생성한다.

```yaml
services:
  chatbot-api:
    image: 123456789012.dkr.ecr.ap-northeast-2.amazonaws.com/ba-chatbot-api:20260526
    container_name: ba-chatbot-api
    restart: unless-stopped
    ports:
      - "8080:8080"
    environment:
      ASPNETCORE_ENVIRONMENT: Production
      Chatbot__KbPath: /app/ChatBot/ba_manual_vector.db
      Chatbot__ModelPath: ""
      Chatbot__TopK: "6"
      Chatbot__MinScore: "0"
      CHATBOT_API_TOKENS: ${CHATBOT_API_TOKENS}
```

주의: 운영 서버의 compose 파일에는 `build:`를 넣지 않는다. 운영 서버는 소스 코드로 빌드하지 않고 ECR에서 이미지를 pull한다.

`.env` 파일을 생성한다.

```bash
nano .env
```

예:

```text
CHATBOT_API_TOKENS=customer-a-token,customer-b-token
```

`.env`에는 고객사 토큰 등 비밀값이 들어가므로 Git에 커밋하지 않는다.

## 12. Lightsail에서 이미지 pull 및 실행

```bash
docker compose -p ba-chatbot-api pull
docker compose -p ba-chatbot-api up -d
```

컨테이너 상태 확인:

```bash
docker compose -p ba-chatbot-api ps
docker logs -f ba-chatbot-api
```

API health 확인:

```bash
curl http://localhost:8080/health
```

Lightsail 방화벽에서 8080 포트를 직접 열었다면 외부에서도 확인할 수 있다.

```bash
curl http://your-lightsail-public-ip:8080/health
```

운영에서는 8080을 직접 공개하기보다 Nginx reverse proxy와 HTTPS를 앞에 두는 것을 권장한다.

## 13. Route53 도메인 연결

서버 내부에서 `curl http://localhost:8080/health`가 정상인데 외부 public IP로 접속이 안 되거나, IP 대신 도메인을 쓰고 싶다면 Route53에서 DNS record를 설정한다.

권장 구조:

```text
chatbot.example.com
└─ Route53 A record
   └─ Lightsail static IP
      └─ Nginx HTTPS reverse proxy
         └─ localhost:8080 ba-chatbot-api
```

### 13-1. Lightsail static IP 사용

Lightsail 인스턴스의 기본 public IP는 인스턴스 stop/start 상황에 따라 바뀔 수 있다. Route53에 연결할 서버는 static IP를 붙이는 것을 권장한다.

Lightsail 콘솔에서:

```text
Networking
→ Create static IP
→ 인스턴스 선택
→ Attach
```

이후 Route53에는 인스턴스의 일반 public IP가 아니라 static IP를 입력한다.

### 13-2. Route53 A record 생성

Route53 콘솔에서:

```text
Hosted zones
→ 사용할 도메인 선택
→ Create record
```

예:

```text
Record name: chatbot
Record type: A
Value: Lightsail static IP
TTL: 300
Routing policy: Simple routing
```

도메인이 `example.com`이면 최종 주소는 다음과 같다.

```text
chatbot.example.com
```

DNS 반영 확인:

```bash
nslookup chatbot.example.com
```

또는:

```bash
dig chatbot.example.com
```

### 13-3. 8080 포트로 직접 테스트하는 경우

Nginx 없이 Docker host port `8080`을 직접 공개한다면 URL에 포트 번호가 필요하다.

```bash
curl http://chatbot.example.com:8080/health
```

이 방식은 테스트에는 단순하지만 운영 권장 방식은 아니다. 이 경우 Lightsail 방화벽에서 TCP 8080을 열어야 한다.

```text
Lightsail instance
→ Networking
→ IPv4 Firewall
→ Add rule
→ Custom / TCP / 8080
```

### 13-4. 운영 권장 방식: Nginx + HTTPS

운영에서는 Route53 도메인을 Nginx에 연결하고, Nginx가 내부의 `localhost:8080`으로 reverse proxy 하도록 구성한다. 이 경우 외부 사용자는 8080 포트 없이 접속한다.

```bash
curl https://chatbot.example.com/health
```

Lightsail 방화벽은 80, 443만 열고 8080은 외부에 열지 않는 구성이 좋다.

```text
HTTP  TCP 80
HTTPS TCP 443
```

Nginx server block 예:

```nginx
server {
    listen 80;
    server_name chatbot.example.com;

    location / {
        proxy_pass http://127.0.0.1:8080;
        proxy_http_version 1.1;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

설정 파일 예:

```bash
sudo nano /etc/nginx/sites-available/ba-chatbot-api
```

활성화:

```bash
sudo ln -s /etc/nginx/sites-available/ba-chatbot-api /etc/nginx/sites-enabled/ba-chatbot-api
sudo nginx -t
sudo systemctl reload nginx
```

HTTPS 인증서는 Let's Encrypt certbot을 사용한다.

```bash
sudo certbot --nginx -d chatbot.example.com
```

인증서 발급 후 확인:

```bash
curl https://chatbot.example.com/health
```

WPF 온라인 모드의 API 주소도 HTTPS 주소로 설정한다.

```text
https://chatbot.example.com
```

## 14. 새 버전 배포

개발 PC에서 새 tag로 다시 빌드한다.

```powershell
docker build -f deploy/Dockerfile -t ba-chatbot-api:20260527 .
docker tag ba-chatbot-api:20260527 123456789012.dkr.ecr.ap-northeast-2.amazonaws.com/ba-chatbot-api:20260527
docker push 123456789012.dkr.ecr.ap-northeast-2.amazonaws.com/ba-chatbot-api:20260527
```

Lightsail 서버의 `docker-compose.yml`에서 image tag를 바꾼다.

```yaml
image: 123456789012.dkr.ecr.ap-northeast-2.amazonaws.com/ba-chatbot-api:20260527
```

다시 pull 및 재시작한다.

```bash
docker compose -p ba-chatbot-api pull
docker compose -p ba-chatbot-api up -d
```

사용하지 않는 이전 이미지는 필요 시 정리한다. 단, 같은 서버에서 다른 서비스도 실행 중이면 전체 정리 명령이 다른 서비스 이미지에 영향을 줄 수 있으므로 삭제 대상을 먼저 확인한다.

```bash
docker images
```

특정 이전 챗봇 이미지만 지우려면 image id 또는 tag를 지정한다.

```bash
docker rmi 123456789012.dkr.ecr.ap-northeast-2.amazonaws.com/ba-chatbot-api:20260526
```

## 15. 현재 repository의 compose 파일 처리

현재 `deploy/docker-compose.yml`은 개발 편의를 위해 `build:`가 포함되어 있다.

```yaml
build:
  context: ..
  dockerfile: deploy/Dockerfile
image: ba-chatbot-api:latest
```

이 형태는 서버에서 소스 코드를 가지고 직접 빌드할 때 사용하는 구성이다.

운영 Lightsail 서버에서는 다음처럼 `build:`를 제거하고 ECR image URI만 남기는 구성이 좋다.

```yaml
services:
  chatbot-api:
    image: 123456789012.dkr.ecr.ap-northeast-2.amazonaws.com/ba-chatbot-api:20260526
    container_name: ba-chatbot-api
    restart: unless-stopped
    ports:
      - "8080:8080"
    environment:
      ASPNETCORE_ENVIRONMENT: Production
      Chatbot__KbPath: /app/ChatBot/ba_manual_vector.db
      Chatbot__ModelPath: ""
      Chatbot__TopK: "6"
      Chatbot__MinScore: "0"
CHATBOT_API_TOKENS: ${CHATBOT_API_TOKENS}
```

## 16. 자주 나는 오류

### no basic auth credentials

ECR 로그인이 안 된 상태다.

```bash
aws ecr get-login-password --region ap-northeast-2 \
  | docker login --username AWS --password-stdin 123456789012.dkr.ecr.ap-northeast-2.amazonaws.com
```

### repository does not exist

ECR repository가 없거나 이름이 다르다.

```bash
aws ecr describe-repositories --region ap-northeast-2
```

### denied: User is not authorized

IAM 권한이 부족하다. 개발 PC에는 ECR push 권한, Lightsail 서버에는 ECR pull 권한이 필요하다.

### HTTPS 접속 시 Nginx 404 Not Found

다음처럼 HTTPS 요청은 Nginx까지 도달하지만 `/health`가 404를 반환하면, 443 server block이 챗봇 API 컨테이너로 reverse proxy 하지 않는 상태다.

```bash
curl https://chatbot-api.batem.com/health
```

응답 예:

```html
<h1>404 Not Found</h1>
<center>nginx/1.18.0 (Ubuntu)</center>
```

먼저 챗봇 컨테이너는 내부에서 정상인지 확인한다.

```bash
curl http://127.0.0.1:8080/health
```

정상이라면 Nginx 설정을 확인한다.

```bash
sudo grep -R "server_name chatbot-api.batem.com" -n /etc/nginx/sites-enabled /etc/nginx/conf.d
sudo nginx -T | grep -A80 -B10 "server_name chatbot-api.batem.com"
```

443 server block에도 다음 `location /` proxy 설정이 있어야 한다.

```nginx
server {
    listen 443 ssl;
    server_name chatbot-api.batem.com;

    ssl_certificate /etc/letsencrypt/live/chatbot-api.batem.com/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/chatbot-api.batem.com/privkey.pem;

    location / {
        proxy_pass http://127.0.0.1:8080;
        proxy_http_version 1.1;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

수정 후 적용한다.

```bash
sudo nginx -t
sudo systemctl reload nginx
curl https://chatbot-api.batem.com/health
```

### exec format error

빌드한 CPU 아키텍처와 서버 아키텍처가 다를 때 발생할 수 있다. 일반 Lightsail Ubuntu 인스턴스는 보통 `linux/amd64`를 사용한다.

필요하면 빌드 시 platform을 지정한다.

```powershell
docker build --platform linux/amd64 -f deploy/Dockerfile -t ba-chatbot-api:20260526 .
```

## 참고 공식 문서

- Amazon ECR CLI 시작하기: https://docs.aws.amazon.com/AmazonECR/latest/userguide/getting-started-cli.html
- AWS CLI `ecr create-repository`: https://docs.aws.amazon.com/cli/latest/reference/ecr/create-repository.html
- Amazon Lightsail Docker/AWS CLI 설치 안내: https://docs.aws.amazon.com/lightsail/latest/userguide/amazon-lightsail-install-software.html
