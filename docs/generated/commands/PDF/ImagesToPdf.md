# Activity: ImagesToPdf

## Summary
이미지 파일들을 PDF파일로 변환하는 액티비티

## Metadata
- group: `PDF`
- script: `PDF.image_to_pdf()`
- pattern: `PDF\.image_to_pdf\(`
- dependencies: `PDF`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `files` | `string` | `-` | - | PDF로 변환할 이미지 파일의 경로를 리스트로 지정합니다.<br/>ex) [r"C:\images\image1.png", r"C:\images\image2.jpg"] |
| `resolution` | `string` | `100` | `50`, `100`, `150`, `200` | 이미지의 해상도를 지정합니다.<br/>ex) 100 (단위: DPI, dots per inch) |
| `savePath` | `string` | `-` | - | 변환한 결과 PDF 파일의 경로를 지정합니다.<br/>ex) r"C:\pdf\result.pdf" |

## Property Notes
### `files`
PDF로 변환할 이미지 파일의 경로를 리스트로 지정합니다.
ex) [r"C:\images\image1.png", r"C:\images\image2.jpg"]

### `resolution`
이미지의 해상도를 지정합니다.
ex) 100 (단위: DPI, dots per inch)

### `savePath`
변환한 결과 PDF 파일의 경로를 지정합니다.
ex) r"C:\pdf\result.pdf"

