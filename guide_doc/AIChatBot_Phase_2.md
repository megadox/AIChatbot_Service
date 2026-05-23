# AI ChatBot Phase 2 설계 정리

## 1. 문서 목적

이 문서는 현재 AI ChatBot 구조를 기준으로, 앞으로 확장할 **업무 프로세스 설명 기반 RPA Task 생성 기능**을 정리한다.

현재 구현의 중심은 “BA-Studio 액티비티와 솔루션 사용법을 근거 기반으로 설명하는 챗봇”이다. Phase 2의 목표는 여기서 한 단계 확장하여, 사용자가 자연어로 업무 절차를 설명하면 챗봇이 적절한 액티비티를 조합하고, 누락 정보를 확인한 뒤, RPA Task 초안을 생성하는 것이다.

## 2. 현재 구현의 위치

현재 구조는 **설명형 도움말 챗봇**에는 적절하다.

핵심 흐름:

```text
사용자 질문
  -> DomainIntentResolver
  -> Embedding
  -> SqliteVectorStore 검색
  -> ChatOrchestrator
  -> 직접 답변 / 명확화 질문 / LLM 기반 답변
  -> WPF UI 스트리밍 출력
```

주요 구성:

| 영역 | 현재 구현 | 역할 |
|---|---|---|
| UI | `BAStudio.Wpf` | 채팅 입력, 출력, 문맥 유지, 답변 수정 |
| Core | `BAStudio.Chatbot` | 계약, 오케스트레이션, 정책, 프롬프트 |
| Infra | `BAStudio.Chatbot.Infra` | 임베딩, SQLite 검색, LLM 추론 |
| KB Builder | `Tools.KbBuilder` | Markdown 액티비티 문서를 SQLite KB로 변환 |
| Test Tool | `Tools.RetrievalSmokeTest` | 검색 품질 및 답변 경로 회귀 확인 |
| QA | `qa/*` | 사용자 테스트 케이스, alias, 답변 수정 데이터 |

현재 구현의 좋은 점:

- 액티비티 설명은 LLM 생성보다 구조화 답변 경로를 우선한다.
- `Summary`, `Metadata`, `Properties`, `Property Notes` 단위로 문서를 청킹한다.
- 도메인 힌트, 액티비티명 힌트, alias, action concept boost를 검색 점수에 반영한다.
- `Click`처럼 여러 도메인에 있는 액티비티는 명확화 질문을 할 수 있다.
- 이전 답변의 근거를 기억하여 “다른 것은?”, “비슷한 것은?”, “차이는?” 같은 후속 질문을 처리한다.
- 답변 수정 내역을 `qa/answer_corrections.jsonl`에 저장하고 다음 KB 빌드에 반영할 수 있다.

현재 구조가 잘 처리하는 질문:

```text
WEB Click 액티비티 사용법 알려줘
엑셀 셀에 값을 쓰는 액티비티는?
Click의 retry 기본값은?
반복하는 액티비티는?
다른 것은?
While과 다른점은?
```

## 3. 현재 구조의 한계

현재 KB는 액티비티를 **문서 검색 대상**으로 다룬다. Phase 2에서 필요한 것은 액티비티를 **조합 가능한 실행 도구**로 다루는 것이다.

현재 문서에 있는 정보:

- 액티비티 이름
- 그룹
- 요약
- script/pattern/dependencies 일부
- 속성명, 타입, 기본값, 옵션, 설명

Phase 2에서 추가로 필요한 정보:

- 필수 입력과 선택 입력의 구분
- 실행 결과 또는 출력 변수
- 선행 조건
- 후행 액티비티 후보
- 함께 사용되는 액티비티
- 대체 가능한 액티비티
- 반복/조건/예외/병렬 같은 제어 구조 의미
- 사용자가 제공해야 하는 업무 데이터
- Task 파일 또는 내부 DSL로 직렬화하는 방법
- 생성된 Task의 유효성 검사 규칙

따라서 현재 구조는 그대로 유지하되, Phase 2에서는 별도의 **계획 생성 계층**을 추가해야 한다.

