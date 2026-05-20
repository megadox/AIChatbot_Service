# Activity: GetWorksheetAsDataFrame

## Summary
특정 워크시트 전체의 값을 데이터프레임 형태로 Name필드에 지정된 변수에 저장하는 액티비티

## Metadata
- group: `EXCEL`
- script: `GetWorksheetAsDataFrame()`
- pattern: `excel\.GetWorksheetAsDataFrame\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `dataFrame`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `close` | `string` | `False` | `True`, `False` | 작업 완료 후 엑셀 문서를 닫을지 여부를 지정합니다.<br/>(path를 사용한 경우에만 동작한다.)<br/>True: 작업 후 열린 엑셀 문서를 닫습니다.<br/>False: 작업 후 엑셀 문서를 열린 상태로 유지합니다. |
| `header` | `string` | `None` | `None`, `0`, `1`, `2`, `3`, `4` | 데이터프레임으로 변환할 엑셀 파일의 헤더를 지정합니다.<br/>None: 헤더가 없음<br/>0: 첫 번째 행을 열 이름으로 사용 |
| `indexCol` | `string` | `None` | `None`, `0`, `1`, `2`, `3`, `4` | 데이터프레임으로 변환할 엑셀 파일의 인덱스 열을 지정합니다.<br/>None: 행 이름으로 사용할 열이 없음<br/>0: 첫 번째 열을 행 이름으로 사용 |
| `mergeCells` | `string` | `False` | `True`, `False` | 병합된 모든 셀에 값을 채울지 여부를 지정합니다.<br/>True: 병합된 모든 셀에 값을 채움.<br/>False: 병합 기준 셀에만 값을 채움(다른 병합 셀에는 값 없음). |
| `names` | `string` | `None` | - | 데이터프레임으로 변환할 엑셀 파일의 열 이름을 지정합니다.<br/>None: 열 이름이 없음<br/>['이름', '나이', '주소']: 열 이름 리스트 |
| `noneHandling` | `string` | `"keep"` | `"keep"`, `"drop"`, `"empty"`, `"fillzero"` | 데이터프레임의 None Value 처리방법을 지정합니다..<br/>"keep": None value 그대로 유지한다.<br/>"drop": None value를 제거한다.<br/>"empty": None value를 빈값으로 대체한다.<br/>"fillzero": None value를 0 값으로 대체한다 |
| `password` | `string` | `-` | - | 데이터프레임 값으로 가져올 엑셀 파일에 설정된 비밀번호를 지정합니다.<br/>ex) "batem12345" |
| `path` | `string` | `-` | - | 데이터프레임 값으로 가져올 엑셀 파일의 경로를 지정합니다.<br/>ex) "C:\excel\sample.xlsx"<br/>(빈 값이면 Target필드의 엑셀의 데이터를 가져온다.) |
| `returnAsText` | `string` | `False` | `True`, `False` | 데이터프레임 내부 값을 텍스트의 형태로 가져올지 지정합니다.<br/>ex) False |
| `sheet` | `string` | `0` | `0`, `1`, `2`, `3`, `4` | 데이터프레임으로 변환할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) 0 (0부터 시작)<br/>ex) "Sheet1" |
| `useCols` | `string` | `None` | - | 데이터프레임으로 변환할 엑셀 파일의 특정 열을 지정합니다.<br/>None: 모든 열 사용<br/>['이름', '나이']: 사용할 열 이름 리스트<br/>[0 ,2, 3]: 사용할 열 인덱스 리스트 |
| `writeResPassword` | `string` | `-` | - | 데이터프레임 값으로 가져올 엑셀 파일에 설정된 쓰기 권한 비밀번호를 지정합니다.<br/>ex) "ba123" |

## Property Notes
### `close`
작업 완료 후 엑셀 문서를 닫을지 여부를 지정합니다.
(path를 사용한 경우에만 동작한다.)
True: 작업 후 열린 엑셀 문서를 닫습니다.
False: 작업 후 엑셀 문서를 열린 상태로 유지합니다.

### `header`
데이터프레임으로 변환할 엑셀 파일의 헤더를 지정합니다.
None: 헤더가 없음
0: 첫 번째 행을 열 이름으로 사용

### `indexCol`
데이터프레임으로 변환할 엑셀 파일의 인덱스 열을 지정합니다.
None: 행 이름으로 사용할 열이 없음
0: 첫 번째 열을 행 이름으로 사용

### `mergeCells`
병합된 모든 셀에 값을 채울지 여부를 지정합니다.
True: 병합된 모든 셀에 값을 채움.
False: 병합 기준 셀에만 값을 채움(다른 병합 셀에는 값 없음).

### `names`
데이터프레임으로 변환할 엑셀 파일의 열 이름을 지정합니다.
None: 열 이름이 없음
['이름', '나이', '주소']: 열 이름 리스트

### `noneHandling`
데이터프레임의 None Value 처리방법을 지정합니다..
"keep": None value 그대로 유지한다.
"drop": None value를 제거한다.
"empty": None value를 빈값으로 대체한다.
"fillzero": None value를 0 값으로 대체한다

### `password`
데이터프레임 값으로 가져올 엑셀 파일에 설정된 비밀번호를 지정합니다.
ex) "batem12345"

### `path`
데이터프레임 값으로 가져올 엑셀 파일의 경로를 지정합니다.
ex) "C:\excel\sample.xlsx"
(빈 값이면 Target필드의 엑셀의 데이터를 가져온다.)

### `returnAsText`
데이터프레임 내부 값을 텍스트의 형태로 가져올지 지정합니다.
ex) False

### `sheet`
데이터프레임으로 변환할 시트의 인덱스 또는 이름을 지정합니다.
ex) 0 (0부터 시작)
ex) "Sheet1"

### `useCols`
데이터프레임으로 변환할 엑셀 파일의 특정 열을 지정합니다.
None: 모든 열 사용
['이름', '나이']: 사용할 열 이름 리스트
[0 ,2, 3]: 사용할 열 인덱스 리스트

### `writeResPassword`
데이터프레임 값으로 가져올 엑셀 파일에 설정된 쓰기 권한 비밀번호를 지정합니다.
ex) "ba123"

