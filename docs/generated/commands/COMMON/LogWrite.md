# Activity: LogWrite

## Summary
로그 메시지를 특정 파일에 저장하는 액티비티

## Metadata
- group: `COMMON`
- script: `LogWrite()`
- pattern: `LogWrite\(`
- dependencies: `FILE, datetime, os`
- theme: `Dark_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `ext` | `string` | `"log"` | - | 로그 파일의 확장자를 지정합니다.<br/>ex) "log" |
| `filePath` | `string` | `` | - | 로그 파일의 저장 폴더 경로를 지정합니다.<br/>ex) "C:\log" |
| `format` | `string` | `"%Y-%m-%d %H:%M:%S"` | - | 로그에 포함될 시간의 표시 형식을 지정합니다.<br/>ex) "%Y-%m-%d %H:%M"<br/>(파이썬 datetime 포맷을 지원한다.) |
| `level` | `string` | `"[Info]"` | - | 표시할 로그의 레벨을 지정합니다.<br/>ex) "[Info]" |
| `message` | `string` | `` | - | 로그의 내용에 해당하는 텍스트를 지정합니다.<br/>ex) "로그 기록" |

## Property Notes
### `ext`
로그 파일의 확장자를 지정합니다.
ex) "log"

### `filePath`
로그 파일의 저장 폴더 경로를 지정합니다.
ex) "C:\log"

### `format`
로그에 포함될 시간의 표시 형식을 지정합니다.
ex) "%Y-%m-%d %H:%M"
(파이썬 datetime 포맷을 지원한다.)

### `level`
표시할 로그의 레벨을 지정합니다.
ex) "[Info]"

### `message`
로그의 내용에 해당하는 텍스트를 지정합니다.
ex) "로그 기록"

