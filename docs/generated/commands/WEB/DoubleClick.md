# Activity: DoubleClick

## Summary
특정 웹 엘리먼트를 클릭하는 액티비티

## Metadata
- group: `WEB`
- script: `double_click()`
- pattern: `browser.*\.double_click\(`
- dependencies: `WEB`
- theme: `Accent1`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `anchor` | `string` | `"center"` | `"center"`, `"topLeft"`, `"topRight"`, `"bottomLeft"`, `"bottomRight"` | 더블 클릭할 웹 엘리먼트의 클릭 기준 위치를 지정합니다.<br/>ex) "center"<br/>(더블 클릭할 웹 엘리먼트의 클릭 위치를 지정할 수 있다.) |
| `combinationkey` | `string` | `None` | `None`, `"ctrl"`, `"shift"`, `"alt"`, `"win"` | 클릭 시 함께 누를 조합 키를 지정합니다.<br/>None: 조합 키를 사용하지 않습니다.<br/>"ctrl": Ctrl 키를 함께 누릅니다.<br/>"shift": Shift 키를 함께 누릅니다.<br/>"alt": Alt 키를 함께 누릅니다.<br/>"win": Windows 키를 함께 누릅니다. |
| `encoding` | `string` | `"utf-8"` | `"utf-8"`, `"cp949"`, `"utf-16"` | 웹 브라우저 기본 인코딩 값 지정합니다.<br/>ex) "utf-8" |
| `offsetX` | `string` | `0` | - | 더블 클릭할 웹 엘리먼트의 X축 오프셋 값을 지정합니다.<br/>ex) 0 |
| `offsetY` | `string` | `0` | - | 더블 클릭할 웹 엘리먼트의 Y축 오프셋 값을 지정합니다.<br/>ex) 0 |
| `selector` | `string` | `-` | - | 대상 웹 객체를 선택합니다.<br/>ex) selector_1 |
| `waitClickable` | `string` | `False` | `True`, `False` | params에 지정된 웹 엘리먼트의 클릭 가능 상태 대기 여부를 지정합니다.<br/>True: 웹 엘리먼트의 클릭 가능 상태를 대기한다.<br/>False: 웹 엘리먼트의 클릭 가능 상태를 대기하지 않는다. |
| `waitParams` | `string` | `True` | `True`, `False` | params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.<br/>True: 웹 엘리먼트의 로딩을 대기한다.<br/>False: 웹 엘리먼트의 로딩을 대기하지 않는다. |

## Property Notes
### `anchor`
더블 클릭할 웹 엘리먼트의 클릭 기준 위치를 지정합니다.
ex) "center"
(더블 클릭할 웹 엘리먼트의 클릭 위치를 지정할 수 있다.)

### `combinationkey`
클릭 시 함께 누를 조합 키를 지정합니다.
None: 조합 키를 사용하지 않습니다.
"ctrl": Ctrl 키를 함께 누릅니다.
"shift": Shift 키를 함께 누릅니다.
"alt": Alt 키를 함께 누릅니다.
"win": Windows 키를 함께 누릅니다.

### `encoding`
웹 브라우저 기본 인코딩 값 지정합니다.
ex) "utf-8"

### `offsetX`
더블 클릭할 웹 엘리먼트의 X축 오프셋 값을 지정합니다.
ex) 0

### `offsetY`
더블 클릭할 웹 엘리먼트의 Y축 오프셋 값을 지정합니다.
ex) 0

### `selector`
대상 웹 객체를 선택합니다.
ex) selector_1

### `waitClickable`
params에 지정된 웹 엘리먼트의 클릭 가능 상태 대기 여부를 지정합니다.
True: 웹 엘리먼트의 클릭 가능 상태를 대기한다.
False: 웹 엘리먼트의 클릭 가능 상태를 대기하지 않는다.

### `waitParams`
params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.
True: 웹 엘리먼트의 로딩을 대기한다.
False: 웹 엘리먼트의 로딩을 대기하지 않는다.

