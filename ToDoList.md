# To Do List

## Improve Follow-up Question Handling - 진행중
- Add a new user intent for alternative requests.
  - Examples: "다른것은?", "다른 액티비티는?", "또 있어?", "다른 방법은?", "Outlook 말고"
  - Expected meaning: reuse the previous question context, exclude the already answered source, and return the next relevant activity.

- Extend conversation grounding.
  - Store the previous question text.
  - Store the previous top search results, not only the best source.
  - Store the selected source, preferred group, activity name, and action concept.

- Add an alternative retrieval path in `ChatOrchestrator`.
  - Do not search with the literal query "다른것은?".
  - Reuse the previous question as the search query.
  - Keep the previous domain as `PreferredGroup`.
  - Exclude the previously answered source.
  - Prefer same-purpose/same-action candidates.

- Extend `SearchRequest`.
  - Add `ExcludeSources` or `AvoidSources`.
  - Apply source exclusion in `SqliteVectorStore.SearchAsync`.

- Add same-purpose activity grouping later.
  - Example group: send email
  - Sources:
    - `EMAIL/SendMail(Outlook).md`
    - `EMAIL/SendMail(SMTP).md`

- Improve first answers when alternatives exist.
  - If multiple same-purpose sources are found, mention the alternative source briefly.
  - Example: `EMAIL/SendMail(SMTP).md` is also available for sending email.

- Add regression cases.
  - First question: "이메일을 보내는"
  - Follow-up: "다른것은?"
  - Expected source: `EMAIL/SendMail(SMTP).md`

- Add a new user intent for comparison requests.
  - Examples: "ForEach와 While을 비교해줘", "ForEach랑 While 차이는?", "A vs B", "A와 B의 차이점"
  - Expected meaning: resolve two or more explicit activity targets, retrieve grounding for each target separately, and generate a side-by-side comparison.

- Extract comparison targets from the question.
  - Normalize Korean particles and connectors.
  - Example: "ForEach와 While을 비교해줘" -> `ForEach`, `While`
  - Resolve each target to exact activity sources when possible.
  - Example sources:
    - `BuiltIn/ForEach.md`
    - `BuiltIn/While.md`

- Add a comparison retrieval path in `ChatOrchestrator`.
  - Do not search the whole phrase as one query when comparison targets are explicit.
  - Search or resolve each activity independently.
  - Retrieve `summary`, `properties`, and important `property_note` chunks for each source.
  - Keep both sources in the final grounding set.

- Add a comparison answer builder.
  - Answer structure should explain the main difference first.
  - For `ForEach` vs `While`, emphasize:
    - `ForEach`: repeats over collection elements.
    - `While`: repeats while a condition is satisfied.
  - Include usage guidance and a `[근거]` section with both sources.

- Add comparison process logs.
  - Example logs:
    - `의도 분석: Compare`
    - `비교 대상: BuiltIn/ForEach.md, BuiltIn/While.md`
    - `ForEach 근거 검색: Summary, Properties`
    - `While 근거 검색: Summary, Properties`

- Add comparison regression cases.
  - Question: "ForEach와 While을 비교해줘"
  - Expected sources:
    - `BuiltIn/ForEach.md`
    - `BuiltIn/While.md`

## Improve Scenario-based Activity Recommendation

- Add stronger intent classification for scenario questions.
  - Examples: "이때 사용하는 것은?", "어떤 액티비티를 써야 하나?", "하고 싶다", "하려면?"
  - Expected meaning: infer the user's desired task and recommend the best matching activity, not only search literal words.

- Extract domain-specific entities from user questions.
  - URL/domain examples: `naver.com`, `daum.net`, `https://...` -> `WEB`
  - Excel range examples: `A1:B10`, `셀`, `시트`, `범위` -> `EXCEL`
  - File path examples: `C:\...`, `파일`, `폴더`, `경로` -> `FILE`
  - UI automation examples: `마우스`, `좌표`, `창`, `컨트롤` -> `WIN32`

- Add a query rewrite step before retrieval.
  - Convert natural scenario descriptions into search-friendly manual terms.
  - Example:
    - Original: "naver.com에서 daum.net으로 이동하고 싶다. 이때 사용하는 것은?"
    - Rewritten: "WEB 브라우저 URL 사이트 이동 Navigate url 새로운 웹 사이트로 이동"

