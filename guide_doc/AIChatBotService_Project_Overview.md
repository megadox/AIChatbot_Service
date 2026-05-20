# AIChatBotService 프로젝트 개요 (목적/기능/구성)

## 1. 프로젝트 한 줄 정의

**AIChatBotService**는 BA-Studio의 내장 도움말/액티비티 매뉴얼을 기반으로, 로컬 환경에서 **RAG(Retrieval-Augmented Generation)** 방식으로 답변하는 챗봇을 만들기 위한 레포지토리입니다.  
핵심은 “모델이 지식을 학습하는 것”이 아니라, **문서 → KB(SQLite Vector DB) → 검색(Retrieval) → 근거 기반 답변** 파이프라인으로 지식을 반영하고 품질을 확보하는 것입니다.

## 2. 목적(Why)

- **운영 가능한 도움말 챗봇**: BA-Studio 매뉴얼 범위 안에서만 답하고, 근거를 함께 제시하는 챗봇 제공
- **로컬/오프라인 지향**: 네트워크/웹검색 없이도 재현 가능한 지식베이스와 추론(또는 추론 대체 경로) 제공
- **품질 개선 루프 내장**: 문서 생성/KB 빌드/스모크·회귀 테스트로 “검색이 맞는 문서로 가는지”를 반복 점검

## 3. 범위(What)

### 3.1 포함(현재 레포 기준)

- `commands.json`을 소스로 **액티비티 문서(.md) 자동 생성**
- 생성된 문서를 청킹하고 임베딩하여 **SQLite 기반 벡터 DB(`ba_manual_vector.db`) 생성**
- 질문을 임베딩/검색하여 **근거(출처 chunk)를 포함한 답변 생성**
- 검색 품질을 확인하는 **스모크 테스트/회귀 테스트 도구**
- 샘플 UI 호스트로 **WPF 챗 UI** 제공

### 3.2 제외/제한(현재 정책/구현 방향)

- 매뉴얼 밖 일반 지식/추측 답변(금지)
- 웹검색 기반 보강(없음)
- “정답 근거 없는 자신감 있는 답변”(지양, 프롬프트/가드 정책으로 억제)

## 4. 전체 아키텍처 개요

### 4.1 데이터/지식 파이프라인(오프라인)

1) `commands.json` → `Tools.CommandsDocGenerator`  
2) `docs/generated/commands/<group>/<title>.md` 생성  
3) 문서 → `Tools.KbBuilder` → `ChatBot/ba_manual_vector.db` 생성  
4) (선택) `Tools.QaSetGenerator`로 회귀 질문 세트 생성  
5) (선택) `Tools.RetrievalRegression`로 회귀 실행

### 4.2 런타임 질의 파이프라인(온라인: 앱 실행 중)

1) 사용자 질문 입력  
2) 임베딩 생성(`IEmbeddingService`)  
3) 검색(`IVectorStore`): 키워드(FTS) 후보 + 벡터 유사도 재정렬(+ 선택적 MMR)  
4) 오케스트레이션(`IChatOrchestrator`)  
   - 검색 결과 모호성(동명이 문서/유사 파일명) 감지 시 짧은 **명확화 질문** 반환  
   - “X 액티비티”류의 직접 조회는 빠른 **근거 고정 답변** 경로 사용 가능  
5) 프롬프트 구성(`IPromptBuilder`) 후 LLM 스트리밍(`ILlmService`) 또는 룰 기반 응답

## 5. 주요 기능(Feature) 상세

### 5.1 문서 생성: `commands.json` → Markdown

- **도구**: `src/Tools.CommandsDocGenerator`
- **산출물**: `docs/generated/commands/<group>/<title>.md`
- **목적**: KB가 읽기 좋은 표준 포맷(제목/요약/메타데이터/속성 표/속성 노트)을 확보하여 검색/인용 품질을 올림

### 5.2 KB(Vector DB) 생성: Markdown → SQLite(`ba_manual_vector.db`)

- **도구**: `src/Tools.KbBuilder`
- **산출물**: `ChatBot/ba_manual_vector.db`
- **핵심 동작**:
  - 문서를 청크로 분할(겹침/최대 길이 등 옵션)
  - 임베딩 생성(환경에 따라 ONNX 또는 해시 임베딩)
  - SQLite에 chunk/embedding 저장(런타임 검색의 기반)

### 5.3 검색(Retrieval): 키워드+벡터 하이브리드, 선택적 MMR

- **구현**: `src/BAStudio.Chatbot.Infra/VectorStore/SqliteVectorStore.cs`
- **전략**:
  - FTS5가 존재하면 **키워드 기반 후보**를 먼저 빠르게 수집(“Click 같은 짧은 토큰” 대응)
  - 후보를 벡터 유사도로 재정렬(정규화 벡터 dot/cosine 형태)
  - 필요 시 MMR로 다양성 확보(중복 문서/유사 chunk 과다 선택 완화)
  - 쿼리에서 액티비티 토큰 힌트를 추출해 `.../Process.md` 같은 **소스 힌트 후보를 강하게 부스팅**
  - 한국어 조사(의/은/는 등)로 검색이 깨지지 않게 토큰화 보정

### 5.4 프롬프트/가드: “매뉴얼 근거만” + “[근거] 섹션 강제”

