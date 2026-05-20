# Activity: ExcelToDataFrame

## Summary
엑셀 파일의 데이터를 데이터프레임으로 변환하는 액티비티

## Metadata
- group: `DATA`
- script: `DATA.excel_to_dataframe()`
- pattern: `DATA\.excel_to_dataframe\(`
- dependencies: `DATA`
- theme: `Accent2`
- prefix: `dataFrame`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `header` | `string` | `0` | `None`, `0`, `1`, `2`, `3`, `4` | 데이터프레임으로 변환할 엑셀 파일의 헤더를 지정합니다.<br/>None: 헤더가 없음<br/>0: 첫 번째 행을 열 이름으로 사용 |
| `indexCol` | `string` | `None` | `None`, `0`, `1`, `2`, `3`, `4` | 데이터프레임으로 변환할 엑셀 파일의 인덱스 열을 지정합니다.<br/>None: 행 이름으로 사용할 열이 없음<br/>0: 첫 번째 열을 행 이름으로 사용 |
| `names` | `string` | `None` | - | 데이터프레임으로 변환할 엑셀 파일의 열 이름을 지정합니다.<br/>None: 열 이름이 없음<br/>['이름', '나이', '주소']: 열 이름 리스트 |
| `path` | `string` | `-` | - | 데이터프레임으로 변환할 엑셀 파일의 경로를 지정합니다.<br/>ex) "C:\\Users\\user\\Desktop\\data.xlsx" |
| `sheet` | `string` | `0` | `None`, `0`, `1`, `2`, `3`, `4` | 데이터프레임으로 변환할 시트의 인덱스, 이름 또는 시트 목록 리스트를 지정합니다.<br/>ex)None: 모든 시트<br/>0 (0부터 시작)<br/>ex) "Sheet1"<br/>ex) ['0', 'Sheet2'] |
| `useCols` | `string` | `None` | - | 데이터프레임으로 변환할 엑셀 파일의 특정 열을 지정합니다.<br/>None: 모든 열 사용<br/>['A', 'C', 'D']: 사용할 열 리스트<br/>[0 ,2, 3]: 사용할 열 인덱스 리스트<br/>"A:Ev or "A,C,E:F": (:),(,)구분자 사용 가능 |

## Property Notes
### `header`
데이터프레임으로 변환할 엑셀 파일의 헤더를 지정합니다.
None: 헤더가 없음
0: 첫 번째 행을 열 이름으로 사용

### `indexCol`
데이터프레임으로 변환할 엑셀 파일의 인덱스 열을 지정합니다.
None: 행 이름으로 사용할 열이 없음
0: 첫 번째 열을 행 이름으로 사용

### `names`
데이터프레임으로 변환할 엑셀 파일의 열 이름을 지정합니다.
None: 열 이름이 없음
['이름', '나이', '주소']: 열 이름 리스트

### `path`
데이터프레임으로 변환할 엑셀 파일의 경로를 지정합니다.
ex) "C:\\Users\\user\\Desktop\\data.xlsx"

### `sheet`
데이터프레임으로 변환할 시트의 인덱스, 이름 또는 시트 목록 리스트를 지정합니다.
ex)None: 모든 시트
0 (0부터 시작)
ex) "Sheet1"
ex) ['0', 'Sheet2']

### `useCols`
데이터프레임으로 변환할 엑셀 파일의 특정 열을 지정합니다.
None: 모든 열 사용
['A', 'C', 'D']: 사용할 열 리스트
[0 ,2, 3]: 사용할 열 인덱스 리스트
"A:Ev or "A,C,E:F": (:),(,)구분자 사용 가능