## 4. Phase 2 목표

Phase 2의 목표는 다음과 같다.

1. 사용자의 업무 설명을 단계별 작업으로 분해한다.
2. 각 작업에 적합한 BA-Studio 액티비티 후보를 찾는다.
3. 액티비티 조합 순서를 만든다.
4. 각 액티비티의 필수 속성값을 채운다.
5. 누락된 값은 사용자에게 질문한다.
6. 생성 가능한 중간 Task 표현을 만든다.
7. 유효성 검사를 통과한 뒤 RPA Task 초안을 출력한다.

목표 흐름:

```text
사용자 업무 설명
  -> Process Intent 분석
  -> 업무 단계 분해
  -> ActivityCatalog 검색
  -> 액티비티 시퀀스 계획
  -> Slot Filling
  -> 누락 정보 질문
  -> Task IR 생성
  -> Validator 검증
  -> RPA Task 초안 생성
```

## 5. 권장 아키텍처

현재 `ChatOrchestrator`는 설명형 질의 처리에 유지한다. Phase 2는 별도 컴포넌트로 분리하는 것이 좋다.

```text
BAStudio.Chatbot
  Contracts
  Orchestration
    ChatOrchestrator
    TaskPlanningOrchestrator
  Policies
    DomainIntentResolver
    ProcessIntentResolver
  Planning
    ProcessDecomposer
    ActivityPlanner
    SlotFiller
    TaskIrBuilder
    TaskValidator
  Prompting
    PromptBuilder
    PlanningPromptBuilder

BAStudio.Chatbot.Infra
  VectorStore
    SqliteVectorStore
    ActivityCatalogStore
  Inference
  Embedding
```

런타임 분기:

```text
질문 유형이 액티비티 설명/사용법이면:
  ChatOrchestrator 사용

질문 유형이 업무 자동화/Task 생성이면:
  TaskPlanningOrchestrator 사용
```

## 6. ActivityCatalog

Phase 2의 핵심은 `ActivityCatalog`이다. 기존 `kb_chunks`는 문서 검색용으로 유지하고, 액티비티 단위의 정규화 테이블을 추가한다.

권장 모델:

```csharp
public sealed record ActivityDefinition(
    string Source,
    string GroupName,
    string ActivityName,
    string Summary,
    string Script,
    string Pattern,
    IReadOnlyList<ActivityProperty> Properties,
    IReadOnlyList<string> Dependencies,
    IReadOnlyList<string> UseCases,
    IReadOnlyList<string> Prerequisites,
    IReadOnlyList<string> Produces,
    IReadOnlyList<string> Consumes,
    IReadOnlyList<string> RelatedActivities);

public sealed record ActivityProperty(
    string Name,
    string Type,
    string DefaultValue,
    IReadOnlyList<string> Options,
    string Description,
    bool Required,
    string? Example);
```

권장 SQLite 테이블:

```sql
CREATE TABLE activity_definitions (
  source TEXT PRIMARY KEY,
  group_name TEXT NOT NULL,
  activity_name TEXT NOT NULL,
  summary TEXT NOT NULL,
  script TEXT NOT NULL,
  pattern TEXT NULL,
  dependencies TEXT NULL,
  use_cases TEXT NULL,
  prerequisites TEXT NULL,
  produces TEXT NULL,
  consumes TEXT NULL,
  related_activities TEXT NULL
);

CREATE TABLE activity_properties (
  source TEXT NOT NULL,
  name TEXT NOT NULL,
  type TEXT NOT NULL,
  default_value TEXT NULL,
  options TEXT NULL,
  description TEXT NULL,
  required INTEGER NOT NULL DEFAULT 0,
  example TEXT NULL,
  PRIMARY KEY(source, name)
);
```

처음에는 기존 Markdown의 `Metadata`와 `Properties`에서 자동 추출하고, 부족한 정보는 `qa/activity_aliases.json`처럼 별도 보강 파일로 관리한다.

예:

```text
qa/activity_catalog_overrides.json
qa/activity_relations.json
qa/activity_use_cases.json
```

