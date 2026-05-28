# Online Chatbot Service Deployment Guide

이 문서는 온라인 고객사를 대상으로 현재 WPF 챗봇 UI는 유지하면서, 답변 생성/RAG/웹 검색/매뉴얼 DB 처리를 AWS Lightsail의 Docker 서비스로 분리하는 구현 방안을 설명한다.

## 목표

온라인 고객사 배포 형태는 다음 구조를 목표로 한다.

```text
고객사 PC
└─ BAStudio.Wpf
   └─ HTTPS API 호출

AWS Lightsail
└─ Docker Container
   ├─ BAStudio.Chatbot.Api
   ├─ BAStudio.Chatbot
   ├─ BAStudio.Chatbot.Infra
   ├─ ChatBot/ba_manual_vector.db
   └─ docs/product-manuals/guides, normalized, supplements
```

WPF는 화면, 세션 UI, 입력/출력 표시만 담당한다. 답변 생성은 웹서비스가 담당한다.

## 현재 코드에서 재사용할 부분

서버에서 그대로 재사용할 수 있는 핵심 구성은 다음이다.

- `BAStudio.Chatbot`
  - `ChatOrchestrator`
  - `DomainIntentResolver`
  - `PromptBuilder`
  - `ChatRequest`, `ChatStreamEvent`
- `BAStudio.Chatbot.Infra`
  - `SqliteVectorStore`
  - `HashEmbeddingService`
  - `NaverWebSearchService`
- `ChatBot/ba_manual_vector.db`
  - activity manual
  - product manual
  - product guide
  - answer correction
- `docs/product-manuals`
  - guide/normalized/supplement 원본 근거

WPF에서 서버로 이동해야 하는 부분은 `ChatViewModel` 내부의 `ChatOrchestrator` 직접 생성과 호출이다.

## 추가할 프로젝트

온라인 서비스용으로 다음 프로젝트를 추가한다.

```text
src/BAStudio.Chatbot.Api
```

권장 타입:

```text
ASP.NET Core Minimal API
TargetFramework: net8.0
```

참조 프로젝트:

```xml
<ProjectReference Include="..\BAStudio.Chatbot\BAStudio.Chatbot.csproj" />
<ProjectReference Include="..\BAStudio.Chatbot.Infra\BAStudio.Chatbot.Infra.csproj" />
```

## API 계약

### 1. 상태 확인

```http
GET /health
```

응답 예:

```json
{
  "status": "ok",
  "kb": "loaded",
  "version": "20260525"
}
```

### 2. 일반 답변 요청

```http
POST /api/chat
Content-Type: application/json
Authorization: Bearer {customer-token}
```

요청:

```json
{
  "conversationId": "customer-session-id",
  "question": "프로젝트에서 디버깅하는 방법을 알려줘.",
  "questionType": "BAStudioGuide",
  "topK": 8,
  "minScore": 0.0,
  "allowWebSearch": false
}
```

응답:

```json
{
  "answer": "현재 Task는 `Debug Task`, 프로젝트 전체는 `Debug Project`로 디버그 실행한다...",
  "details": "source: docs/product-manuals/guides/ba-studio/2.6.0/debug-task-project.md\n...",
  "events": [
    {
      "kind": "Status",
      "text": "질문 의도를 분석하는 중..."
    },
    {
      "kind": "Token",
      "text": "현재 Task는..."
    }
  ]
}
```

### 3. 스트리밍 답변 요청

WPF에서 현재처럼 상태 로그와 답변 토큰을 순차 표시하려면 SSE 방식이 가장 단순하다.

```http
POST /api/chat/stream
Accept: text/event-stream
Content-Type: application/json
Authorization: Bearer {customer-token}
```

SSE 이벤트 예:

```text
event: status
data: 질문 의도를 분석하는 중...

event: status
data: 매뉴얼을 검색하는 중...

event: token
data: 현재 Task는 `Debug Task`, 프로젝트 전체는 `Debug Project`로...

event: completed
data:
```

WPF는 `status` 이벤트를 처리 로그에 추가하고, `token` 이벤트를 답변 영역에 누적한다.

## API 서버 구현 방향

`Program.cs` 구성 예시는 다음 방향으로 작성한다.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new ChatbotOptions
{
    KbPath = builder.Configuration["Chatbot:KbPath"] ?? "ChatBot/ba_manual_vector.db",
    ModelPath = builder.Configuration["Chatbot:ModelPath"] ?? ""
});

