# Activity: CompressFolder

## Summary
특정 폴더를 zip파일로 압축하는 액티비티

## Metadata
- group: `FILE`
- script: `FILE.zip_folder()`
- pattern: `FILE\.zip_folder\(`
- dependencies: `FILE`
- theme: `Accent5_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `destPath` | `string` | `-` | - | 결과 압축 파일의 저장 경로를 지정합니다.<br/>ex) "C:\temp\result.zip" |
| `srcPath` | `string` | `-` | - | 압축할 폴더의 경로를 지정합니다.<br/>ex) "C:\temp" |

## Property Notes
### `destPath`
결과 압축 파일의 저장 경로를 지정합니다.
ex) "C:\temp\result.zip"

### `srcPath`
압축할 폴더의 경로를 지정합니다.
ex) "C:\temp"

