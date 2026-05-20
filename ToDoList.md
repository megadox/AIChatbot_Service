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
