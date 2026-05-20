# Activity: GetCellValue

## Summary
특정 셀의 값을 Name필드에 지정된 변수에 저장하는 액티비티

## Metadata
- group: `EXCEL`
- script: `GetCellValue()`
- pattern: `= excel\.GetCellValue\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `value`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `cell` | `string` | `-` | - | 값을 가져올 셀을 엑셀 표시 방식으로 지정합니다.<br/>ex) "B2"<br/>(빈 값이면 row와 column 필드의 값으로 셀을 지정한다.) |
| `column` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 값을 가져올 열의 값을 인덱스 방식으로 지정합니다.<br/>ex) 3 |
| `returnAsText` | `string` | `False` | `True`, `False` | 값을 텍스트의 형태로 가져올지 지정합니다.<br/>ex) False |
| `row` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 값을 가져올 행의 값을 인덱스 방식으로 지정합니다.<br/>ex) 2 |

## Property Notes
### `cell`
값을 가져올 셀을 엑셀 표시 방식으로 지정합니다.
ex) "B2"
(빈 값이면 row와 column 필드의 값으로 셀을 지정한다.)

### `column`
값을 가져올 열의 값을 인덱스 방식으로 지정합니다.
ex) 3

### `returnAsText`
값을 텍스트의 형태로 가져올지 지정합니다.
ex) False

### `row`
값을 가져올 행의 값을 인덱스 방식으로 지정합니다.
ex) 2

