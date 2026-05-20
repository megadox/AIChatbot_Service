# Activity: ReadPDFTextAsList

## Summary
PDF파일로부터 텍스트 데이터를 추출하여 Name필드에 지정된 변수에 리스트 형태로 저장하는 액티비티

## Metadata
- group: `PDF`
- script: `PDF.read_pdf_text_as_collection()`
- pattern: `PDF\.read_pdf_text_as_collection\(`
- dependencies: `PDF`
- prefix: `collection`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `filePath` | `string` | `-` | - | 텍스트 데이터를 추출할 PDF 파일의 경로를 지정합니다.<br/>ex) "C:\\pdf\\sample.pdf" |
| `password` | `string` | `-` | - | 텍스트 데이터를 추출할 PDF 파일에 설정된 비밀번호를 지정합니다.<br/>ex) "batem12345" |

## Property Notes
### `filePath`
텍스트 데이터를 추출할 PDF 파일의 경로를 지정합니다.
ex) "C:\\pdf\\sample.pdf"

### `password`
텍스트 데이터를 추출할 PDF 파일에 설정된 비밀번호를 지정합니다.
ex) "batem12345"

