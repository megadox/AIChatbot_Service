# Activity: SetCellValue

## Summary
특정 셀에 값 또는 수식을 입력하는 액티비티

## Metadata
- group: `EXCEL`
- script: `SetCellValue()`
- pattern: `excel\.SetCellValue\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `cell` | `string` | `-` | - | 값을 입력할 셀을 엑셀 표시 방식으로 지정합니다.<br/>ex) "A1"<br/>(빈 값이면 row와 column 필드의 값으로 셀을 지정한다.) |
| `column` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 값을 입력할 열의 값을 인덱스 방식으로 지정합니다.<br/>ex) 3 |
| `row` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 값을 입력할 행의 값을 인덱스 방식으로 지정합니다.<br/>ex) 0 |
| `value` | `string` | `-` | - | 입력할 값을 지정합니다.<br/>ex) 1234 또는 "ABCD" |

## Property Notes
### `cell`
값을 입력할 셀을 엑셀 표시 방식으로 지정합니다.
ex) "A1"
(빈 값이면 row와 column 필드의 값으로 셀을 지정한다.)

### `column`
값을 입력할 열의 값을 인덱스 방식으로 지정합니다.
ex) 3

### `row`
값을 입력할 행의 값을 인덱스 방식으로 지정합니다.
ex) 0

### `value`
입력할 값을 지정합니다.
ex) 1234 또는 "ABCD"

