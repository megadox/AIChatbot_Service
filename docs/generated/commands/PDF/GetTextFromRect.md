# Activity: GetTextFromRect

## Summary
PDF파일에서 특정 영역의 텍스트 데이터를 읽고 Name필드에 지정된 변수에 저장하는 액티비티

## Metadata
- group: `PDF`
- script: `PDF.get_text_from_rect()`
- pattern: `PDF\.get_text_from_rect\(`
- dependencies: `PDF`
- prefix: `text`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `height` | `string` | `-` | - | 텍스트를 추출할 특정 영역의 높이를 지정합니다.<br/>ex) 300 |
| `left` | `string` | `-` | - | 텍스트를 추출할 특정 영역의 x 지점을 지정합니다.<br/>ex) 0 |
| `page` | `string` | `-` | - | 텍스트 데이터를 추출할 PDF 파일의 페이지를 지정합니다.<br/>ex) 3 |
| `password` | `string` | `-` | - | 텍스트로 가져올 PDF 파일에 설정된 비밀번호를 지정합니다.<br/>ex) "batem12345" |
| `path` | `string` | `-` | - | 텍스트 데이터를 추출할 PDF 파일의 경로를 지정합니다.<br/>ex) "C:\pdf\sample.pdf" |
| `top` | `string` | `-` | - | 텍스트를 추출할 특정 영역의 y 지점을 지정합니다.<br/>ex) 0 |
| `width` | `string` | `-` | - | 텍스트를 추출할 특정 영역의 너비를 지정합니다.<br/>ex) 100 |

## Property Notes
### `height`
텍스트를 추출할 특정 영역의 높이를 지정합니다.
ex) 300

### `left`
텍스트를 추출할 특정 영역의 x 지점을 지정합니다.
ex) 0

### `page`
텍스트 데이터를 추출할 PDF 파일의 페이지를 지정합니다.
ex) 3

### `password`
텍스트로 가져올 PDF 파일에 설정된 비밀번호를 지정합니다.
ex) "batem12345"

### `path`
텍스트 데이터를 추출할 PDF 파일의 경로를 지정합니다.
ex) "C:\pdf\sample.pdf"

### `top`
텍스트를 추출할 특정 영역의 y 지점을 지정합니다.
ex) 0

### `width`
텍스트를 추출할 특정 영역의 너비를 지정합니다.
ex) 100

