# Activity: WaitDisappear

## Summary
특정 웹 엘리먼트가 사라지기를 기다리는 액티비티

## Metadata
- group: `WEB`
- script: `wait_disappear()`
- pattern: `browser\.wait_disappear\(`
- dependencies: `WEB`
- theme: `Accent1`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `encoding` | `string` | `"utf-8"` | `"utf-8"`, `"cp949"`, `"utf-16"` | 웹 브라우저 기본 인코딩 값 지정합니다.<br/>ex)"utf-8" |
| `retry` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 재시도 횟수를 지정합니다.<br/>ex) 3<br/>(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.) |
| `selector` | `string` | `-` | - | 대상 웹 객체를 선택합니다.<br/>ex) selector_1 |
| `timeout` | `string` | `30` | `30`, `40`, `50`, `60`, `100` | 대기할 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |

## Property Notes
### `encoding`
웹 브라우저 기본 인코딩 값 지정합니다.
ex)"utf-8"

### `retry`
재시도 횟수를 지정합니다.
ex) 3
(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.)

### `selector`
대상 웹 객체를 선택합니다.
ex) selector_1

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30 (단위: 초)

