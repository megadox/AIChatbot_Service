# Activity: DataFrameToExcel

## Summary
데이터프레임의 데이터를 엑셀 파일로 변환하는 액티비티

## Metadata
- group: `DATA`
- script: `DATA.dataframe_to_excel()`
- pattern: `DATA\.dataframe_to_excel\(`
- dependencies: `DATA`
- theme: `Accent2`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `columns` | `string` | `None` | - | 데이터프레임을 엑셀 파일로 변환할 열 이름의 리스트를 지정합니다.<br/>None: 모든 열 사용<br/>['이름', '나이', '주소']: 열 이름 리스트 |
| `data` | `string` | `-` | - | 엑셀 파일로 변환할 데이터프레임을 지정합니다.<br/>ex) DataFrame_1 |
| `header` | `string` | `True` | `True`, `False` | 데이터프레임의 열 이름을 포함하여 엑셀 파일로 변환할지 여부를 지정합니다.<br/>True: 행 이름 포함<br/>False: 열 이름 포함하지 않음 |
| `index` | `string` | `True` | `True`, `False` | 데이터프레임의 행 이름을 포함하여 엑셀 파일로 변환할지 여부를 지정합니다.<br/>True: 열 이름 포함<br/>False: 행 이름 포함하지 않음 |
| `mergeCells` | `string` | `True` | `True`, `False` | 데이터프레임의 멀티인덱스를 병합된 셀로 표시할지 여부를 지정합니다.<br/>True: 셀 병합 표시<br/>False: 셀 병합 표시하지 않음 |
| `nanRep` | `string` | `-` | - | 데이터프레임의 결측치(NaN)를 Excel에서 어떤 값으로 표현할지 지정합니다.<br/>ex) 0 |
| `path` | `string` | `-` | - | 데이터프레임을 엑셀 파일로 변환할 경로를 지정합니다.<br/>ex) "C:\\Users\\user\\Desktop\\data.xlsx" |
| `sheet` | `string` | `"Sheet1"` | - | 데이터프레임을 엑셀 파일로 변환할 시트 이름을 지정합니다.<br/>ex) "Sheet1" |
| `startCol` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 데이터프레임을 엑셀 파일로 변환할 시작 열을 지정합니다.<br/>ex) 1 |
| `startRow` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 데이터프레임을 엑셀 파일로 변환할 시작 행을 지정합니다.<br/>ex) 1 |

## Property Notes
### `columns`
데이터프레임을 엑셀 파일로 변환할 열 이름의 리스트를 지정합니다.
None: 모든 열 사용
['이름', '나이', '주소']: 열 이름 리스트

### `data`
엑셀 파일로 변환할 데이터프레임을 지정합니다.
ex) DataFrame_1

### `header`
데이터프레임의 열 이름을 포함하여 엑셀 파일로 변환할지 여부를 지정합니다.
True: 행 이름 포함
False: 열 이름 포함하지 않음

### `index`
데이터프레임의 행 이름을 포함하여 엑셀 파일로 변환할지 여부를 지정합니다.
True: 열 이름 포함
False: 행 이름 포함하지 않음

### `mergeCells`
데이터프레임의 멀티인덱스를 병합된 셀로 표시할지 여부를 지정합니다.
True: 셀 병합 표시
False: 셀 병합 표시하지 않음

### `nanRep`
데이터프레임의 결측치(NaN)를 Excel에서 어떤 값으로 표현할지 지정합니다.
ex) 0

### `path`
데이터프레임을 엑셀 파일로 변환할 경로를 지정합니다.
ex) "C:\\Users\\user\\Desktop\\data.xlsx"

### `sheet`
데이터프레임을 엑셀 파일로 변환할 시트 이름을 지정합니다.
ex) "Sheet1"

### `startCol`
데이터프레임을 엑셀 파일로 변환할 시작 열을 지정합니다.
ex) 1

### `startRow`
데이터프레임을 엑셀 파일로 변환할 시작 행을 지정합니다.
ex) 1