- Add an action concept map for common task expressions.
  - "사이트 이동", "URL 변경", "페이지 열기", "접속" -> `WEB/Navigate.md`
  - "현재 페이지 새로고침" -> `WEB/Refresh.md`
  - "웹 요소 클릭" -> `WEB/Click.md`
  - "셀 범위 최대값" -> `EXCEL/MaxInRange.md`

- Improve candidate validation and reranking.
  - Penalize or exclude candidates whose domain conflicts with strong entity signals.
  - Example: if the question contains URL domains, reject `EXCEL/MaxInRange.md` unless there is explicit Excel context.
  - Avoid direct answers when the top candidate has no keyword, alias, domain, or action-concept support.

- Extend `SearchRequest` or add a query analysis model.
  - Include original question, rewritten query, required/preferred group, extracted entities, and action concept hints.
  - Keep the original question for answer wording and logs.

- Keep source-level chunk enrichment after the activity is selected.
  - Search decides the activity source.
  - Then retrieve all chunks for that source: `Summary`, `Metadata`, `Properties`, and `Property Notes`.
  - Do not depend on the top retrieval pool for property output.

- Add regression cases for scenario recommendations.
  - Question: "naver.com에서 daum.net으로 이동하고 싶다. 이때 사용하는 것은?"
  - Expected source: `WEB/Navigate.md`
  - Question: "현재 웹 페이지를 다시 불러오고 싶다"
  - Expected source: `WEB/Refresh.md`

## Add Solution Usage and Guide Chat Support

- Add a separate intent for solution/product guide questions.
  - Examples: "이 프로그램은 어떻게 실행해?", "KB는 어떻게 빌드해?", "대화 세션은 어디에 저장돼?", "답변 수정은 어떻게 반영돼?"
  - Expected meaning: answer about the AI Chatbot solution itself, not BA-Studio activity manuals.

- Separate guide knowledge from activity manual knowledge.
  - Activity manual KB: `docs/generated/commands/**/*.md`
  - Solution guide KB candidates:
    - `README.md`
    - `guide_doc/**/*.md`
    - `ToDoList.md`
    - generated operation guide files
  - Keep source type metadata: `activity_manual`, `solution_guide`, `qa_correction`.

- Add a solution guide document set.
  - App overview and purpose
  - How to run the WPF chatbot
  - How to build or rebuild the manual vector DB
  - How to configure model path and KB path
  - How chat sessions are saved and loaded
  - How answer corrections are stored and reflected
  - How to run retrieval smoke tests

- Extend KB builder or add a second guide KB builder path.
  - Option A: store guide docs in the same SQLite DB with a new source type.
  - Option B: maintain a separate `solution_guide_vector.db`.
  - Prefer Option A first if source type filtering is added.

- Extend retrieval routing.
  - If intent is `SolutionGuide`, search only solution guide sources.
  - If intent is `ActivityLookup` or `ActivityRecommendation`, search activity manual sources.
  - If intent is ambiguous, search both and rerank by intent confidence.

- Add a guide answer builder.
  - Use procedural step format for "how to" questions.
  - Use short explanation plus paths for "where is stored" questions.
  - Include exact local file paths or commands when available.
  - Avoid mixing activity properties into solution guide answers.

- Add process logs for guide questions.
  - Example logs:
    - `의도 분석: SolutionGuide`
    - `가이드 문서 검색: README.md, guide_doc/...`
    - `솔루션 사용법 답변 생성`

- Add guide regression cases.
  - Question: "KB는 어떻게 다시 빌드해?"
  - Expected source: `README.md` or guide doc with KB build command.
  - Question: "대화 세션은 어디에 저장돼?"
  - Expected source: guide doc describing `ChatBot/conversations/`.
  - Question: "답변 수정은 어디에 저장돼?"
  - Expected source: guide doc or `qa/answer_corrections.jsonl` explanation.

- Add user-facing help entry later.
  - Provide a small "도움말" button or command.
  - Show examples grouped by:
    - Activity lookup
    - Scenario recommendation
    - Comparison
    - Follow-up questions
    - Solution usage
