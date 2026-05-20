# Activity: ReadPDFText

## Summary
PDF파일로부터 텍스트 데이터를 읽고 Name필드에 지정된 변수에 저장하는 액티비티

## Metadata
- group: `PDF`
- script: `PDF.read_pdf_text()`
- pattern: `PDF\.read_pdf_text\(`
- dependencies: `PDF`
- prefix: `text`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `filePath` | `string` | `-` | - | 텍스트 데이터를 추출할 PDF 파일의 경로를 지정합니다.<br/>ex) "C:\pdf\sample.pdf" |
| `pageSeparator` | `string` | `"\n"` | - | 페이지 끝부분에 추가할 구분자를 지정합니다.<br/>ex) "\n" |
| `password` | `string` | `-` | - | 텍스트 데이터를 추출할 PDF 파일에 설정된 비밀번호를 지정합니다.<br/>ex) "batem12345" |

## Property Notes
### `filePath`
텍스트 데이터를 추출할 PDF 파일의 경로를 지정합니다.
ex) "C:\pdf\sample.pdf"

### `pageSeparator`
페이지 끝부분에 추가할 구분자를 지정합니다.
ex) "\n"

### `password`
텍스트 데이터를 추출할 PDF 파일에 설정된 비밀번호를 지정합니다.
ex) "batem12345"

