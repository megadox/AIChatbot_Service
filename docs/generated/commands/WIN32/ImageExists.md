# Activity: ImageExists

## Summary
윈도우 화면 위의 이미지의 존재 여부를 확인하는 액티비티

## Metadata
- group: `WIN32`
- script: `IMAGE.image_exists()`
- pattern: `IMAGE\.image_exists\(`
- dependencies: `IMAGE`
- theme: `Accent3_5`
- prefix: `exist`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `confidence` | `string` | `"90%"` | `"90%"`, `"80%"`, `"70%"`, `"60%"`, `"50%"` | 대상 이미지의 신뢰도를 지정합니다.<br/>ex) "90%" (90퍼센트만 일치해도 같은 이미지로 인정) |
| `image` | `string` | `-` | - | 존재 여부를 확인할 이미지의 이름을 지정합니다.<br/>image_0 |
| `module` | `string` | `"gdi32"` | `"graphic"`, `"gdi32"`, `"WDD"` | 이미지 캡쳐에 사용할 모듈을 지정합니다.<br/>ex) "gdi32" |
| `timeout` | `string` | `30000` | `30000`, `60000`, `120000`, `300000` | 이미지를 찾는 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |

## Property Notes
### `confidence`
대상 이미지의 신뢰도를 지정합니다.
ex) "90%" (90퍼센트만 일치해도 같은 이미지로 인정)

### `image`
존재 여부를 확인할 이미지의 이름을 지정합니다.
image_0

### `module`
이미지 캡쳐에 사용할 모듈을 지정합니다.
ex) "gdi32"

### `timeout`
이미지를 찾는 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

