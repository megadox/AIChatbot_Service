# AI ChatBot Service

BA Chatbot은 BA-Studio와 BA-Assist 사용자를 위한 AI 도움말 챗봇 프로젝트입니다. WPF 데스크톱 앱, ASP.NET Core API, 로컬 RAG 엔진, 제품 매뉴얼 수집기, KB 빌더, QA 테스트 데이터를 함께 관리합니다.

## 주요 기능

- BA-Studio/BA-Assist 제품 매뉴얼과 액티비티 문서를 기반으로 질문에 답변합니다.
- WPF 챗봇 UI에서 Online API 모드와 Offline 로컬 모드를 전환할 수 있습니다.
- 액티비티/Task, BA-Studio 사용법, BA-Assist 사용법, 일반 질문 유형을 구분합니다.
- 문맥 유지, 웹 검색, 답변 복사, 답변 피드백/수정, 대화 세션 저장을 지원합니다.
- ASP.NET Core API는 `/health`, `/api/chat`, `/api/chat/stream` 엔드포인트를 제공합니다.
- KB 빌더는 생성 문서, 제품 가이드, 정규화된 매뉴얼, 답변 수정 내역을 SQLite KB로 빌드합니다.
- QA 데이터와 테스트 스크립트로 사용자 질문, 기능 라우팅, 제품 가이드 답변을 검증합니다.

## 프로젝트 구조

```text
AI_ChatBot_Service/
├─ src/
│  ├─ BAStudio.Wpf/                  # BA Chatbot WPF 데스크톱 앱
│  ├─ BAStudio.Chatbot.Api/          # 원격/로컬 배포용 ASP.NET Core Chatbot API
│  ├─ BAStudio.Chatbot/              # RAG 오케스트레이션, 정책, 프롬프트, 계약
│  ├─ BAStudio.Chatbot.Infra/        # SQLite vector store, embedding, LLM, web search 구현
│  ├─ Tools.KbBuilder/               # 문서와 피드백을 SQLite KB로 변환
│  ├─ Tools.ProductManualCollector/  # 제품 매뉴얼 수집/정규화 도구
│  └─ Tools.RetrievalSmokeTest/      # 단건 검색 품질 확인 도구
├─ ChatBot/
│  └─ ba_manual_vector.db            # 생성된 로컬 KB
├─ docs/
│  ├─ generated/commands/            # 액티비티별 생성 문서
│  ├─ product-manuals/               # 원본/정규화/가이드/자산/리포트
│  ├─ quality/                       # 사용자 검증 체크리스트
│  ├─ chatbot-ui-improvement-plan.md # WPF UI 개선 구현안
│  └─ chatbot-installer-todolist.md  # 설치 패키지 분리 계획
├─ qa/
│  ├─ feature_tests/                 # 기능별 테스트 케이스
│  ├─ user_test_cases.json           # 사용자 질문 테스트 케이스
│  ├─ answer_corrections.jsonl       # 답변 평가/수정 누적 데이터
│  └─ *.bat, *.ps1                   # QA 실행 스크립트
├─ deploy/
│  ├─ Dockerfile
│  ├─ docker-compose.yml
│  └─ nginx/chatbot.conf
├─ AIChatBotService.slnx
├─ NuGet.Config
└─ VERSION
```

## 주요 프로젝트

