# Activity: TesseractOCRImageToText

## Summary
OCR을 이용하여 이미지에서 텍스트를 추출하는 액티비티

## Metadata
- group: `AI`
- script: `OCR.extract_string_from_image()`
- pattern: `OCR.\.extract_string_from_image\(`
- dependencies: `OCR`
- theme: `Accent2_4`
- prefix: `text`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `canny` | `string` | `False` | `False`, `True` | 테두리 윤곽 보정 옵션 사용을 지정합니다.<br/>True: 테두리 윤곽 보정 옵션을 사용한다.<br/>False: 테두리 윤곽 보정 옵션을 사용하지 않는다. |
| `closing` | `string` | `False` | `False`, `True` | closing 옵션 사용을 지정합니다.<br/>True: closing 옵션을 사용한다.<br/>False: closing 옵션을 사용하지 않는다. |
| `deskew` | `string` | `False` | `False`, `True` | 이미지 기울기 보정 옵션 사용을 지정합니다.<br/>True: 이미지 기울기 보정 옵션을 사용한다.<br/>False: 이미지 기울기 보정 옵션을 사용하지 않는다. |
| `dilate` | `string` | `False` | `False`, `1`, `2`, `3`, `4`, `5` | 팽창 옵션을 지정합니다.<br/>ex) 2 |
| `erode` | `string` | `False` | `False`, `1`, `2`, `3`, `4`, `5` | 침식 옵션을 지정합니다.<br/>ex) 5 |
| `extractRange` | `string` | `"Full"` | `"Full"`, `"x, y, width, height"` | 텍스트 추출 범위를 지정합니다.<br/>ex) 0, 0, 100, 100 (시작 x좌표, 시작 y좌표, 끝 x좌표, 끝 y좌표)<br/>(Full: 이미지 전체에서 추출한다.) |
| `grayscale` | `string` | `False` | `False`, `True` | 회색조 옵션 사용을 지정합니다.<br/>True: 회색조 옵션을 사용한다.<br/>False: 회색조 옵션을 사용하지 않는다. |
| `imgPath` | `string` | `-` | - | OCR을 이용할 이미지 파일의 경로를 지정합니다.<br/>ex) "C:\imgs\sample.jpg" |
| `lang` | `string` | `-` | - | 추출할 언어를 지정합니다.<br/> kor+eng<br/>(+로 다중 언어를 선택할 수 있다.) |
| `OCRengine` | `string` | `r"C:\Program Files\Tesseract-OCR\tesseract.exe"` | - | 테서렉트OCR 엔진의 경로를 지정합니다.<br/>ex) "C:\Program Files\Tesseract-OCR\tesseract.exe" |
| `opening` | `string` | `False` | `False`, `True` | opening 옵션 사용을 지정합니다.<br/>True: opening 옵션을 사용한다.<br/>False: opening 옵션을 사용하지 않는다. |
| `psm` | `string` | `6` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9`, `10`, `11`, `12` | 문단 구분자를 지정합니다.<br/>ex) 6 |
| `removeNoise` | `string` | `False` | `False`, `1`, `3`, `5`, `7`, `9` | 노이즈 제거 옵션을 지정합니다.<br/>ex) 3<br/>(홀수 값만 가능하다.) |
| `threshold` | `string` | `False` | `False`, `0`, `50`, `100`, `150`, `200` | 바이너리 옵션을 지정합니다.<br/>ex) 50 |

## Property Notes
### `canny`
테두리 윤곽 보정 옵션 사용을 지정합니다.
True: 테두리 윤곽 보정 옵션을 사용한다.
False: 테두리 윤곽 보정 옵션을 사용하지 않는다.

### `closing`
closing 옵션 사용을 지정합니다.
True: closing 옵션을 사용한다.
False: closing 옵션을 사용하지 않는다.

### `deskew`
이미지 기울기 보정 옵션 사용을 지정합니다.
True: 이미지 기울기 보정 옵션을 사용한다.
False: 이미지 기울기 보정 옵션을 사용하지 않는다.

### `dilate`
팽창 옵션을 지정합니다.
ex) 2

### `erode`
침식 옵션을 지정합니다.
ex) 5

### `extractRange`
텍스트 추출 범위를 지정합니다.
ex) 0, 0, 100, 100 (시작 x좌표, 시작 y좌표, 끝 x좌표, 끝 y좌표)
(Full: 이미지 전체에서 추출한다.)

### `grayscale`
회색조 옵션 사용을 지정합니다.
True: 회색조 옵션을 사용한다.
False: 회색조 옵션을 사용하지 않는다.

### `imgPath`
OCR을 이용할 이미지 파일의 경로를 지정합니다.
ex) "C:\imgs\sample.jpg"

### `lang`
추출할 언어를 지정합니다.
 kor+eng
(+로 다중 언어를 선택할 수 있다.)

### `OCRengine`
테서렉트OCR 엔진의 경로를 지정합니다.
ex) "C:\Program Files\Tesseract-OCR\tesseract.exe"

### `opening`
opening 옵션 사용을 지정합니다.
True: opening 옵션을 사용한다.
False: opening 옵션을 사용하지 않는다.

### `psm`
문단 구분자를 지정합니다.
ex) 6

### `removeNoise`
노이즈 제거 옵션을 지정합니다.
ex) 3
(홀수 값만 가능하다.)

### `threshold`
바이너리 옵션을 지정합니다.
ex) 50

