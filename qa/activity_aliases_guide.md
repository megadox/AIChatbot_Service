# Activity Alias Guide

`activity_aliases.json` is used when a user phrase should deterministically route to a specific activity source.

## When To Add An Alias

Add an alias when a user naturally says something that is not reliably represented in the generated manual text.

Good candidates:

- Korean wording for English activity names, such as `클립보드 값을 가져`.
- Domain-specific wording, such as `애플리케이션 컨트롤을 더블클릭`.
- Action/object phrases that distinguish similar activities.

Avoid aliases that only name an object:

- Bad: `클립보드`
- Better: `클립보드 데이터를 가져`
- Reason: `클립보드` also appears in Set/Clear clipboard questions.

- Bad: `셀 데이터`
- Better: `셀 값을 가져`
- Reason: `셀 데이터` also appears in search/filter/range activities.

## Format

```json
[
  {
    "source": "COMMON/GetClipboard.md",
    "aliases": [
      "클립보드 데이터를 가져",
      "클립보드 값을 가져"
    ]
  }
]
```

After editing aliases, rebuild the KB:

```powershell
dotnet run --project src\Tools.KbBuilder\Tools.KbBuilder.csproj
```

Then run:

```powershell
qa\run_all_activity_tests.bat
qa\run_user_test_cases.bat
```
