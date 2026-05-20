# Activity: ReadCaptcha

## Summary
이미지 CAPTCHA를 입력받아, 딥러닝 모델로 이미지를 인식하고 결과를 반환하는 액티비티

## Metadata
- group: `AI`
- script: `AI.read_captcha()`
- pattern: `AI\.read_captcha\(`
- dependencies: `AI`
- theme: `Accent2_4`
- prefix: `text`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `classes` | `string` | `10` | - | 모델이 분류하도록 학습된 클래스(문자/숫자)의 총 개수를 지정합니다.<br/>예) 10  (0 ~ 9 숫자) |
| `height` | `string` | `-` | - | 모델 학습에 사용된 이미지의 높이(px)를 지정합니다.<br/>예) 50 |
| `image` | `string` | `-` | - | 인식할 CAPTCHA 이미지 파일의 전체 경로를 지정합니다.<br/>예) "C:\\imgs\\captcha.png" |
| `model` | `string` | `"정부24"` | `"정부24"`, `"대법원"`, `r"path\to\.pth"` | 사용할 ONNX 딥러닝 모델을 지정합니다. 사전에 제공된 ONNX 모델을 사용할 경우 모델 이름을, 직접 변환한 .onnx 파일을 사용할 경우 경로를 입력하세요.<br/>예) "정부24"<br/>예) "대법원"<br/>예) "C:\\models\\captcha.onnx" |
| `width` | `string` | `-` | - | 모델 학습에 사용된 이미지의 너비(px)를 지정합니다.<br/>예) 100 |

## Property Notes
### `classes`
모델이 분류하도록 학습된 클래스(문자/숫자)의 총 개수를 지정합니다.
예) 10  (0 ~ 9 숫자)

### `height`
모델 학습에 사용된 이미지의 높이(px)를 지정합니다.
예) 50

### `image`
인식할 CAPTCHA 이미지 파일의 전체 경로를 지정합니다.
예) "C:\\imgs\\captcha.png"

### `model`
사용할 ONNX 딥러닝 모델을 지정합니다. 사전에 제공된 ONNX 모델을 사용할 경우 모델 이름을, 직접 변환한 .onnx 파일을 사용할 경우 경로를 입력하세요.
예) "정부24"
예) "대법원"
예) "C:\\models\\captcha.onnx"

### `width`
모델 학습에 사용된 이미지의 너비(px)를 지정합니다.
예) 100

