# Activity: WaitForImage

## Summary
안드로이드 디바이스의 이미지를 대기하는 액티비티

## Metadata
- group: `ANDROID`
- script: `android_wait_for_image()`
- pattern: `device\.android_wait_for_image\(`
- dependencies: `ANDROID`
- theme: `Accent6`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `confidence` | `string` | `"90%"` | `"90%"`, `"80%"`, `"70%"`, `"60%"`, `"50%"` | 대상 이미지의 신뢰도를 지정합니다.<br/>ex) "90%" (90퍼센트만 일치해도 같은 이미지로 인정) |
| `image` | `string` | `-` | - | 대기할 이미지의 이름을 지정합니다.<br/>image_0 |
| `imageCollection` | `string` | `-` | - | 대기할 이미지 컬렉션을 지정합니다. 그 중 단 하나의 이미지라도 찾는다면 탭이 발생합니다. <br/>ex) ['image_0', 'image_1']<br/>(이미지 컬렉션을 사용할 경우 image 프로퍼티는 사용하지 않습니다.) |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |

## Property Notes
### `confidence`
대상 이미지의 신뢰도를 지정합니다.
ex) "90%" (90퍼센트만 일치해도 같은 이미지로 인정)

### `image`
대기할 이미지의 이름을 지정합니다.
image_0

### `imageCollection`
대기할 이미지 컬렉션을 지정합니다. 그 중 단 하나의 이미지라도 찾는다면 탭이 발생합니다. 
ex) ['image_0', 'image_1']
(이미지 컬렉션을 사용할 경우 image 프로퍼티는 사용하지 않습니다.)

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

