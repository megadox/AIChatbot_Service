# Activity: DeleteCellRange

## Summary
셀의 특정 영역의 데이터를 삭제할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `DeleteCellRange()`
- pattern: `excel\.DeleteCellRange\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `range` | `string` | `-` | - | 삭제할 영역을 지정합니다.<br/>ex) "E2:E3" |
| `shift` | `string` | `"LEFT"` | `"Left"`, `"Up"` | 삭제 후 이동 방향을 지정합니다.<br/>"Left": 삭제된 열을 기준으로 우측에 위치한 모든 열이 왼쪽으로 이동한다.<br/>"Up": 삭제된 행을 기준으로 아래에 위치한 모든 행이 위쪽으로 이동한다. |

## Property Notes
### `range`
삭제할 영역을 지정합니다.
ex) "E2:E3"

### `shift`
삭제 후 이동 방향을 지정합니다.
"Left": 삭제된 열을 기준으로 우측에 위치한 모든 열이 왼쪽으로 이동한다.
"Up": 삭제된 행을 기준으로 아래에 위치한 모든 행이 위쪽으로 이동한다.