## 7. ProcessIntentResolver

현재 `DomainIntentResolver`는 액티비티 조회 중심이다. Phase 2에서는 업무 자동화 요청을 구분하는 resolver가 필요하다.

추가 의도:

| Intent | 설명 |
|---|---|
| `ActivityLookup` | 액티비티 설명 |
| `ActivityRecommendation` | 특정 작업에 쓸 액티비티 추천 |
| `HowTo` | 사용법 또는 절차 설명 |
| `ProcessPlanning` | 업무 절차를 단계로 분해 |
| `TaskGeneration` | RPA Task 초안 생성 |
| `TaskModification` | 기존 Task 수정 |
| `TaskValidation` | 생성된 Task 검증 |
| `Troubleshooting` | 오류 해결 |

예:

```text
"엑셀에서 고객 목록을 읽고 웹사이트에 입력한 뒤 결과를 저장하는 자동화 만들어줘"
  -> TaskGeneration

"웹 버튼 클릭은 어떤 액티비티 써?"
  -> ActivityRecommendation

"WEB Click 속성 알려줘"
  -> ActivityLookup
```

## 8. ProcessDecomposer

사용자 업무 설명을 자동화 가능한 단계로 분해한다.

입력:

```text
엑셀 파일에서 고객 목록을 읽고, 웹 사이트에 접속해서 고객번호를 검색한 뒤,
결과를 다시 엑셀에 저장하는 작업을 자동화하고 싶다.
```

출력 예:

```json
{
  "steps": [
    {
      "goal": "엑셀 파일 열기",
      "domainHint": "EXCEL"
    },
    {
      "goal": "고객 목록 읽기",
      "domainHint": "EXCEL"
    },
    {
      "goal": "웹 사이트 접속",
      "domainHint": "WEB"
    },
    {
      "goal": "고객번호 검색",
      "domainHint": "WEB"
    },
    {
      "goal": "검색 결과 저장",
      "domainHint": "EXCEL"
    }
  ]
}
```

주의:

- 처음부터 최종 Task를 만들지 않는다.
- 단계 분해 결과를 먼저 만들고, 각 단계에 액티비티를 매핑한다.
- 불명확한 단계는 `needsClarification`으로 표시한다.

## 9. ActivityPlanner

`ActivityPlanner`는 분해된 업무 단계에 대해 액티비티 후보를 선택한다.

입력:

```json
{
  "goal": "웹 사이트 접속",
  "domainHint": "WEB"
}
```

출력:

```json
{
  "selected": "WEB/OpenBrowser.md",
  "alternatives": [
    "WEB/Navigate.md",
    "WEB/ChromeAttach.md",
    "WEB/EdgeAttach.md"
  ],
  "reason": "새 브라우저를 열어 URL로 접속하는 단계로 판단됨"
}
```

선택 기준:

- 사용자의 도메인 힌트
- 업무 동사와 액티비티 summary/use case 매칭
- 필수 입력을 채울 수 있는지
- 선행/후행 관계가 맞는지
- 같은 목적의 액티비티 중 더 구체적인 액티비티 우선

## 10. SlotFiller

액티비티가 선택되면 속성값을 채워야 한다.

예:

```json
{
  "activity": "WEB/OpenBrowser.md",
  "slots": {
    "url": null,
    "browserType": "chrome"
  },
  "missing": [
    "url"
  ]
}
```

누락 정보 질문 예:

```text
웹 사이트 접속 단계에 필요한 URL이 없습니다. 접속할 주소를 알려주세요.
```

중요 정책:

- 필수 속성이 없으면 Task를 완성했다고 말하지 않는다.
- 기본값이 있는 속성은 기본값 사용 여부를 명시한다.
- selector, 파일 경로, 시트명, 범위, 로그인 정보처럼 실행 환경 의존 값은 추측하지 않는다.

## 11. Task IR

LLM이 바로 최종 RPA Task 파일을 생성하게 하지 말고, 먼저 중간 표현을 만든다.

권장 Task IR:

