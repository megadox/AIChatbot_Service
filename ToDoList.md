# To Do List

## Improve Follow-up Question Handling

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
