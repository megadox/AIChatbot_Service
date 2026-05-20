# Activity: DecompressFile

## Summary
특정 압축파일을 해제하는 액티비티

## Metadata
- group: `FILE`
- script: `FILE.unzip()`
- pattern: `FILE\.unzip\(`
- dependencies: `FILE`
- theme: `Accent5_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `destPath` | `string` | `-` | - | 압축을 풀 폴더의 경로를 지정합니다.<br/>ex) "C:\result" |
| `srcPath` | `string` | `-` | - | 압축 해제할 파일의 경로를 지정합니다.<br/>ex) "C:\temp\sample.zip" |

## Property Notes
### `destPath`
압축을 풀 폴더의 경로를 지정합니다.
ex) "C:\result"

### `srcPath`
압축 해제할 파일의 경로를 지정합니다.
ex) "C:\temp\sample.zip"