```json
{
  "title": "고객 검색 자동화",
  "status": "draft",
  "steps": [
    {
      "id": "step-001",
      "activity": "EXCEL/OpenWorkbook.md",
      "displayName": "엑셀 파일 열기",
      "params": {
        "path": "{{excelPath}}"
      },
      "missingParams": [
        "path"
      ]
    },
    {
      "id": "step-002",
      "activity": "WEB/OpenBrowser.md",
      "displayName": "웹 사이트 접속",
      "params": {
        "url": "{{targetUrl}}"
      },
      "missingParams": [
        "url"
      ]
    }
  ],
  "questions": [
    "엑셀 파일 경로를 알려주세요.",
    "접속할 웹 사이트 URL을 알려주세요."
  ]
}
```

Task IR의 장점:

- 검증하기 쉽다.
- UI에서 단계별 편집이 가능하다.
- 최종 Task 포맷이 바뀌어도 planner를 재사용할 수 있다.
- LLM 환각을 Validator로 걸러낼 수 있다.

## 12. TaskValidator

생성된 Task IR은 반드시 검증한다.

검증 항목:

- activity source가 실제 `ActivityCatalog`에 존재하는가
- 존재하지 않는 property를 사용하지 않았는가
- 필수 property가 누락되지 않았는가
- default가 있는 property는 올바르게 생략 또는 채움 처리했는가
- selector, path, range 등 실행 필수값이 추측으로 채워지지 않았는가
- 제어 구조의 시작/끝 쌍이 맞는가
- `ForEach`, `While`, `TryExcept`, `MultiThread` 같은 블록 액티비티의 중첩 구조가 유효한가
- 같은 객체가 필요한 단계 사이의 연결이 맞는가

검증 결과 예:

```json
{
  "valid": false,
  "errors": [
    {
      "stepId": "step-002",
      "message": "WEB/OpenBrowser.md의 필수 속성 url이 누락되었습니다."
    }
  ],
  "questions": [
    "접속할 웹 사이트 URL을 알려주세요."
  ]
}
```

## 13. 답변 정책

Phase 2에서는 답변을 세 단계로 나누는 것이 좋다.

### 13.1 계획만 가능한 경우

필수 정보가 부족하지만 전체 자동화 흐름은 제안할 수 있는 경우:

```text
다음 흐름으로 자동화할 수 있습니다.

1. EXCEL/OpenWorkbook - 엑셀 파일 열기
2. EXCEL/GetRangeAsCollection - 고객 목록 읽기
3. WEB/OpenBrowser - 웹 사이트 접속
4. WEB/SetValue - 고객번호 입력
5. WEB/Click - 검색 버튼 클릭
6. EXCEL/SetCellValue - 결과 저장

다음 정보가 필요합니다.
- 엑셀 파일 경로
- 읽을 시트명과 범위
- 접속할 URL
- 고객번호 입력칸 selector
- 검색 버튼 selector
```

### 13.2 초안 생성 가능한 경우

필수 정보가 대부분 있는 경우:

```text
RPA Task 초안을 생성했습니다.
아래 단계는 검증을 통과했습니다.

[Task Steps]
...

[확인 필요]
- 검색 결과 영역 selector는 실제 화면에서 다시 확인해야 합니다.
```

### 13.3 생성 불가한 경우

업무 설명이 너무 추상적인 경우:

```text
업무 절차가 아직 구체적이지 않아 Task를 만들 수 없습니다.
먼저 입력 데이터, 대상 프로그램, 반복 기준, 결과 저장 위치를 알려주세요.
```

## 14. UI 확장

현재 WPF 채팅 UI는 설명형 답변에는 충분하다. Task 생성 기능을 넣으려면 다음 UI가 추가되면 좋다.

| UI 요소 | 목적 |
|---|---|
| 모드 선택 | 도움말 / Task 생성 |
| 단계 미리보기 | 생성된 Task IR의 steps 표시 |
| 누락 정보 패널 | 필요한 path, selector, range 등을 입력 |
| 단계별 편집 | 액티비티 변경, 속성값 수정 |
| 검증 결과 패널 | 오류/경고/확인 필요 항목 표시 |
| Task 내보내기 | 최종 RPA Task 파일 또는 스크립트 생성 |

