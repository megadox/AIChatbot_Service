# Activity: CompressFiles

## Summary
특정 폴더 아래 위치한 여러 파일들을 zip파일로 압축하는 액티비티

## Metadata
- group: `FILE`
- script: `FILE.zip_files()`
- pattern: `FILE\.zip_files\(`
- dependencies: `FILE`
- theme: `Accent5_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `destPath` | `string` | `-` | - | 결과 압축 파일의 저장 경로를 지정합니다.<br/>ex) "C:\temp\result.zip" |
| `extensions` | `string` | `-` | - | 특정 파일 확장자를 가진 파일을 모아 압축할 때 확장자명을 지정합니다.<br/>ex) "xlsx" |
| `includeSubDir` | `string` | `True` | - | 하위 디렉토리도 압축할지 여부를 지정합니다.<br/>True: 하위 디렉토리도 압축한다.<br/>False: 압축하지 않는다. |
| `srcPath` | `string` | `-` | - | 압축할 폴더의 경로를 지정합니다.<br/>ex) "C:\temp" |

## Property Notes
### `destPath`
결과 압축 파일의 저장 경로를 지정합니다.
ex) "C:\temp\result.zip"

### `extensions`
특정 파일 확장자를 가진 파일을 모아 압축할 때 확장자명을 지정합니다.
ex) "xlsx"

### `includeSubDir`
하위 디렉토리도 압축할지 여부를 지정합니다.
True: 하위 디렉토리도 압축한다.
False: 압축하지 않는다.

### `srcPath`
압축할 폴더의 경로를 지정합니다.
ex) "C:\temp"

