# Activity: DoubleClickOnImage

## Summary
윈도우 화면 위의 이미지를 찾아서 마우스 더블클릭을 수행하는 액티비티

## Metadata
- group: `WIN32`
- script: `IMAGE.DoubleClick()`
- pattern: `IMAGE\.DoubleClick\(`
- dependencies: `IMAGE`
- theme: `Accent3_5`
- prefix: `clicked`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `combinationkey` | `string` | `None` | `None`, `"ctrl"`, `"shift"`, `"alt"`, `"win"` | 클릭 시 함께 누를 조합 키를 지정합니다.<br/>None: 조합 키를 사용하지 않습니다.<br/>"ctrl": Ctrl 키를 함께 누릅니다.<br/>"shift": Shift 키를 함께 누릅니다.<br/>"alt": Alt 키를 함께 누릅니다.<br/>"win": Windows 키를 함께 누릅니다. |
| `confidence` | `string` | `"90%"` | `"90%"`, `"80%"`, `"70%"`, `"60%"`, `"50%"` | 대상 이미지의 신뢰도를 지정합니다.<br/>ex) "90%" (90퍼센트만 일치해도 같은 이미지로 인정) |
| `image` | `string` | `-` | - | 더블클릭할 이미지의 이름을 지정합니다.<br/>image_0 |
| `imageCollection` | `string` | `-` | - | 더블클릭할 이미지 컬렉션을 지정합니다. 그 중 단 하나의 이미지라도 찾는다면 더블클릭이 발생합니다. <br/>ex) ['image_0', 'image_1']<br/>(이미지 컬렉션을 사용할 경우 image 프로퍼티는 사용하지 않습니다.) |
| `module` | `string` | `"gdi32"` | `"graphic"`, `"gdi32"`, `"WDD"` | 이미지 캡쳐에 사용할 모듈을 지정합니다.<br/>ex) "gdi32" |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |
| `waiting` | `string` | `True` | `True`, `False` | 더블클릭할 이미지 대기 여부를 지정합니다.<br/>True: 이미지가 나타날 때까지 대기한다.<br/>False: 이미지가 나타날 때까지 대기하지 않는다. |
| `x` | `string` | `"50%"` | - | 커서 클릭할 x좌표의 위치를 지정합니다.<br/> ex) "30%" |
| `y` | `string` | `"50%"` | - | 커서 클릭할 y좌표의 위치를 지정합니다.<br/> ex) "70%" |

## Property Notes
### `combinationkey`
클릭 시 함께 누를 조합 키를 지정합니다.
None: 조합 키를 사용하지 않습니다.
"ctrl": Ctrl 키를 함께 누릅니다.
"shift": Shift 키를 함께 누릅니다.
"alt": Alt 키를 함께 누릅니다.
"win": Windows 키를 함께 누릅니다.

### `confidence`
대상 이미지의 신뢰도를 지정합니다.
ex) "90%" (90퍼센트만 일치해도 같은 이미지로 인정)

### `image`
더블클릭할 이미지의 이름을 지정합니다.
image_0

### `imageCollection`
더블클릭할 이미지 컬렉션을 지정합니다. 그 중 단 하나의 이미지라도 찾는다면 더블클릭이 발생합니다. 
ex) ['image_0', 'image_1']
(이미지 컬렉션을 사용할 경우 image 프로퍼티는 사용하지 않습니다.)

### `module`
이미지 캡쳐에 사용할 모듈을 지정합니다.
ex) "gdi32"

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

### `waiting`
더블클릭할 이미지 대기 여부를 지정합니다.
True: 이미지가 나타날 때까지 대기한다.
False: 이미지가 나타날 때까지 대기하지 않는다.

### `x`
커서 클릭할 x좌표의 위치를 지정합니다.
 ex) "30%"

### `y`
커서 클릭할 y좌표의 위치를 지정합니다.
 ex) "70%"

