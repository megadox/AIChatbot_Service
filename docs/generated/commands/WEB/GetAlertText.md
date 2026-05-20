# Activity: GetAlertText

## Summary
웹 브라우저에 알림창의 텍스트를 가져오는 액티비티

## Metadata
- group: `WEB`
- script: `get_alert_text()`
- pattern: `browser\.get_alert_text\(`
- dependencies: `WEB`
- theme: `Accent1`
- prefix: `text`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `retry` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 재시도 횟수를 지정합니다.<br/>ex) 3<br/>(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.) |
| `wait` | `string` | `True` | `True`, `False` | 알림창이 나타날 때까지 대기 여부를 지정합니다.<br/>True: 알림창이 나타날 때까지 대기한다.<br/>False: 알림창이 나타날 때까지 대기하지 않는다. |

## Property Notes
### `retry`
재시도 횟수를 지정합니다.
ex) 3
(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.)

### `wait`
알림창이 나타날 때까지 대기 여부를 지정합니다.
True: 알림창이 나타날 때까지 대기한다.
False: 알림창이 나타날 때까지 대기하지 않는다.

