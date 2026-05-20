# Activity: ConvertToGrayscale

## Summary
특정 이미지 파일을 Greyscale로 변환하여 특정 위치에 저장하는 액티비티

## Metadata
- group: `IMAGE`
- script: `IMAGE.to_greyscale()`
- pattern: `IMAGE.\.to_greyscale\(`
- dependencies: `IMAGE`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `destPath` | `string` | `-` | - | 변환하여 저장할 파일의 경로를 지정합니다.<br/>ex) "C:\result\sample2.jpg" |
| `srcPath` | `string` | `-` | - | 변환 대상 원본 파일의 경로를 지정합니다.<br/>ex) "C:\imgs\sample.jpg" |
| `threshold` | `string` | `100` | `0`, `50`, `100`, `150`, `200` | 임계값을 지정합니다.<br/>ex) 20<br/>(밝게 0 ~ 255 어둡게) |

## Property Notes
### `destPath`
변환하여 저장할 파일의 경로를 지정합니다.
ex) "C:\result\sample2.jpg"

### `srcPath`
변환 대상 원본 파일의 경로를 지정합니다.
ex) "C:\imgs\sample.jpg"

### `threshold`
임계값을 지정합니다.
ex) 20
(밝게 0 ~ 255 어둡게)

