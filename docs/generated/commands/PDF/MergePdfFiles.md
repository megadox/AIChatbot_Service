# Activity: MergePdfFiles

## Summary
여러 개의 PDF파일을 하나로 통합하는 액티비티

## Metadata
- group: `PDF`
- script: `PDF.merge_pdf_files()`
- pattern: `PDF\.merge_pdf_files\(`
- dependencies: `PDF`
- prefix: `merge`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `files` | `string` | `-` | - | 병합 대상 파일 리스트를 지정합니다.<br/>['.\file1.pdf', '.\file2.pdf', '.\file3.pdf']<br/> |
| `savePath` | `string` | `-` | - | 병합한 파일의 저장 경로를 지정합니다.<br/>ex) "C:\mergeResult\result.pdf" |

## Property Notes
### `files`
병합 대상 파일 리스트를 지정합니다.
['.\file1.pdf', '.\file2.pdf', '.\file3.pdf']

### `savePath`
병합한 파일의 저장 경로를 지정합니다.
ex) "C:\mergeResult\result.pdf"

