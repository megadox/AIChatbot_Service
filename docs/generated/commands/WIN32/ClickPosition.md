# Activity: ClickPosition

## Summary
윈도우 화면의 좌표에 마우스 클릭을 수행하는 액티비티

## Metadata
- group: `WIN32`
- script: `WIN32.ClickPosition()`
- pattern: `WIN32\.ClickPosition\(`
- dependencies: `WIN32`
- theme: `Accent3_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `button` | `string` | `"left"` | `"left"`, `"middle"`, `"right"` | 클릭에 사용할 마우스의 버튼을 지정합니다.<br/>"left": 좌클릭을 사용한다.<br/>"middle": 휠클릭을 사용한다.<br/>"right": 우클릭을 사용한다. |
| `clicks` | `string` | `1` | `1`, `2`, `3` | 클릭의 횟수를 지정합니다.<br/>ex) 2 |
| `combinationkey` | `string` | `None` | `None`, `"ctrl"`, `"shift"`, `"alt"`, `"win"` | 클릭 시 함께 누를 조합 키를 지정합니다.<br/>None: 조합 키를 사용하지 않습니다.<br/>"ctrl": Ctrl 키를 함께 누릅니다.<br/>"shift": Shift 키를 함께 누릅니다.<br/>"alt": Alt 키를 함께 누릅니다.<br/>"win": Windows 키를 함께 누릅니다. |
| `x` | `string` | `100` | - | 클릭할 마우스 x좌표를 지정합니다.<br/>ex) 122 |
| `y` | `string` | `100` | - | 클릭할 마우스 y좌표를 지정합니다.<br/>ex) 523 |

## Property Notes
### `button`
클릭에 사용할 마우스의 버튼을 지정합니다.
"left": 좌클릭을 사용한다.
"middle": 휠클릭을 사용한다.
"right": 우클릭을 사용한다.

### `clicks`
클릭의 횟수를 지정합니다.
ex) 2

### `combinationkey`
클릭 시 함께 누를 조합 키를 지정합니다.
None: 조합 키를 사용하지 않습니다.
"ctrl": Ctrl 키를 함께 누릅니다.
"shift": Shift 키를 함께 누릅니다.
"alt": Alt 키를 함께 누릅니다.
"win": Windows 키를 함께 누릅니다.

### `x`
클릭할 마우스 x좌표를 지정합니다.
ex) 122

### `y`
클릭할 마우스 y좌표를 지정합니다.
ex) 523

