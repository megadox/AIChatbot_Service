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

## Improve Scenario-based Activity Recommendation - 구현됨

- Implemented current scope.
  - Scenario intent classification now handles "이때 사용하는 것은?", "하고 싶다", "하려면?" style questions.
  - URL/domain, Excel range, file path, and UI automation signals are extracted for retrieval routing.
  - Scenario questions are rewritten into manual-friendly search terms before vector retrieval.
  - Common action concepts now boost explicit sources such as `WEB/Navigate.md`, `WEB/Refresh.md`, `WEB/Click.md`, and `EXCEL/MaxInRange.md`.
  - Added `qa/activity_scenarios.json` so all generated activities can provide scenario terms, negative terms, rewrite terms, and entity hints outside code.
  - `DomainIntentResolver` now loads the scenario map and scores candidates generically instead of relying only on hardcoded activity rules.
  - Strong entity signals can constrain conflicting candidate groups while softer terms remain preference-only to avoid regressions.
  - Regression cases were added to `qa/user_test_cases.json`.

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

### Add Workflow Planning for Multi-step Scenarios

- Add a new user intent for workflow or activity plan requests.
  - Proposed intent: `ActivityPlan`
  - Examples:
    - "네이버에 로그인 할때 사용하는 액티비티는?"
    - "사이트에 로그인하려면?"
    - "엑셀 파일을 열고 특정 범위를 복사하려면?"
    - "메일을 작성해서 첨부파일과 함께 보내려면?"
  - Expected meaning: decompose the user's goal into ordered automation steps and recommend one activity per step, not a single top activity.

- Add workflow template data outside code.
  - Proposed file: `qa/activity_workflows.json`
  - Example workflow: `web-login`
    - Trigger terms: "로그인", "사이트 로그인", "웹 로그인"
    - Required/preferred group: `WEB`
    - Steps:
      - Open target website -> `WEB/OpenBrowser.md`
      - Enter user id -> `WEB/SetValue.md`
      - Enter password -> `WEB/SetValue.md`
      - Click login button -> `WEB/Click.md`

- Extract workflow entities from user questions.
  - Site names and domains:
    - "네이버", `naver.com` -> target URL or site hint.
    - "다음", `daum.net` -> target URL or site hint.
  - Operation terms:
    - "로그인" -> `web-login`
    - "검색" -> later `web-search`
    - "다운로드" -> later `web-download`
  - Keep unknown values as placeholders in the answer.
    - Example: selector for id field, password field, login button.

- Add a workflow retrieval path in `ChatOrchestrator`.
  - If intent is `ActivityPlan`, do not return only the highest ranked activity.
  - Resolve the matching workflow template first.
  - For each step:
    - Use `preferredSource` when the workflow step defines one.
    - Retrieve all source chunks via `GetChunksBySourceAsync`.
    - Keep all step sources in the final grounding set.

- Add a workflow answer builder.
  - Answer should state that the task requires multiple activities.
  - Use ordered steps.
  - For web login, expected answer shape:
    - `WEB/OpenBrowser` for opening `naver.com`.
    - `WEB/SetValue` for entering the ID.
    - `WEB/SetValue` for entering the password.
    - `WEB/Click` for clicking the login button.
  - Include important property guidance:
    - URL
    - selector
    - value/text
  - Include a `[근거]` section listing all source documents.

- Extend regression tests to support multiple expected sources.
  - Current `UserTestCase.ExpectedSource` supports only one source.
  - Add `ExpectedSources` array while keeping `ExpectedSource` backward compatible.
  - A workflow test passes only when all expected sources appear in retrieval or answer grounding.

- Add workflow regression cases.
  - Question: "네이버에 로그인 할때 사용하는 액티비티는?"
  - Expected sources:
    - `WEB/OpenBrowser.md`
    - `WEB/SetValue.md`
    - `WEB/Click.md`

