# Activity: WaitAlert

## Summary
웹 브라우저에 알림창이 뜰 때까지 대기하는 액티비티

## Metadata
- group: `WEB`
- script: `wait_alert()`
- pattern: `browser\.wait_alert\(`
- dependencies: `WEB`
- theme: `Accent1`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `retry` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 재시도 횟수를 지정합니다.<br/>ex) 3<br/>(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.) |
| `timeout` | `string` | `30` | `10`, `20`, `30`, `40`, `50`, `60` | 대기할 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |

## Property Notes
### `retry`
재시도 횟수를 지정합니다.
ex) 3
(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.)

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30 (단위: 초)

