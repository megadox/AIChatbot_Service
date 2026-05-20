# Activity: ClickAlert

## Summary
웹 브라우저에 알림창의 확인/취소 버튼을 클릭하는 액티비티

## Metadata
- group: `WEB`
- script: `click_alert()`
- pattern: `browser\.click_alert()`
- dependencies: `WEB`
- theme: `Accent1`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `clickOption` | `string` | `"OK"` | `"OK"`, `"CANCEL"` | 클릭할 브라우저 알림창의 버튼을 지정합니다.<br/>OK: 확인 버튼을 클릭한다.<br/>CANCEL: 취소 버튼을 클릭한다. |
| `wait` | `string` | `True` | `True`, `False` | 알림창이 나타날 때까지 대기할지 여부를 지정합니다.<br/>True: 알림창이 나타날 때까지 대기한다.<br/>False: 알림창이 나타날 때까지 대기하지 않는다. |

## Property Notes
### `clickOption`
클릭할 브라우저 알림창의 버튼을 지정합니다.
OK: 확인 버튼을 클릭한다.
CANCEL: 취소 버튼을 클릭한다.

### `wait`
알림창이 나타날 때까지 대기할지 여부를 지정합니다.
True: 알림창이 나타날 때까지 대기한다.
False: 알림창이 나타날 때까지 대기하지 않는다.