| 경로 | 역할 |
| --- | --- |
| `src/BAStudio.Wpf` | 사용자가 실행하는 WPF 챗봇 앱입니다. 세션 목록, 채팅 UI, 설정 창, Online/Offline 모드 설정, 답변 피드백 UX를 포함합니다. |
| `src/BAStudio.Chatbot.Api` | WPF Online 모드와 서버 배포에서 사용하는 ASP.NET Core API입니다. 토큰 인증, health check, 일반 응답, SSE streaming 응답을 제공합니다. |
| `src/BAStudio.Chatbot` | 질문 처리 흐름의 중심입니다. 채팅 계약, 인터페이스, 검색 정책, 프롬프트 구성, 답변 생성 orchestration을 담당합니다. |
| `src/BAStudio.Chatbot.Infra` | 해시 임베딩, SQLite 검색, Phi-4 GGUF LLamaSharp 추론, grounded fallback, 웹 검색 구현을 포함합니다. |
| `src/Tools.KbBuilder` | `docs/generated/commands`, 제품 매뉴얼, 제품 가이드, 답변 수정 데이터를 읽어 `ChatBot/ba_manual_vector.db`를 생성합니다. |
| `src/Tools.ProductManualCollector` | BA-Studio/BA-Assist 온라인 매뉴얼을 수집하고 markdown, 이미지 자산, manifest, audit report로 정규화합니다. |
| `src/Tools.RetrievalSmokeTest` | 특정 질문에 대해 KB 검색 결과를 빠르게 확인하는 개발용 도구입니다. |

## 빌드

```powershell
dotnet restore AIChatBotService.slnx --configfile NuGet.Config
dotnet build AIChatBotService.slnx --no-restore
```

WPF만 빌드하려면 다음 명령을 사용합니다.

```powershell
dotnet build src/BAStudio.Wpf/BAStudio.Wpf.csproj
```

## WPF 실행

```powershell
dotnet run --project src/BAStudio.Wpf/BAStudio.Wpf.csproj
```

WPF 앱은 설정에 따라 두 가지 방식으로 동작합니다.

- `Remote`: WPF는 화면만 담당하고 BA Chatbot API로 질문을 보냅니다.
- `Local`: WPF 프로세스 안에서 로컬 KB와 로컬 답변 엔진을 사용합니다.

로컬 모드에서 `model/microsoft_Phi-4-mini-instruct-Q4_K_M.gguf` 파일이 있으면 LLamaSharp 기반 Phi-4-mini 추론 서비스를 사용합니다. 모델 파일이 없으면 검색 근거 기반 fallback 답변 서비스를 사용합니다.

## API 실행

```powershell
dotnet run --project src/BAStudio.Chatbot.Api/BAStudio.Chatbot.Api.csproj
```

주요 엔드포인트:

| Method | Path | 설명 |
| --- | --- | --- |
| `GET` | `/health` | API 상태, 버전, build 정보, KB 로드 상태를 반환합니다. |
| `POST` | `/api/chat` | 질문에 대한 전체 응답과 이벤트 목록을 JSON으로 반환합니다. |
| `POST` | `/api/chat/stream` | Server-Sent Events 방식으로 status/token 이벤트를 스트리밍합니다. |

토큰 인증은 `Chatbot:ApiTokens` 설정 또는 `CHATBOT_API_TOKENS` 환경 변수로 구성합니다. 토큰이 설정된 경우 `/health`를 제외한 API 요청은 `Authorization: Bearer {token}` 헤더가 필요합니다.

## KB 생성

```powershell
dotnet run --project src/Tools.KbBuilder/Tools.KbBuilder.csproj
```

기본 출력:

```text
ChatBot/ba_manual_vector.db
```

입력 데이터:

- `docs/generated/commands/**/*.md`
- `docs/product-manuals/guides/**/*.md`
- `docs/product-manuals/normalized/**/*.md`
- `qa/activity_aliases.json`
- `qa/answer_corrections.jsonl`

옵션:

```powershell
dotnet run --project src/Tools.KbBuilder/Tools.KbBuilder.csproj -- --docs docs/generated/commands --out ChatBot/ba_manual_vector.db
```

## 제품 매뉴얼 수집

```powershell
dotnet run --project src/Tools.ProductManualCollector/Tools.ProductManualCollector.csproj
```

기본 소스는 `qa/product_manual_sources.json`을 사용합니다. 수집 결과는 `docs/product-manuals` 아래에 저장됩니다.

