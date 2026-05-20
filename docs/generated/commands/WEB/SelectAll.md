# Activity: SelectAll

## Summary
특정 웹 엘리먼트들을 가르키는 참조값을 Name필드에 설정된 변수에 컬렉션 형태로 저장하는 액티비티

## Metadata
- group: `WEB`
- script: `FindElements()`
- pattern: `browser\.FindElements\(`
- dependencies: `WEB`
- theme: `Accent1`
- prefix: `collection`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `retry` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 재시도 횟수를 지정합니다.<br/>ex) 3<br/>(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.) |
| `selector` | `string` | `-` | - | 대상 웹 객체를 선택합니다.<br/>ex) selector_1 |
| `waitParams` | `string` | `True` | `True`, `False` | params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.<br/>True: 웹 엘리먼트의 로딩을 대기한다.<br/>False: 웹 엘리먼트의 로딩을 대기하지 않는다. |

## Property Notes
### `retry`
재시도 횟수를 지정합니다.
ex) 3
(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.)

### `selector`
대상 웹 객체를 선택합니다.
ex) selector_1

### `waitParams`
params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.
True: 웹 엘리먼트의 로딩을 대기한다.
False: 웹 엘리먼트의 로딩을 대기하지 않는다.

