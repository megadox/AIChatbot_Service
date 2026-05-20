# Activity: TapOnImage

## Summary
안드로이드 디바이스의 이미지를 탭하는 액티비티

## Metadata
- group: `ANDROID`
- script: `android_tap_on_image()`
- pattern: `device\.android_tap_on_image\(`
- dependencies: `ANDROID`
- theme: `Accent6`
- prefix: `tapped`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `confidence` | `string` | `"90%"` | `"90%"`, `"80%"`, `"70%"`, `"60%"`, `"50%"` | 대상 이미지의 신뢰도를 지정합니다.<br/>ex) "90%" (90퍼센트만 일치해도 같은 이미지로 인정) |
| `image` | `string` | `-` | - | 탭할 이미지의 이름을 지정합니다.<br/>image_0 |
| `imageCollection` | `string` | `-` | - | 탭할 이미지 컬렉션을 지정합니다. 그 중 단 하나의 이미지라도 찾는다면 탭이 발생합니다. <br/>ex) ['image_0', 'image_1']<br/>(이미지 컬렉션을 사용할 경우 image 프로퍼티는 사용하지 않습니다.) |
| `taps` | `string` | `1` | `1`, `2`, `3` | 탭의 횟수를 지정합니다.<br/>ex) 2 |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |
| `waiting` | `string` | `True` | `True`, `False` | 탭할 이미지 대기 여부를 지정합니다.<br/>True: 이미지가 나타날 때까지 대기한다.<br/>False: 이미지가 나타날 때까지 대기하지 않는다. |
| `x` | `string` | `"50%"` | - | 탭할 x좌표의 위치를 지정합니다.<br/> ex) "50%" |
| `y` | `string` | `"50%"` | - | 탭할 y좌표의 위치를 지정합니다.<br/> ex) "50%" |

## Property Notes
### `confidence`
대상 이미지의 신뢰도를 지정합니다.
ex) "90%" (90퍼센트만 일치해도 같은 이미지로 인정)

### `image`
탭할 이미지의 이름을 지정합니다.
image_0

### `imageCollection`
탭할 이미지 컬렉션을 지정합니다. 그 중 단 하나의 이미지라도 찾는다면 탭이 발생합니다. 
ex) ['image_0', 'image_1']
(이미지 컬렉션을 사용할 경우 image 프로퍼티는 사용하지 않습니다.)

### `taps`
탭의 횟수를 지정합니다.
ex) 2

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

### `waiting`
탭할 이미지 대기 여부를 지정합니다.
True: 이미지가 나타날 때까지 대기한다.
False: 이미지가 나타날 때까지 대기하지 않는다.

### `x`
탭할 x좌표의 위치를 지정합니다.
 ex) "50%"

### `y`
탭할 y좌표의 위치를 지정합니다.
 ex) "50%"

