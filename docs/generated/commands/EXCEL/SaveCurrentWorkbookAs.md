# Activity: SaveCurrentWorkbookAs

## Summary
현재 활성화된 워크북을 다른 이름으로 저장하는 액티비티

## Metadata
- group: `EXCEL`
- script: `SaveCurrentWorkbookAs()`
- pattern: `excel\.SaveCurrentWorkbookAs\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `workbook`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `overwrite` | `string` | `True` | `True`, `False` | 기존 파일명과 이름이 같을 경우 덮어쓰기 할지 여부를 지정합니다.<br/>True: 덮어쓴다.<br/>False: 덮어쓰지 않는다. |
| `path` | `string` | `-` | - | 다른 이름으로 저장할 엑셀 파일의 저장 경로를 지정합니다.<br/>ex) "C:\excel\sample.xlsx" |

## Property Notes
### `overwrite`
기존 파일명과 이름이 같을 경우 덮어쓰기 할지 여부를 지정합니다.
True: 덮어쓴다.
False: 덮어쓰지 않는다.

### `path`
다른 이름으로 저장할 엑셀 파일의 저장 경로를 지정합니다.
ex) "C:\excel\sample.xlsx"

