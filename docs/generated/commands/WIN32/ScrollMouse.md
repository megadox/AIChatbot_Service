# Activity: ScrollMouse

## Summary
현재 마우스 포인트 위치에서 마우스 스크롤을 수행하는 액티비티

## Metadata
- group: `WIN32`
- script: `WIN32.scroll_mouse()`
- pattern: `win32\.scroll_mouse`
- dependencies: `WIN32`
- theme: `Accent3_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `current` | `string` | `False` | `True`, `False` | 현재 마우스 커서의 위치 사용 여부를 지정합니다.<br/>True: 현재 마우스 커서 위치에서 스크롤한다.<br/>False: x, y좌표에서 스크롤한다. |
| `wheelCount` | `string` | `1` | - | 스크롤할 휠의 횟수를 지정합니다.<br/>ex) -10<br/>(값이 양수면 위로 스크롤/값이 음수면 아래로 스크롤) |
| `x` | `string` | `0` | - | current 필드의 값이 False일 경우 스크롤할 x좌표를 지정합니다.<br/>ex) 300 |
| `y` | `string` | `0` | - | current 필드의 값이 False일 경우 스크롤할 y좌표를 지정합니다.<br/>ex) 400 |

## Property Notes
### `current`
현재 마우스 커서의 위치 사용 여부를 지정합니다.
True: 현재 마우스 커서 위치에서 스크롤한다.
False: x, y좌표에서 스크롤한다.

### `wheelCount`
스크롤할 휠의 횟수를 지정합니다.
ex) -10
(값이 양수면 위로 스크롤/값이 음수면 아래로 스크롤)

### `x`
current 필드의 값이 False일 경우 스크롤할 x좌표를 지정합니다.
ex) 300

### `y`
current 필드의 값이 False일 경우 스크롤할 y좌표를 지정합니다.
ex) 400