- **구현**: `src/BAStudio.Chatbot/Prompting/PromptBuilder.cs`
- **정책**:
  - 시스템 메시지에서 **매뉴얼 밖은 추측 금지**
  - 근거 부족 시 “문서에 근거가 부족합니다” + 추가 정보 질문 유도
  - “어떤 액티비티를 써야 하나” 유형은 Summary/용도 비교 중심으로 후보 제시
  - 답변 마지막에 **[근거] 섹션**을 만들고 사용한 chunk를 bullet로 나열하도록 유도

### 5.5 오케스트레이션: 모호성 처리 + 빠른 근거 고정 응답

- **구현**: `src/BAStudio.Chatbot/Orchestration/ChatOrchestrator.cs`
- **주요 동작**:
  - 검색 결과 상위에 동명이 문서(서로 다른 그룹) 또는 유사 파일명이 섞이면, 곧바로 **명확화 질문**을 반환
  - “액티비티/Activity” 직접 조회 질문은 LLM 생성 없이도 빠르게 요약/속성/근거를 구성하는 **grounded path** 제공

### 5.6 추론(LLM): 로컬 GGUF 모델 스트리밍 또는 룰 기반 대체

- **LLamaSharp 기반**: `src/BAStudio.Chatbot.Infra/Inference/LlamaSharpLlmService.cs`
  - GGUF 모델 로드(로컬 파일), 스트리밍 생성
  - 기본 샘플링(Temperature/TopP/TopK), AntiPrompt 설정
- **룰 기반(검증용)**: `src/BAStudio.Chatbot.Infra/Inference/RuleBasedLlmService.cs`
  - end-to-end wiring(프롬프트 생성/스트리밍)이 정상인지 확인하는 최소 구현

### 5.7 샘플 호스트 UI: WPF 챗 화면

- **프로젝트**: `src/BAStudio.Wpf`
- **구성**:
  - 입력(TextBox) + 전송/취소 버튼 + 메시지 리스트
  - `ChatViewModel`이 `IChatOrchestrator`로부터 토큰 스트리밍을 받아 UI에 누적 표시

## 6. 구성 프로젝트(폴더) 요약

- `src/BAStudio.Chatbot`
  - Contracts(요청/응답 모델, 인터페이스)
  - Prompting, Orchestration 등 핵심 도메인 로직
- `src/BAStudio.Chatbot.Infra`
  - Embedding(ONNX/Hashing), VectorStore(SQLite), Inference(LLamaSharp/RuleBased)
- `src/BAStudio.Wpf`
  - 샘플 UI 호스트(챗 UI)
- `src/Tools.CommandsDocGenerator`
  - `commands.json` → Markdown 문서 생성기
- `src/Tools.KbBuilder`
  - 문서 청킹/임베딩/DB 생성기
- `src/Tools.RetrievalSmokeTest`
  - 단건 질문으로 리트리벌 결과(점수/출처/섹션) 빠른 확인
- `src/Tools.QaSetGenerator`
  - 생성된 문서로부터 회귀 질문 세트 생성
- `src/Tools.RetrievalRegression`
  - 질문 세트를 일괄 실행해 Top-N 내 정답 source 포함 여부를 평가
- `src/Tools.OnnxInspector`
  - ONNX 모델 I/O 점검(텐서명/형상 확인 목적)

## 7. 실행/운영 관점의 핵심 파일

### 7.1 리소스 폴더(기본: `ChatBot/`)

- `ChatBot/ba_manual_vector.db`: 벡터 DB(검색 대상)
- `ChatBot/embedding_model.onnx`: 임베딩 ONNX(옵션, 없으면 해시 임베딩 폴백 가능)
- `ChatBot/tokenizer.vocab.txt`: WordPiece vocab(임베딩 토크나이저)
- `ChatBot/<model>.gguf`: LLM 추론용 GGUF 모델

### 7.2 앱 설정 예시

샘플 앱(WPF)은 `src/BAStudio.Wpf/appsettings.json`의 `Chatbot` 섹션으로 경로를 구성합니다.

## 8. 품질 확보(테스트/체크리스트)

- **문서 생성/KB 재생성 절차**를 표준화하고, 변경 시 항상 동일 루틴으로 업데이트
- `Tools.RetrievalSmokeTest`로 대표 질문에 대해 TopK가 기대 문서로 가는지 점검
- `Tools.RetrievalRegression`으로 변경 전/후 검색 품질 회귀 확인
- 레포의 체크리스트 문서(`CHATBOT_QUALITY_CHECKLIST.md`)를 기준으로
  - 데이터/임베딩/검색/생성/UX/관측/보안 항목을 단계적으로 강화

## 9. 설계 의도(요약)

- **검색이 답변 품질을 결정**하는 구조이므로, 문서 포맷·청킹·임베딩 정합성·FTS 쿼리/힌트·MMR 튜닝이 핵심 레버입니다.
- LLM은 “답을 만들어내는 엔진”이 아니라, **검색된 근거를 사용자 친화적으로 요약/구성하는 마지막 단계**로 취급합니다.
- 구현/운영 단위로는 “문서 생성 → KB 생성 → (스모크/회귀) → 앱”의 반복을 가장 중요하게 둡니다.

