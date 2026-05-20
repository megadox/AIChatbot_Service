# Activity: CopyAndAppendRange

## Summary
지정한 셀의 값을 복사하여 셀과 같은 열 아래로 keyColumn의 마지막 행까지 적용할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `CopyAndAppendRange()`
- pattern: `excel\.CopyAndAppendRange\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `autoClose` | `string` | `False` | `True`, `False` | 복사완료 후 복사했던 워크북을 자동으로 닫을지 여부를 지정합니다.<br/>True: 자동으로 복사했던 워크북을 종료한다.<br/>False: 복사했던 워크북을 종료하지 않는다. |
| `fromBeginRow` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 복사를 시작할 행을 인덱스 방식으로 지정합니다.<br/>ex) 3 |
| `fromKeyColumn` | `string` | `-` | - | 복사할 시트에서 유효한 데이터 영역의 끝을 알 수 있게 해주는 기준열을 지정합니다.<br/>ex) 1 |
| `fromRange` | `string` | `-` | - | 복사할 영역을 지정합니다.<br/>ex) "E2:E3" |
| `fromSheetObject` | `string` | `-` | - | 복사할 시트의 객체를 지정합니다.<br/>ex) sheet_0<br/>(빈 값이면 활성화된 시트에서 진행한다.) |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |
| `toKeyColumn` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 붙여넣기할 시트에서 데이터 영역의 끝을 알 수 있게 해주는 기준열을 지정합니다.<br/>ex) 2 |
| `toWorkbook` | `string` | `-` | - | 붙여넣기할 엑셀 문서의 경로와 파일이름을 지정합니다.<br/>ex) path_0\sample1.xlsx |
| `toWorksheet` | `string` | `"Sheet1"` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 붙여넣기할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) 0<br/>ex) "Sheet 1" |

## Property Notes
### `autoClose`
복사완료 후 복사했던 워크북을 자동으로 닫을지 여부를 지정합니다.
True: 자동으로 복사했던 워크북을 종료한다.
False: 복사했던 워크북을 종료하지 않는다.

### `fromBeginRow`
복사를 시작할 행을 인덱스 방식으로 지정합니다.
ex) 3

### `fromKeyColumn`
복사할 시트에서 유효한 데이터 영역의 끝을 알 수 있게 해주는 기준열을 지정합니다.
ex) 1

### `fromRange`
복사할 영역을 지정합니다.
ex) "E2:E3"

### `fromSheetObject`
복사할 시트의 객체를 지정합니다.
ex) sheet_0
(빈 값이면 활성화된 시트에서 진행한다.)

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

### `toKeyColumn`
붙여넣기할 시트에서 데이터 영역의 끝을 알 수 있게 해주는 기준열을 지정합니다.
ex) 2

### `toWorkbook`
붙여넣기할 엑셀 문서의 경로와 파일이름을 지정합니다.
ex) path_0\sample1.xlsx

### `toWorksheet`
붙여넣기할 시트의 인덱스 또는 이름을 지정합니다.
ex) 0
ex) "Sheet 1"

