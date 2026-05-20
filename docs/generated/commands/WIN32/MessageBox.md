# Activity: MessageBox

## Summary
윈도우 화면에 알람창을 통해 메시지를 표시해주는 액티비티

## Metadata
- group: `WIN32`
- script: `WIN32.MessageBox()`
- pattern: `win32\.MessageBox`
- dependencies: `WIN32`
- theme: `Accent3_5`
- prefix: `msgbox`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `align` | `string` | `"CENTER"` | `"LEFT"`, `"RIGHT"`, `"CENTER"` | 메시지 박스에 나타낼 컨텐츠 텍스트의 정렬을 지정합니다<br/>ex) "CENTER" |
| `text` | `string` | `-` | - | 메시지 박스에 나타낼 컨텐츠 텍스트를 지정합니다.<br/>ex) "작업 종료" |
| `timeout` | `string` | `"None"` | `"None"`, `3`, `4`, `5`, `6`, `7`, `8`, `9`, `10` | 메시지 박스를 자동 종료할 시간을 지정합니다.<br/>ex) 5 (단위: 초) |
| `title` | `string` | `-` | - | 메시지 박스에 나타낼 타이틀 텍스트를 지정합니다.<br/>ex) "info" |

## Property Notes
### `align`
메시지 박스에 나타낼 컨텐츠 텍스트의 정렬을 지정합니다
ex) "CENTER"

### `text`
메시지 박스에 나타낼 컨텐츠 텍스트를 지정합니다.
ex) "작업 종료"

### `timeout`
메시지 박스를 자동 종료할 시간을 지정합니다.
ex) 5 (단위: 초)

### `title`
메시지 박스에 나타낼 타이틀 텍스트를 지정합니다.
ex) "info"

