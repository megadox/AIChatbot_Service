# Activity: End

## Summary
현재 실행 중인 RPA 프로세스를 정상적으로 종료시키는 액티비티

## Metadata
- group: `COMMON`
- script: `end_engine()`
- pattern: `end_engine\(`
- theme: `Dark_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `endScope` | `string` | `"All"` | `"All"`, `"Current"` | 종료 범위를 지정합니다.<br/>"All": 현재 실행중인 모든 프로세스를 종료한다.<br/>"Current": 현재 태스크만 종료한다.<br/>(기본값: "All") |

## Property Notes
### `endScope`
종료 범위를 지정합니다.
"All": 현재 실행중인 모든 프로세스를 종료한다.
"Current": 현재 태스크만 종료한다.
(기본값: "All")

