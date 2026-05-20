# Activity: SetValue

## Summary
특정 웹 엘리먼트에 value필드의 값을 입력하는 액티비티

## Metadata
- group: `WEB`
- script: `SetValue()`
- pattern: `\.SetValue\(`
- dependencies: `WEB`
- theme: `Accent1`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `encoding` | `string` | `"utf-8"` | `"utf-8"`, `"cp949"`, `"utf-16"` | 웹 브라우저 기본 인코딩 값 지정합니다.<br/>ex) "utf-8" |
| `selector` | `string` | `-` | - | 대상 웹 객체를 선택합니다.<br/>ex) selector_1 |
| `value` | `string` | `-` | - | 웹 엘리먼트에 입력할 값을 문자열로 지정합니다.<br/>ex) "환율" |
| `waitParams` | `string` | `True` | `True`, `False` | params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.<br/>True: 웹 엘리먼트의 로딩을 대기한다.<br/>False: 웹 엘리먼트의 로딩을 대기하지 않는다. |

## Property Notes
### `encoding`
웹 브라우저 기본 인코딩 값 지정합니다.
ex) "utf-8"

### `selector`
대상 웹 객체를 선택합니다.
ex) selector_1

### `value`
웹 엘리먼트에 입력할 값을 문자열로 지정합니다.
ex) "환율"

### `waitParams`
params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.
True: 웹 엘리먼트의 로딩을 대기한다.
False: 웹 엘리먼트의 로딩을 대기하지 않는다.

