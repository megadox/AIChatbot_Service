# Activity: SelectDropboxItem

## Summary
웹 페이지에서 드롭박스의 아이템(select 태그)을 valuueType필드 값의 방식으로 선택하는 액티비티

## Metadata
- group: `WEB`
- script: `select_drop_down()`
- pattern: `\.select_drop_down\(`
- dependencies: `WEB`
- theme: `Accent1`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `selector` | `string` | `-` | - | 대상 웹 객체를 선택합니다.<br/>ex) selector_1 |
| `value` | `string` | `` | - | valueType에서 지정한 종류에 해당하는 값을 지정합니다.<br/>ex) 0 (index 방식)<br/>ex) "abc" (value 또는 text 방식) |
| `valueType` | `string` | `"value"` | `"index"`, `"value"`, `"text"` | value의 값의 종류를 지정합니다.<br/>"index": 옵션을 인덱스 방식으로 선택한다.<br/>"value": value속성의 값으로 선택한다.<br/>"text": 옵션의 text 값으로 선택한다. |
| `waitParams` | `string` | `True` | `True`, `False` | params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.<br/>True: 웹 엘리먼트의 로딩을 대기한다.<br/>False: 웹 엘리먼트의 로딩을 대기하지 않는다. |

## Property Notes
### `selector`
대상 웹 객체를 선택합니다.
ex) selector_1

### `value`
valueType에서 지정한 종류에 해당하는 값을 지정합니다.
ex) 0 (index 방식)
ex) "abc" (value 또는 text 방식)

### `valueType`
value의 값의 종류를 지정합니다.
"index": 옵션을 인덱스 방식으로 선택한다.
"value": value속성의 값으로 선택한다.
"text": 옵션의 text 값으로 선택한다.

### `waitParams`
params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.
True: 웹 엘리먼트의 로딩을 대기한다.
False: 웹 엘리먼트의 로딩을 대기하지 않는다.

