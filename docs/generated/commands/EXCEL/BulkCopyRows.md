# Activity: BulkCopyRows

## Summary
특정 파일의 시트와 영역시점을 지정하여 다른 파일의 시트와 영역시점으로 복사할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `EXCEL.BulkCopyRows()`
- pattern: `EXCEL\.BulkCopyRows\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `rowcount`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `fromFileName` | `string` | `-` | - | 복사할 소스 파일의 이름을 지정합니다.<br/>ex) "src.xlsx" |
| `fromRangeBegin` | `string` | `"A1"` | - | 복사할 영역의 시작부분 셀주소를 지정합니다.<br/>ex) A1 |
| `fromSheetIndex` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 복사할 소스 시트의 인덱스를 지정합니다.<br/>ex) 0 |
| `toFileName` | `string` | `-` | - | 붙여넣기할 목적 파일의 이름을 지정합니다.<br/>ex) "dest.xlsx" |
| `toRangeBegin` | `string` | `"A1"` | - | 붙여넣기할 역역의 시작부분 셀주소를 지정합니다.<br/>ex) A2 |
| `toSheetIndex` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 붙여넣기할 목적 시트의 인덱스를 지정합니다.<br/>ex) 2 |

## Property Notes
### `fromFileName`
복사할 소스 파일의 이름을 지정합니다.
ex) "src.xlsx"

### `fromRangeBegin`
복사할 영역의 시작부분 셀주소를 지정합니다.
ex) A1

### `fromSheetIndex`
복사할 소스 시트의 인덱스를 지정합니다.
ex) 0

### `toFileName`
붙여넣기할 목적 파일의 이름을 지정합니다.
ex) "dest.xlsx"

### `toRangeBegin`
붙여넣기할 역역의 시작부분 셀주소를 지정합니다.
ex) A2

### `toSheetIndex`
붙여넣기할 목적 시트의 인덱스를 지정합니다.
ex) 2

