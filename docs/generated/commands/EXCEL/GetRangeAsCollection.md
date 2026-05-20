# Activity: GetRangeAsCollection

## Summary
특정 범위의 값을 리스트 형태로 Name필드에 지정된 변수에 저장하는 액티비티

## Metadata
- group: `EXCEL`
- script: `GetRangeAsCollection()`
- pattern: `excel\.GetRangeAsCollection\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `collection`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `close` | `string` | `True` | `True`, `False` | 작업 완료 후 엑셀 문서를 닫을지 여부를 지정합니다.<br/>(path를 사용한 경우에만 동작한다.)<br/>True: 작업 후 열린 엑셀 문서를 닫습니다.<br/>False: 작업 후 엑셀 문서를 열린 상태로 유지합니다. |
| `password` | `string` | `-` | - | 컬렉션 값으로 가져올 엑셀 파일에 설정된 비밀번호를 지정합니다.<br/>ex) "batem12345" |
| `path` | `string` | `-` | - | 컬렉션 값으로 가져올 엑셀 파일의 경로를 지정합니다.<br/>ex) "C:\excel\sample.xlsx"<br/>(빈 값이면 Target필드의 엑셀의 데이터를 가져온다.) |
| `range` | `string` | `"A1:B2"` | - | 컬렉션 값을 가져올 영역을 지정합니다.<br/>ex) "E2:E3"<br/>(컬렉션은 2D리스트로 반환한다.) |
| `returnAsText` | `string` | `False` | `True`, `False` | 값을 텍스트의 형태로 가져올지 지정합니다.<br/>ex) False |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |
| `writeResPassword` | `string` | `-` | - | 컬렉션 값으로 가져올 엑셀 파일에 설정된 쓰기 권한 비밀번호를 지정합니다.<br/>ex) "ba123" |

## Property Notes
### `close`
작업 완료 후 엑셀 문서를 닫을지 여부를 지정합니다.
(path를 사용한 경우에만 동작한다.)
True: 작업 후 열린 엑셀 문서를 닫습니다.
False: 작업 후 엑셀 문서를 열린 상태로 유지합니다.

### `password`
컬렉션 값으로 가져올 엑셀 파일에 설정된 비밀번호를 지정합니다.
ex) "batem12345"

### `path`
컬렉션 값으로 가져올 엑셀 파일의 경로를 지정합니다.
ex) "C:\excel\sample.xlsx"
(빈 값이면 Target필드의 엑셀의 데이터를 가져온다.)

### `range`
컬렉션 값을 가져올 영역을 지정합니다.
ex) "E2:E3"
(컬렉션은 2D리스트로 반환한다.)

### `returnAsText`
값을 텍스트의 형태로 가져올지 지정합니다.
ex) False

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

### `writeResPassword`
컬렉션 값으로 가져올 엑셀 파일에 설정된 쓰기 권한 비밀번호를 지정합니다.
ex) "ba123"