- Add workflow process logs.
  - Example logs:
    - `의도 분석: ActivityPlan`
    - `워크플로우 감지: web-login`
    - `단계 1 근거: WEB/OpenBrowser.md`
    - `단계 2 근거: WEB/SetValue.md`
    - `단계 3 근거: WEB/SetValue.md`
    - `단계 4 근거: WEB/Click.md`

## Add BA-Studio / BA-Assist Product Manual Chat Support

- Implement manual collection and refinement pipeline. - 구현됨
  - Added `Tools.ProductManualCollector`.
  - Source list is maintained in `qa/product_manual_sources.json`.
  - Default sources:
    - BA-Studio 2.6.0: `https://docs.batem.com/b_manual/b_studio_2.6.0.html`
    - BA-Assist 2.5.0: `https://docs.batem.com/b_manual/b_assist_2.5.0.html`
    - BA-Assist 2.5.0 appendix: `https://docs.batem.com/b_manual/b_assist_appendix_2.5.0.html`
  - Generated outputs:
    - Raw HTML: `docs/product-manuals/raw/`
    - Downloaded images: `docs/product-manuals/assets/`
    - Normalized Markdown: `docs/product-manuals/normalized/`
    - Manifest: `docs/product-manuals/manual_manifest.json`
    - Audit report: `docs/product-manuals/reports/manual_audit.json`

- Review current manual audit.
  - BA-Studio 2.6.0: 131 headings, 179 image references, 1 missing image.
  - BA-Assist 2.5.0: 46 headings, 59 image references, 0 missing images.
  - BA-Assist appendix: 4 headings, text-oriented FAQ content.
  - Empty top-level headings are expected for title/TOC sections, but nested empty sections should be reviewed.

- Add manual image notes for important screenshots.
  - Proposed file: `qa/product_manual_image_notes.json`
  - Use for screenshots where the visible UI text is not enough in the HTML.
  - Add captions for menus, dialogs, scheduler screens, settings screens, and task editor screenshots.

- Extend KB builder for product manuals.
  - 구현됨
  - Ingest `docs/product-manuals/normalized/**/*.md`.
  - Add or infer source type metadata:
    - `activity_manual`
    - `product_manual`
    - `qa_correction`
  - Store product, version, source URL, section path, and image references.

- Add a separate intent for product guide questions.
  - 구현됨
  - Proposed intent: `ProductGuide`
  - Examples:
    - "BA-Studio에서 프로젝트는 어떻게 만들어?"
    - "BA-Assist에서 스케줄은 어떻게 등록해?"
    - "BA-Assist 로그는 어디서 봐?"
    - "BA-Studio 셀렉터 편집기는 어떻게 사용해?"
  - Expected meaning: answer about BA-Studio / BA-Assist product usage, not only individual activity properties.

- Extend retrieval routing.
  - 구현됨
  - If intent is `ProductGuide`, search only product manual chunks.
  - If intent is `ActivityLookup` or `ActivityRecommendation`, search activity manual chunks.
  - If the question combines product usage and activity recommendation, search both and keep the answer sections separated.

- Add product guide answer builder.
  - 구현됨
  - Use ordered steps for "how to" questions.
  - Mention product and version in the grounding.
  - Include related screenshot captions when available.
  - Avoid returning a single activity when the user is asking how to use the BA-Studio or BA-Assist UI.

- Add regression cases.
  - 구현됨
  - Question: "BA-Studio에서 새 프로젝트는 어떻게 만들어?"
  - Expected source: `docs/product-manuals/normalized/ba-studio/2.6.0.md`
  - Question: "BA-Assist에서 스케줄은 어떻게 활성화해?"
  - Expected source: `docs/product-manuals/normalized/ba-assist/2.5.0.md`
  - Question: "BA-Assist와 BA-Server 차이는?"
  - Expected source: `docs/product-manuals/normalized/ba-assist/2.5.0-appendix.md`