- `raw/`: 원본 HTML
- `assets/`: 다운로드한 이미지
- `normalized/`: 정규화된 markdown
- `manual_manifest.json`: 수집 manifest
- `reports/manual_audit.json`: 수집 품질 리포트

이미 받은 원본을 재사용하려면 다음 옵션을 사용합니다.

```powershell
dotnet run --project src/Tools.ProductManualCollector/Tools.ProductManualCollector.csproj -- --skip-download
```

## 검색 확인

```powershell
dotnet run --project src/Tools.RetrievalSmokeTest/Tools.RetrievalSmokeTest.csproj -- "웹 Click 액티비티 사용법 알려줘"
dotnet run --project src/Tools.RetrievalSmokeTest/Tools.RetrievalSmokeTest.csproj -- "엑셀 셀에 값 입력"
```

## 답변 평가/수정

WPF에서 답변을 받은 뒤 `피드백` 버튼을 누르면 마지막 질문/답변을 평가하고 수정할 수 있습니다. 저장된 수정 내역은 JSONL 형식으로 누적됩니다.

```text
qa/answer_corrections.jsonl
```

다음 KB 빌드 시 `Tools.KbBuilder`가 이 파일을 읽어 `answer_correction` chunk로 반영합니다.

수정 창에서 `수정된 답변`에는 챗봇이 다음부터 그대로 사용해도 되는 정답 문장을 적습니다. `기대 근거/힌트`에는 `WIN32/ClickAutomation.md` 같은 문서 경로 또는 검색 힌트를 적을 수 있습니다.

수정 내역을 반영하려면 KB를 다시 생성합니다.

```powershell
dotnet run --project src/Tools.KbBuilder/Tools.KbBuilder.csproj
```

## QA

QA 데이터는 `qa/` 아래에 있습니다.

```powershell
qa/run_feature_tests.ps1
qa/run_feature_tests.bat
qa/run_user_test_cases.bat
qa/run_all_activity_tests.bat
```

주요 테스트 데이터:

- `qa/user_test_cases.json`
- `qa/feature_tests/question_type_routing_cases.json`
- `qa/feature_tests/product_guide_cases.json`
- `qa/feature_tests/activity_task_cases.json`
- `qa/activity_scenarios.json`

## 배포와 설치 패키지

현재 배포 관련 자료는 `deploy/`와 `docs/`에 정리되어 있습니다.

- `deploy/Dockerfile`: BA Chatbot API 컨테이너 빌드
- `deploy/docker-compose.yml`: API 실행 구성
- `deploy/nginx/chatbot.conf`: reverse proxy 예시
- `deploy/publish-ecr-lightsail.ps1`: AWS ECR/Lightsail 배포 스크립트
- `docs/aws-ecr-lightsail-docker-deployment.md`: AWS 배포 가이드
- `docs/online-service-deployment.md`: 온라인 서비스 배포 가이드
- `docs/chatbot-installer-todolist.md`: Online, Offline, API Add-on 패키지 분리 계획

설치 패키지 목표:

- Online Package: WPF 앱만 포함하고 원격 API를 사용합니다.
- Offline Package: WPF 앱과 로컬 BA Chatbot API, KB를 함께 포함합니다.
- API Add-on Package: Online 설치본에 로컬 API 파일을 추가해 Offline처럼 사용할 수 있게 합니다.

## 설정과 런타임 파일

주요 파일:

- `ChatBot/ba_manual_vector.db`: 로컬 KB
- `ChatBot/appsettings.example.json`: 챗봇 설정 예시
- `VERSION`: 앱/패키지 버전
- `commands.json`: 액티비티 문서 생성의 원천 데이터
- `NuGet.Config`: 패키지 복원 설정

로컬 LLM을 사용할 경우 모델 파일은 다음 위치를 기본으로 사용합니다.

```text
model/microsoft_Phi-4-mini-instruct-Q4_K_M.gguf
```
