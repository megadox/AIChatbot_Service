# Activity: SwipeImageToImage

## Summary
안드로이드 디바이스의 특정 이미지에서 다른 이미지로 스와이프하는 액티비티

## Metadata
- group: `ANDROID`
- script: `android_swipe_on_image()`
- pattern: `device\.android_swipe_on_image\(`
- dependencies: `ANDROID`
- theme: `Accent6`
- prefix: `swiped`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `confidence` | `string` | `"90%"` | `"90%"`, `"80%"`, `"70%"`, `"60%"`, `"50%"` | 대상 이미지의 신뢰도를 지정합니다.(startImage와 endImage동시 적용)<br/>ex) "90%" (90퍼센트만 일치해도 같은 이미지로 인정) |
| `duration` | `string` | `1000` | `1000`, `2000`, `3000`, `4000`, `5000` | 스와이프하는 시간을 지정합니다.<br/>ex) 2000 |
| `endImage` | `string` | `-` | - | 스와이프 끝 이미지의 이름을 지정합니다.<br/>image_1 |
| `endImageCollection` | `string` | `-` | - | 스와이프 끝 이미지 컬렉션을 지정합니다. 그 중 단 하나의 이미지라도 찾는다면 탭이 발생합니다. <br/>ex) ['image_0', 'image_1']<br/>(이미지 컬렉션을 사용할 경우 endImage 프로퍼티는 사용하지 않습니다.) |
| `startImage` | `string` | `-` | - | 스와이프 시작 이미지의 이름을 지정합니다.<br/>image_0 |
| `startImageCollection` | `string` | `-` | - | 스와이프 시작 이미지 컬렉션을 지정합니다. 그 중 단 하나의 이미지라도 찾는다면 탭이 발생합니다. <br/>ex) ['image_0', 'image_1']<br/>(이미지 컬렉션을 사용할 경우 startImage 프로퍼티는 사용하지 않습니다.) |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 대기할 최대 시간을 지정합니다.(startImage와 endImage동시 적용)<br/>ex) 30000 (단위: 밀리세컨) |
| `waiting` | `string` | `True` | `True`, `False` | 스와이프할 이미지 대기 여부를 지정합니다.<br/>True: 이미지가 나타날 때까지 대기한다.<br/>False: 이미지가 나타날 때까지 대기하지 않는다.<br/>(startImage와 endImage동시 적용) |

## Property Notes
### `confidence`
대상 이미지의 신뢰도를 지정합니다.(startImage와 endImage동시 적용)
ex) "90%" (90퍼센트만 일치해도 같은 이미지로 인정)

### `duration`
스와이프하는 시간을 지정합니다.
ex) 2000

### `endImage`
스와이프 끝 이미지의 이름을 지정합니다.
image_1

### `endImageCollection`
스와이프 끝 이미지 컬렉션을 지정합니다. 그 중 단 하나의 이미지라도 찾는다면 탭이 발생합니다. 
ex) ['image_0', 'image_1']
(이미지 컬렉션을 사용할 경우 endImage 프로퍼티는 사용하지 않습니다.)

### `startImage`
스와이프 시작 이미지의 이름을 지정합니다.
image_0

### `startImageCollection`
스와이프 시작 이미지 컬렉션을 지정합니다. 그 중 단 하나의 이미지라도 찾는다면 탭이 발생합니다. 
ex) ['image_0', 'image_1']
(이미지 컬렉션을 사용할 경우 startImage 프로퍼티는 사용하지 않습니다.)

### `timeout`
대기할 최대 시간을 지정합니다.(startImage와 endImage동시 적용)
ex) 30000 (단위: 밀리세컨)

### `waiting`
스와이프할 이미지 대기 여부를 지정합니다.
True: 이미지가 나타날 때까지 대기한다.
False: 이미지가 나타날 때까지 대기하지 않는다.
(startImage와 endImage동시 적용)

