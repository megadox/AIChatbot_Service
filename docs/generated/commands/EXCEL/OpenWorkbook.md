# Activity: OpenWorkbook

## Summary
특정 폴더에 있는 엑셀파일을 여는 액티비티

## Metadata
- group: `EXCEL`
- script: `OpenWorkbook()`
- pattern: `excel\.OpenWorkbook\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `workbook`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `password` | `string` | `-` | - | 오픈할 엑셀 파일에 설정된 비밀번호를 지정합니다.<br/>ex) "batem12345" |
| `path` | `string` | `-` | - | 오픈할 엑셀 파일의 경로를 지정합니다.<br/>ex) "C:\excel\sample.xlsx" |
| `writeResPassword` | `string` | `-` | - | 오픈할 엑셀 파일에 설정된 쓰기 권한 비밀번호를 지정합니다.<br/>ex) "ba123" |

## Property Notes
### `password`
오픈할 엑셀 파일에 설정된 비밀번호를 지정합니다.
ex) "batem12345"

### `path`
오픈할 엑셀 파일의 경로를 지정합니다.
ex) "C:\excel\sample.xlsx"

### `writeResPassword`
오픈할 엑셀 파일에 설정된 쓰기 권한 비밀번호를 지정합니다.
ex) "ba123"

