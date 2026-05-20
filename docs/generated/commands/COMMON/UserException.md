# Activity: UserException

## Summary
사용자가 임의로 예외를 발생시키는 액티비티

## Metadata
- group: `COMMON`
- script: `COMMON.create_error()`
- pattern: `COMMON\.create_error\(`
- dependencies: `COMMON`
- theme: `Dark_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `comment` | `string` | `"error"` | - | 사용자 오류 내용을 지정합니다.<br/>ex) "태스크1번 오류 발생" |
| `enterOnError` | `string` | `False` | `True`, `False` | On Error 액티비티에 들어갈지 여부를 지정합니다.<br/>True: On Error액티비티를 수행한다.<br/>False: 프로세스를 바로 종료한다. |

## Property Notes
### `comment`
사용자 오류 내용을 지정합니다.
ex) "태스크1번 오류 발생"

### `enterOnError`
On Error 액티비티에 들어갈지 여부를 지정합니다.
True: On Error액티비티를 수행한다.
False: 프로세스를 바로 종료한다.

