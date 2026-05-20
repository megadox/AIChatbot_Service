# Activity: CopyAndAppendRangeIf

## Summary
지정한 조건에 맞는 셀의 값을 복사하여 셀과 같은 열 아래로 keyColumn의 마지막 행까지 적용할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `CopyAndAppendRangeIf()`
- pattern: `excel\.CopyAndAppendRangeIf\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `autoClose` | `string` | `False` | `True`, `False` | 복사완료 후 복사했던 워크북을 자동으로 닫을지 여부를 지정합니다.<br/>True: 자동으로 복사했던 워크북을 종료한다.<br/>False: 복사했던 워크북을 종료하지 않는다. |
| `autoSave` | `string` | `False` | `True`, `False` | 복사완료 후 복사했던 워크북을 자동으로 저장할 지 여부를 지정합니다.<br/>True: 자동으로 복사했던 워크북을 저장한다.<br/>False: 복사했던 워크북을 저장하지 않는다. |
| `condition` | `string` | `-` | - | 복사할 데이터의 조건을 지정합니다.<br/>ex) "1\|사고건수"<br/>(『1\|사고건수』는 인덱스 1인 열(=B열)에서 셀값이 사고건수인 경우만 복사하는 동작을 수행한다.) |
| `cut` | `string` | `False` | `True`, `False` | 붙여넣기 할 시트에서 조건에 맞는 행의 내용을 삭제할 지 여부를 지정합니다.<br/>True: 복사만 진행한다.<br/>False: 잘라내기한다. |
| `fromBeginRow` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 복사를 시작할 행을 인덱스 방식으로 지정합니다.<br/>ex) 3 |
| `fromKeyColumn` | `string` | `-` | - | 복사할 시트에서 유효한 데이터 영역의 끝을 알 수 있게 해주는 기준열을 지정합니다.<br/>ex) 1 |
| `fromRange` | `string` | `-` | - | 복사할 영역을 지정합니다.<br/>ex) "E2:E3" |
| `fromSheetObject` | `string` | `-` | - | 복사할 시트의 객체를 지정합니다.<br/>ex) sheet_0<br/>(빈 값이면 활성화된 시트에서 진행한다.) |
| `removeBlank` | `string` | `False` | `True`, `False` | 붙여넣기 할 시트에서 조건에 맞는 행의 내용을 삭제 후, 빈 행일때 행 삭제를 수행할 지 여부를 지정합니다.<br/>True: 빈 행을 삭제한다.<br/>False: 빈 행을 삭제하지 않는다. |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |
| `toKeyColumn` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 붙여넣기할 시트에서 데이터 영역의 끝을 알 수 있게 해주는 기준열을 지정합니다.<br/>ex) 2 |
| `toWorkbook` | `string` | `-` | - | 붙여넣기할 엑셀 문서의 경로와 파일이름을 지정합니다.<br/>ex) path_0\sample1.xlsx |
| `toWorksheet` | `string` | `"Sheet1"` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 붙여넣기할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) 0<br/>ex) "Sheet 1" |

## Property Notes
### `autoClose`
복사완료 후 복사했던 워크북을 자동으로 닫을지 여부를 지정합니다.
True: 자동으로 복사했던 워크북을 종료한다.
False: 복사했던 워크북을 종료하지 않는다.

### `autoSave`
복사완료 후 복사했던 워크북을 자동으로 저장할 지 여부를 지정합니다.
True: 자동으로 복사했던 워크북을 저장한다.
False: 복사했던 워크북을 저장하지 않는다.

### `condition`
복사할 데이터의 조건을 지정합니다.
ex) "1|사고건수"
(『1|사고건수』는 인덱스 1인 열(=B열)에서 셀값이 사고건수인 경우만 복사하는 동작을 수행한다.)

### `cut`
붙여넣기 할 시트에서 조건에 맞는 행의 내용을 삭제할 지 여부를 지정합니다.
True: 복사만 진행한다.
False: 잘라내기한다.

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

### `removeBlank`
붙여넣기 할 시트에서 조건에 맞는 행의 내용을 삭제 후, 빈 행일때 행 삭제를 수행할 지 여부를 지정합니다.
True: 빈 행을 삭제한다.
False: 빈 행을 삭제하지 않는다.

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

