# Activity: SaveElementImage

## Summary
웹 엘리먼트의 이미지를 다운받아 savePath필드에 지정한 경로에 이미지 파일로 저장하는 액티비티

## Metadata
- group: `WEB`
- script: `SaveElementImage()`
- pattern: `\.SaveElementImage\(`
- dependencies: `WEB`
- theme: `Accent1`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `savePath` | `string` | `-` | - | 이미지를 저장할 경로를 지정합니다.<br/>ex) "C:\images\img1.png" |
| `selector` | `string` | `-` | - | 대상 웹 객체를 선택합니다.<br/>ex) selector_1 |
| `waitParams` | `string` | `True` | `True`, `False` | params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.<br/>True: 웹 엘리먼트의 로딩을 대기한다.<br/>False: 웹 엘리먼트의 로딩을 대기하지 않는다. |

## Property Notes
### `savePath`
이미지를 저장할 경로를 지정합니다.
ex) "C:\images\img1.png"

### `selector`
대상 웹 객체를 선택합니다.
ex) selector_1

### `waitParams`
params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.
True: 웹 엘리먼트의 로딩을 대기한다.
False: 웹 엘리먼트의 로딩을 대기하지 않는다.

