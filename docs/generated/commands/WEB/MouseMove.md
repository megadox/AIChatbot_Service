# Activity: MouseMove

## Summary
마우스를 브라우저의 특정 좌표로 이동시키는 액티비티

## Metadata
- group: `WEB`
- script: `move_mouse()`
- pattern: `.*\.move_mouse\(`
- dependencies: `WEB`
- theme: `Accent1`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `retry` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 재시도 횟수를 지정합니다.<br/>ex) 3<br/>(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.) |
| `XPos` | `string` | `0` | - | 마우스가 이동할 x 좌표값을 지정합니다.<br/>ex) 500 |
| `YPos` | `string` | `0` | - | 마우스가 이동할 y 좌표값을 지정합니다.<br/>ex) 100 |

## Property Notes
### `retry`
재시도 횟수를 지정합니다.
ex) 3
(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.)

### `XPos`
마우스가 이동할 x 좌표값을 지정합니다.
ex) 500

### `YPos`
마우스가 이동할 y 좌표값을 지정합니다.
ex) 100

