# Activity: CopySheetToWorkBook

## Summary
같은 인스턴스 내에서 특정 워크북의 시트를 복사하여 다른 워크북에 붙여넣을 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `CopySheetToWorkbook()`
- pattern: `excel\.CopySheetToWorkbook\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `sheet`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `dstSheet` | `string` | `-` | - | 붙여넣기할 시트의 이름, 인덱스 또는 객체 변수를 지정합니다.<br/>(값이 비어 있거나 대상 시트가 존재하지 않으면 새 시트를 생성한다.)<br/>ex) "Sheet5", 1 또는 sheet_2 |
| `dstWorkbook` | `string` | `-` | - | 붙여넣기할 대상 워크북(파일)의 이름 또는 객체 변수를 지정합니다.<br/>ex) "Destination.xlsx" 또는 workbook_2 |
| `fromRangeBegin` | `string` | `-` | - | 복사를 시작할 셀 주소를 지정합니다.<br/>(값이 비어 있으면 시트 전체를 복사한다.)<br/>* 특정 영역만 복사할 경우 열 너비는 복사되지 않습니다.<br/>ex) "B3" |
| `insertIndex` | `string` | `-1` | `-1`, `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 새로 생성되는 시트의 삽입 위치를 인덱스로 지정합니다.<br/>(-1인 경우 마지막 시트 뒤에 추가한다.)<br/>ex) 1 |
| `newSheetName` | `string` | `-` | - | 새로 생성되거나 붙여넣을 시트의 이름을 지정합니다.<br/>(값이 비어 있을 경우, dstSheet가 존재하면 해당 시트 이름을 유지하며, 새 시트 생성 시에는 srcSheet 이름을 사용한다.)<br/>ex) "CopiedSheet" |
| `srcSheet` | `string` | `-` | - | 복사할 시트의 이름, 인덱스 또는 객체 변수를 지정합니다.<br/>(값이 비어 있으면 활성화된 시트를 복사한다.)<br/>ex) "Sheet3", 0 또는 sheet_1 |
| `srcWorkbook` | `string` | `-` | - | 복사할 시트가 포함된 원본 워크북(파일)의 이름 또는 객체 변수를 지정합니다.<br/>ex) "Source.xlsx" 또는 workbook_1 |
| `toRangeBegin` | `string` | `-` | - | 붙여넣기를 시작할 셀 주소를 지정합니다.<br/>(값이 비어 있으면 A1 셀부터 붙여 넣는다.)<br/>* 본 옵션은 fromRangeBegin 프로퍼티를 지정한 경우에만 사용할 수 있습니다.<br/>* 특정 영역만 복사할 경우 열 너비는 복사되지 않습니다.<br/>ex) "C4" |

## Property Notes
### `dstSheet`
붙여넣기할 시트의 이름, 인덱스 또는 객체 변수를 지정합니다.
(값이 비어 있거나 대상 시트가 존재하지 않으면 새 시트를 생성한다.)
ex) "Sheet5", 1 또는 sheet_2

### `dstWorkbook`
붙여넣기할 대상 워크북(파일)의 이름 또는 객체 변수를 지정합니다.
ex) "Destination.xlsx" 또는 workbook_2

### `fromRangeBegin`
복사를 시작할 셀 주소를 지정합니다.
(값이 비어 있으면 시트 전체를 복사한다.)
* 특정 영역만 복사할 경우 열 너비는 복사되지 않습니다.
ex) "B3"

### `insertIndex`
새로 생성되는 시트의 삽입 위치를 인덱스로 지정합니다.
(-1인 경우 마지막 시트 뒤에 추가한다.)
ex) 1

### `newSheetName`
새로 생성되거나 붙여넣을 시트의 이름을 지정합니다.
(값이 비어 있을 경우, dstSheet가 존재하면 해당 시트 이름을 유지하며, 새 시트 생성 시에는 srcSheet 이름을 사용한다.)
ex) "CopiedSheet"

### `srcSheet`
복사할 시트의 이름, 인덱스 또는 객체 변수를 지정합니다.
(값이 비어 있으면 활성화된 시트를 복사한다.)
ex) "Sheet3", 0 또는 sheet_1

### `srcWorkbook`
복사할 시트가 포함된 원본 워크북(파일)의 이름 또는 객체 변수를 지정합니다.
ex) "Source.xlsx" 또는 workbook_1

### `toRangeBegin`
붙여넣기를 시작할 셀 주소를 지정합니다.
(값이 비어 있으면 A1 셀부터 붙여 넣는다.)
* 본 옵션은 fromRangeBegin 프로퍼티를 지정한 경우에만 사용할 수 있습니다.
* 특정 영역만 복사할 경우 열 너비는 복사되지 않습니다.
ex) "C4"

