# Activity: GetPDFText

## Summary
PDF파일로부터 텍스트 데이터를 추출하여 Name필드에 지정된 변수에 저장하는 액티비티

## Metadata
- group: `PDF`
- script: `PDF.GetPDFText()`
- pattern: `PDF\.GetPDFText\(`
- dependencies: `PDF`
- prefix: `text`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `password` | `string` | `-` | - | 텍스트 데이터를 추출할 PDF 파일에 설정된 비밀번호를 지정합니다.<br/>ex) "batem12345" |
| `path` | `string` | `-` | - | 텍스트 데이터를 추출할 PDF 파일의 경로를 지정합니다.<br/>ex) "C:\pdf\sample.pdf" |

## Property Notes
### `password`
텍스트 데이터를 추출할 PDF 파일에 설정된 비밀번호를 지정합니다.
ex) "batem12345"

### `path`
텍스트 데이터를 추출할 PDF 파일의 경로를 지정합니다.
ex) "C:\pdf\sample.pdf"

