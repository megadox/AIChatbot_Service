# AI ChatBot Service

BA-Studio 내장형 로컬 RAG 챗봇 재구현 프로젝트입니다. WPF는 .NET 8.0(`net8.0-windows`)을 사용합니다.

## 구성

- `src/BAStudio.Chatbot`: RAG 오케스트레이션, 정책, 프롬프트, 계약
- `src/BAStudio.Chatbot.Infra`: 해시 임베딩, SQLite 검색, Phi-4 GGUF LLamaSharp 추론 서비스
- `src/BAStudio.Wpf`: .NET 8 WPF 챗 UI
- `src/Tools.KbBuilder`: `docs/generated/commands/**/*.md`를 SQLite KB로 변환
- `src/Tools.RetrievalSmokeTest`: 단건 검색 품질 확인 도구
- `ChatBot/ba_manual_vector.db`: 생성된 로컬 KB
- `model/microsoft_Phi-4-mini-instruct-Q4_K_M.gguf`: 런타임 LLM 모델

## 빌드

```powershell
dotnet restore AIChatBotService.slnx --configfile NuGet.Config
dotnet build AIChatBotService.slnx --no-restore
```

## KB 생성

```powershell
dotnet run --project src/Tools.KbBuilder/Tools.KbBuilder.csproj
```

기본 출력:

```text
ChatBot/ba_manual_vector.db
```

## 검색 확인

```powershell
dotnet run --project src/Tools.RetrievalSmokeTest/Tools.RetrievalSmokeTest.csproj -- "웹 Click 액티비티 사용법 알려줘"
dotnet run --project src/Tools.RetrievalSmokeTest/Tools.RetrievalSmokeTest.csproj -- "엑셀 셀에 값 입력"
```

## WPF 실행

```powershell
dotnet run --project src/BAStudio.Wpf/BAStudio.Wpf.csproj
```

WPF는 `ChatBot/ba_manual_vector.db`를 검색하고, `model/microsoft_Phi-4-mini-instruct-Q4_K_M.gguf`가 있으면 LLamaSharp 기반 Phi-4-mini 추론 서비스를 사용합니다. 직접 조회형 질문은 LLM 호출 전에 구조화 답변으로 처리됩니다.

## 답변 평가/수정

WPF의 채팅 메시지는 질문/답변 모두 텍스트 선택과 복사가 가능합니다.

답변을 받은 뒤 입력창 옆의 `답변` 버튼을 누르면 마지막 질문/답변을 평가하고 수정할 수 있습니다. 저장된 수정 내역은 아래 파일에 JSONL 형식으로 누적됩니다.

```text
qa/answer_corrections.jsonl
```

다음 KB 빌드 시 `Tools.KbBuilder`가 이 파일을 읽어 `answer_correction` chunk로 반영합니다.

수정 창에서 `수정된 답변` 영역에는 챗봇이 다음부터 그대로 사용해도 되는 정답 문장을 적습니다. `수정` 또는 `나쁨`으로 저장할 때 이 값이 원래 답변과 같으면 보정 답변으로 반영되지 않습니다.

`기대 근거/힌트`는 답변 본문으로 출력하지 않는 보조 정보입니다. 여기에 `WIN32/ClickAutomation.md`처럼 실제 문서 경로를 쓰거나, 문서 경로를 모를 때는 `클릭, 컨트롤` 같은 검색 힌트를 적을 수 있습니다. 단, 힌트만 입력하고 `수정된 답변`을 바꾸지 않으면 기존 답변을 덮어쓰지 않습니다.

수정 내역을 반영하려면 KB를 다시 생성합니다.

```powershell
dotnet run --project src/Tools.KbBuilder/Tools.KbBuilder.csproj
```
