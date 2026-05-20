# Activity: MergePdf

## Summary
두 개의 PDF파일을 하나로 통합하는 액티비티

## Metadata
- group: `PDF`
- script: `PDF.merge_pdf()`
- pattern: `PDF\.merge_pdf\(`
- dependencies: `PDF`
- prefix: `merge`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `filePath1` | `string` | `-` | - | 병합 대상 파일1의 경로를 지정합니다.<br/>ex) "C:\pdf\sample1.pdf" |
| `filePath2` | `string` | `-` | - | 병합 대상 파일2의 경로를 지정합니다.<br/>ex) "C:\pdf\sample2.pdf" |
| `savePath` | `string` | `-` | - | 병합한 파일의 저장 경로를 지정합니다.<br/>ex) "C:\mergeResult\result.pdf" |

## Property Notes
### `filePath1`
병합 대상 파일1의 경로를 지정합니다.
ex) "C:\pdf\sample1.pdf"

### `filePath2`
병합 대상 파일2의 경로를 지정합니다.
ex) "C:\pdf\sample2.pdf"

### `savePath`
병합한 파일의 저장 경로를 지정합니다.
ex) "C:\mergeResult\result.pdf"