builder.Services.AddSingleton<IEmbeddingService, HashEmbeddingService>();
builder.Services.AddSingleton<IVectorStore>(sp =>
{
    var options = sp.GetRequiredService<ChatbotOptions>();
    return new SqliteVectorStore(options.KbPath);
});
builder.Services.AddSingleton<IPromptBuilder, PromptBuilder>();
builder.Services.AddSingleton<ILlmService, NoopLlmService>();
builder.Services.AddSingleton<IWebSearchService, NaverWebSearchService>();
builder.Services.AddSingleton<DomainIntentResolver>();
builder.Services.AddSingleton<IChatOrchestrator, ChatOrchestrator>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/api/chat", async (
    ChatRequest request,
    IChatOrchestrator orchestrator,
    CancellationToken cancellationToken) =>
{
    var events = new List<ChatStreamEvent>();
    await foreach (var evt in orchestrator.AskAsync(request, cancellationToken))
    {
        events.Add(evt);
    }

    var fullText = string.Concat(events
        .Where(e => e.Kind == ChatStreamEventKind.Token)
        .Select(e => e.Text));

    var parts = fullText.Split("\n<<<DETAILS>>>\n", 2, StringSplitOptions.None);
    return Results.Ok(new
    {
        answer = parts[0],
        details = parts.Length > 1 ? parts[1] : "",
        events
    });
});

app.Run();
```

초기 구현은 `/api/chat` 동기 응답으로 시작하고, WPF 응답 UX를 유지해야 하면 `/api/chat/stream`을 추가한다.

## WPF 변경 방향

현재 WPF는 `ChatViewModel.Create()`에서 직접 다음 객체를 생성한다.

```csharp
var orchestrator = new ChatOrchestrator(...);
```

온라인 고객사용 빌드에서는 이 부분을 HTTP 클라이언트 기반 구현으로 바꾼다.

권장 추상화:

```text
IChatOrchestrator
├─ LocalChatOrchestrator: 현재 방식
└─ RemoteChatOrchestratorClient: API 호출 방식
```

`RemoteChatOrchestratorClient`는 `IChatOrchestrator`를 구현하고, 내부에서 `/api/chat/stream` 또는 `/api/chat`을 호출한다.

WPF 설정 파일 예:

```json
{
  "Chatbot": {
    "Mode": "Remote",
    "ApiBaseUrl": "https://chatbot.example.com",
    "ApiToken": "customer-issued-token"
  }
}
```

오프라인 설치 파일은 `Mode=Local`, 온라인 고객사는 `Mode=Remote`로 배포한다.

## 인증

최소 인증 방식:

```http
Authorization: Bearer {customer-token}
```

서버는 고객사별 토큰을 환경변수 또는 설정 파일로 관리한다.

권장 환경변수:

```text
CHATBOT_API_TOKENS=customer-a-token,customer-b-token
```

추후 확장:

- 고객사별 API Key 발급
- 토큰 만료일 관리
- 요청량 제한
- IP allowlist
- 고객사별 로그 분리

## Docker 구성

권장 Dockerfile:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet publish src/BAStudio.Chatbot.Api/BAStudio.Chatbot.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .
COPY ChatBot/ba_manual_vector.db ./ChatBot/ba_manual_vector.db
COPY docs/product-manuals ./docs/product-manuals

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BAStudio.Chatbot.Api.dll"]
```

권장 `.dockerignore`:

```text
bin/
obj/
.git/
.vs/
*.user
AI_ChatBot_Service.code-workspace
```

## Lightsail 배포 방식

권장 운영 방식은 Lightsail 인스턴스에 Docker를 설치하고 컨테이너를 실행하는 방식이다.

배포 흐름:

```text
1. 로컬 또는 CI에서 Docker 이미지 빌드
2. 이미지 registry에 push
3. Lightsail 인스턴스에서 image pull
4. docker compose up -d
5. HTTPS reverse proxy 연결
6. /health 확인
```

AWS ECR을 registry로 사용하고 Lightsail 인스턴스에서 이미지를 pull하는 상세 절차는
`docs/aws-ecr-lightsail-docker-deployment.md`를 참고한다.

`docker-compose.yml` 예:

