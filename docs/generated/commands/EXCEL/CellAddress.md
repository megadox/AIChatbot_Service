# Activity: CellAddress

## Summary
특정 셀의 인덱스 방식의 주소를 엑셀 표시 형식의 주소를 변환할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `CellAddress()`
- pattern: `excel\.CellAddress\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `cellName`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `active` | `string` | `False` | `True`, `False` | 현재 활성화된 셀의 주소의 변환 여부를 지정합니다.<br/>True: 현재 활성화된 셀의 주소를 변환한다.<br/>False: row와 column 필드의 값에 해당하는 셀의 주소를 변환한다. |
| `column` | `string` | `0` | - | 변환할 열의 값을 인덱스 방식으로 지정합니다.<br/>ex) 3 |
| `row` | `string` | `0` | - | 변환할 행의 값을 인덱스 방식으로 지정합니다.<br/>ex) 5 |

## Property Notes
### `active`
현재 활성화된 셀의 주소의 변환 여부를 지정합니다.
True: 현재 활성화된 셀의 주소를 변환한다.
False: row와 column 필드의 값에 해당하는 셀의 주소를 변환한다.

### `column`
변환할 열의 값을 인덱스 방식으로 지정합니다.
ex) 3

### `row`
변환할 행의 값을 인덱스 방식으로 지정합니다.
ex) 5

