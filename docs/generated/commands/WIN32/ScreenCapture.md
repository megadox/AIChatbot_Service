# Activity: ScreenCapture

## Summary
윈도우 화면을 캡쳐할 때 사용하는 액티비티

## Metadata
- group: `WIN32`
- script: `WIN32.screen_capture()`
- pattern: `WIN32\.screen_capture\(`
- dependencies: `WIN32`
- theme: `Accent3_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `path` | `string` | `-` | - | 캡쳐한 이미지를 저장할 경로를 지정합니다.<br/>ex) "C:\\image\\screenShot.png" |
| `region` | `string` | `"full"` | `"full"`, `"0,0,500,500"`, `"0,0,1920,1080"` | 캡쳐할 스크린 범위를 지정합니다.<br/>"full": 전체화면을 캡쳐한다.<br/>"0,0,100,100": 시작 x좌표, 시작 y좌표, 끝 x좌표, 끝 y좌표의 범위를 캡쳐한다. |

## Property Notes
### `path`
캡쳐한 이미지를 저장할 경로를 지정합니다.
ex) "C:\\image\\screenShot.png"

### `region`
캡쳐할 스크린 범위를 지정합니다.
"full": 전체화면을 캡쳐한다.
"0,0,100,100": 시작 x좌표, 시작 y좌표, 끝 x좌표, 끝 y좌표의 범위를 캡쳐한다.

