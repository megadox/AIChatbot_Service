# Activity: SetCheckboxValue

## Summary
웹 페이지에서 체크박스 엘리먼트의 체크 여부 값을 설정하는 액티비티

## Metadata
- group: `WEB`
- script: `set_checkbox_value()`
- pattern: `browser\.set_checkbox_value\(`
- dependencies: `WEB`
- theme: `Accent1`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `retry` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 재시도 횟수를 지정합니다.<br/>ex) 3<br/>(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.) |
| `selector` | `string` | `-` | - | 대상 웹 객체를 선택합니다.<br/>ex) selector_1 |
| `value` | `string` | `True` | `True`, `False` | 체크 여부 값을 설정할 값(True/False)을 지정합니다.<br/>ex) True |
| `waitParams` | `string` | `True` | `True`, `False` | params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.<br/>True: 웹 엘리먼트의 로딩을 대기한다.<br/>False: 웹 엘리먼트의 로딩을 대기하지 않는다. |

## Property Notes
### `retry`
재시도 횟수를 지정합니다.
ex) 3
(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.)

### `selector`
대상 웹 객체를 선택합니다.
ex) selector_1

### `value`
체크 여부 값을 설정할 값(True/False)을 지정합니다.
ex) True

### `waitParams`
params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.
True: 웹 엘리먼트의 로딩을 대기한다.
False: 웹 엘리먼트의 로딩을 대기하지 않는다.