```yaml
services:
  chatbot-api:
    image: your-registry/ba-chatbot-api:latest
    container_name: ba-chatbot-api
    restart: unless-stopped
    ports:
      - "8080:8080"
    environment:
      ASPNETCORE_ENVIRONMENT: Production
      Chatbot__KbPath: /app/ChatBot/ba_manual_vector.db
      CHATBOT_API_TOKENS: ${CHATBOT_API_TOKENS}
    volumes:
      - ./logs:/app/logs
```

HTTPS는 다음 중 하나를 선택한다.

- Lightsail Load Balancer + 인증서
- Nginx reverse proxy + Let's Encrypt
- AWS CloudFront 앞단 배치

초기에는 Nginx reverse proxy 구성이 단순하다.

## 데이터 업데이트 전략

제품 매뉴얼, guide, activity KB가 변경되면 다음 순서로 배포한다.

```text
1. Tools.ProductManualCollector 실행
2. guide 문서 보강
3. Tools.KbBuilder 실행
4. RetrievalSmokeTest 실행
5. Docker image rebuild
6. Lightsail 배포
7. /health 및 대표 질문 테스트
```

대표 검증 질문:

```text
프로젝트 생성방법을 알려줘.
프로젝트에서 라이브러리 사용법을 알려줘.
디버깅 하는 방법을 알려줘.
브라우저를 오픈하는 액티비티는?
엑셀의 A2 값을 가져오는 액티비티는?
```

## 로그와 모니터링

서버에서 남겨야 할 로그:

- 요청 시간
- 고객사 ID
- 질문 유형
- 질문 길이
- 검색 top source
- 답변 성공/실패
- 처리 시간
- 예외 메시지

답변 전문과 질문 전문은 고객사 보안 정책에 따라 저장 여부를 선택한다. 민감 정보가 포함될 수 있으므로 기본값은 질문/답변 전문 미저장을 권장한다.

## 보안 고려사항

- API는 반드시 HTTPS로만 제공한다.
- 고객사별 API Token을 분리한다.
- 토큰은 WPF 설정 파일에 평문 저장하지 않는 것을 권장한다.
- 가능한 경우 Windows Credential Manager 또는 암호화된 설정 저장소를 사용한다.
- 서버 로그에 개인정보, 계정, 비밀번호, 업무 데이터가 남지 않도록 한다.
- 관리자용 재빌드/업로드 API는 초기 버전에서 제공하지 않는다.

## 장애 대응

WPF 온라인 모드에서 서버 호출 실패 시 사용자에게 다음처럼 안내한다.

```text
온라인 답변 서비스에 연결할 수 없습니다.
네트워크 상태 또는 서버 상태를 확인해주세요.
```

선택 가능한 fallback:

- 온라인 고객사: 서버 오류 메시지만 표시
- 하이브리드 고객사: 로컬 KB가 있으면 Local 모드로 임시 전환
- 운영자용: `/health` 결과와 마지막 오류 코드를 표시

## 구현 순서

1. `src/BAStudio.Chatbot.Api` 프로젝트 추가
2. `/health`, `/api/chat` 구현
3. API Token 인증 미들웨어 추가
4. `RemoteChatOrchestratorClient` 구현
5. WPF 설정에 `Mode=Local|Remote` 추가
6. WPF에서 mode에 따라 local/remote orchestrator 선택
7. Dockerfile 및 docker-compose.yml 추가
8. Lightsail 인스턴스에 배포
9. 대표 질문 smoke test 자동화
10. 고객사별 토큰/URL 설정 방식 정리

## 권장 산출물

```text
src/BAStudio.Chatbot.Api/
src/BAStudio.Wpf/Services/RemoteChatOrchestratorClient.cs
deploy/Dockerfile
deploy/docker-compose.yml
deploy/nginx/chatbot.conf
docs/online-service-deployment.md
```

## 최종 구조

오프라인 고객사:

```text
WPF + local ChatOrchestrator + local SQLite KB
```

온라인 고객사:

```text
WPF + RemoteChatOrchestratorClient
AWS Lightsail Docker + ChatOrchestrator + SQLite KB + guide RAG
```

이 구조를 사용하면 UI는 현재 WPF 경험을 유지하면서도, 온라인 고객사의 답변 품질 개선과 KB 업데이트를 서버에서 중앙 관리할 수 있다.
