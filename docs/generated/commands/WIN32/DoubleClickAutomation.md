# Activity: DoubleClickAutomation

## Summary
윈도우 애플리케이션 오브젝트(컨트롤)에 마우스 더블클릭을 수행하는 액티비티

## Metadata
- group: `WIN32`
- script: `MSAA.DoubleClick()`
- pattern: `MSAA\.DoubleClick\(`
- dependencies: `MSAA`
- theme: `Accent3_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `anchor` | `string` | `"center"` | `"center"`, `"topLeft"`, `"topRight"`, `"bottomLeft"`, `"bottomRight"` | 더블 클릭할 위치를 지정할 때 기준점을 지정합니다.<br/>"center": 윈도우 객체의 중앙을 기준으로 클릭한다.<br/>"topLeft": 윈도우 객체의 좌상단을 기준으로 클릭한다.<br/>"topRight": 윈도우 객체의 우상단을 기준으로 클릭한다.<br/>"bottomLeft": 윈도우 객체의 좌하단을 기준으로 클릭한다.<br/>"bottomRight": 윈도우 객체의 우하단을 기준으로 클릭한다. |
| `combinationkey` | `string` | `None` | `None`, `"ctrl"`, `"shift"`, `"alt"`, `"win"` | 클릭 시 함께 누를 조합 키를 지정합니다.<br/>None: 조합 키를 사용하지 않습니다.<br/>"ctrl": Ctrl 키를 함께 누릅니다.<br/>"shift": Shift 키를 함께 누릅니다.<br/>"alt": Alt 키를 함께 누릅니다.<br/>"win": Windows 키를 함께 누릅니다. |
| `move` | `string` | `False` | `True`, `False` | 마우스 포인트 이동 애니메이션 사용 여부를 지정합니다.<br/>True: 마우스 이동 애니메이션을 표시한다.<br/>False: 마우스 이동 애니메이션을 표시하지 않는다. |
| `offsetX` | `string` | `0` | - | 더블 클릭할 위치의 X축 오프셋을 지정합니다.<br/>ex) 10 |
| `offsetY` | `string` | `0` | - | 더블 클릭할 위치의 Y축 오프셋을 지정합니다.<br/>ex) 10 |
| `searchMode` | `string` | `"All"` | `"All"`, `"UIA"`, `"MSAA"` | 대상 윈도우 객체 검색 방식을 지정합니다.<br/>"All": UIA와 MSAA를 순차적으로 사용합니다.<br/>"UIA": UIA 방식으로만 검색합니다.<br/>"MSAA": MSAA 방식으로만 검색합니다. |
| `selector` | `string` | `-` | - | 대상 윈도우 객체를 선택합니다.<br/>ex) selector_0 |
| `timeout` | `string` | `30` | `5`, `10`, `20`, `30`, `40`, `50`, `60` | 윈도우 객체를 찾는 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |

## Property Notes
### `anchor`
더블 클릭할 위치를 지정할 때 기준점을 지정합니다.
"center": 윈도우 객체의 중앙을 기준으로 클릭한다.
"topLeft": 윈도우 객체의 좌상단을 기준으로 클릭한다.
"topRight": 윈도우 객체의 우상단을 기준으로 클릭한다.
"bottomLeft": 윈도우 객체의 좌하단을 기준으로 클릭한다.
"bottomRight": 윈도우 객체의 우하단을 기준으로 클릭한다.

### `combinationkey`
클릭 시 함께 누를 조합 키를 지정합니다.
None: 조합 키를 사용하지 않습니다.
"ctrl": Ctrl 키를 함께 누릅니다.
"shift": Shift 키를 함께 누릅니다.
"alt": Alt 키를 함께 누릅니다.
"win": Windows 키를 함께 누릅니다.

### `move`
마우스 포인트 이동 애니메이션 사용 여부를 지정합니다.
True: 마우스 이동 애니메이션을 표시한다.
False: 마우스 이동 애니메이션을 표시하지 않는다.

### `offsetX`
더블 클릭할 위치의 X축 오프셋을 지정합니다.
ex) 10

### `offsetY`
더블 클릭할 위치의 Y축 오프셋을 지정합니다.
ex) 10

### `searchMode`
대상 윈도우 객체 검색 방식을 지정합니다.
"All": UIA와 MSAA를 순차적으로 사용합니다.
"UIA": UIA 방식으로만 검색합니다.
"MSAA": MSAA 방식으로만 검색합니다.

### `selector`
대상 윈도우 객체를 선택합니다.
ex) selector_0

### `timeout`
윈도우 객체를 찾는 최대 시간을 지정합니다.
ex) 30 (단위: 초)