초기에는 채팅만으로 시작하고, Task IR이 안정화된 뒤 편집 UI를 붙이는 순서가 좋다.

## 15. 품질 테스트

현재 `qa/user_test_cases.json`은 액티비티 검색 중심이다. Phase 2에는 별도 테스트 세트가 필요하다.

권장 파일:

```text
qa/task_generation_test_cases.json
qa/task_generation_result_latest.json
```

테스트 케이스 예:

```json
{
  "id": "task-case-001",
  "question": "엑셀 파일에서 고객 목록을 읽고 웹 사이트에 입력하는 자동화 만들어줘",
  "expectedActivities": [
    "EXCEL/OpenWorkbook.md",
    "EXCEL/GetRangeAsCollection.md",
    "WEB/OpenBrowser.md",
    "WEB/SetValue.md"
  ],
  "expectedMissingParams": [
    "excelPath",
    "range",
    "url",
    "selector"
  ]
}
```

통과 기준:

- 기대 액티비티가 계획 안에 포함되어야 한다.
- 존재하지 않는 액티비티를 만들지 않아야 한다.
- 필수 누락값을 질문해야 한다.
- Task IR이 JSON schema 검증을 통과해야 한다.
- Validator 오류가 있는 경우 최종 Task 생성 완료라고 말하지 않아야 한다.

## 16. 구현 순서

### 1단계: 현재 설명형 챗봇 안정화

- `user_test_cases.json` 회귀 유지
- alias와 answer correction 정리
- 직접 답변 경로 품질 개선
- 문맥 유지 후속 질문 테스트 추가

### 2단계: ActivityCatalog 구축

- `Tools.KbBuilder`에서 `activity_definitions`, `activity_properties` 생성
- 기존 Markdown의 Metadata/Properties 파싱 강화
- override JSON으로 부족한 실행 의미 보강

### 3단계: Task IR 정의

- `TaskPlan`, `TaskStep`, `TaskParameter`, `ValidationResult` 계약 추가
- JSON schema 또는 C# validator 작성
- WPF에서 Task IR을 표시할 수 있는 최소 모델 추가

### 4단계: Planner 구현

- `ProcessIntentResolver`
- `ProcessDecomposer`
- `ActivityPlanner`
- `SlotFiller`
- `TaskPlanningOrchestrator`

### 5단계: Validator 구현

- 액티비티 존재 검사
- 속성 존재 검사
- 필수값 검사
- 블록 구조 검사
- 누락 정보 질문 생성

### 6단계: UI 확장

- 도움말 / Task 생성 모드 분기
- 생성된 단계 표시
- 누락 정보 입력
- 검증 결과 표시

### 7단계: Task Export

- 실제 BA-Studio Task 포맷 확인
- Task IR -> 실제 Task 파일 변환기 구현
- export 전 최종 validator 실행

## 17. 최종 방향

현재 구조는 버릴 필요가 없다. Phase 2는 현재 구조 위에 “계획 생성 계층”을 얹는 방식으로 가는 것이 가장 안전하다.

정리하면 다음과 같다.

```text
현재:
  문서 검색 기반 액티비티 설명 챗봇

Phase 2:
  ActivityCatalog 기반 액티비티 조합/계획 생성기

최종:
  설명, 추천, 비교, Task 초안 생성까지 가능한 BA-Studio 내장 RPA Assistant
```

핵심 원칙:

- LLM은 최종 결정자가 아니라 planner의 보조자로 사용한다.
- 액티비티와 속성의 진실은 `ActivityCatalog`에 둔다.
- 생성 결과는 반드시 `Task IR`과 `Validator`를 통과시킨다.
- 누락 정보는 추측하지 말고 사용자에게 질문한다.
- 먼저 계획을 신뢰 가능하게 만들고, 그 다음 실제 Task 파일 생성을 붙인다.
