# Activity: CellByName

## Summary
특정 셀을 선택할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `CellByName()`
- pattern: `excel\.CellByName\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `columnName` | `string` | `"A"` | - | 선택할 열의 값을 엑셀 표시 방식으로 지정합니다.<br/>ex) "B" |
| `row` | `string` | `1` | - | 선택할 행의 값을 엑셀 표시 방식으로 지정합니다.<br/>ex) 13 |
| `select` | `string` | `True` | `True`, `False` | 선택 셀의 활성화 여부를 지정합니다.<br/>True: 활성화된 커서의 위치는 변경하지 않고 이전 상태를 유지한다.<br/>False: 커서의 위치를 columnName과 row에 지정한 셀로 이동한다. |

## Property Notes
### `columnName`
선택할 열의 값을 엑셀 표시 방식으로 지정합니다.
ex) "B"

### `row`
선택할 행의 값을 엑셀 표시 방식으로 지정합니다.
ex) 13

### `select`
선택 셀의 활성화 여부를 지정합니다.
True: 활성화된 커서의 위치는 변경하지 않고 이전 상태를 유지한다.
False: 커서의 위치를 columnName과 row에 지정한 셀로 이동한다.

