# Activity: ToPNG

## Summary
PDF파일을 이미지 파일로 변환하는 액티비티

## Metadata
- group: `PDF`
- script: `PDF.to_image()`
- pattern: `PDF\.to_image\(`
- dependencies: `PDF`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `destDir` | `string` | `-` | - | 이미지로 저장할 파일의 저장 폴더 경로를 지정합니다.<br/>ex) "C:\images" |
| `filePath` | `string` | `-` | - | 이미지 파일로 변환할 PDF 파일의 경로를 지정합니다.<br/>ex) "C:\pdf\sample.pdf" |
| `fromPage` | `string` | `-` | - | 이미지로 변환하려는 PDF파일의 시작 페이지 번호를 지정합니다.<br/>ex) 1 |
| `password` | `string` | `-` | - | 이미지 파일로 변환할 PDF 파일에 설정된 비밀번호를 지정합니다.<br/>ex) "batem12345" |
| `prefix` | `string` | `-` | - | 저장하는 파일 이름의 앞부분을 지정합니다.<br/>ex) 변환이미지<br/>(뒷부분은 페이지 번호가 붙여진다.) |
| `toPage` | `string` | `-` | - | 이미지로 변환하려는 PDF파일의 끝 페이지 번호를 지정합니다.<br/>ex) 2 |
| `zoomLevel` | `string` | `1` | `1`, `2`, `3` | 이미지의 확대/축소 배율을 지정합니다.<br/>ex) 1.5 |

## Property Notes
### `destDir`
이미지로 저장할 파일의 저장 폴더 경로를 지정합니다.
ex) "C:\images"

### `filePath`
이미지 파일로 변환할 PDF 파일의 경로를 지정합니다.
ex) "C:\pdf\sample.pdf"

### `fromPage`
이미지로 변환하려는 PDF파일의 시작 페이지 번호를 지정합니다.
ex) 1

### `password`
이미지 파일로 변환할 PDF 파일에 설정된 비밀번호를 지정합니다.
ex) "batem12345"

### `prefix`
저장하는 파일 이름의 앞부분을 지정합니다.
ex) 변환이미지
(뒷부분은 페이지 번호가 붙여진다.)

### `toPage`
이미지로 변환하려는 PDF파일의 끝 페이지 번호를 지정합니다.
ex) 2

### `zoomLevel`
이미지의 확대/축소 배율을 지정합니다.
ex) 1.5

