# Activity: SendFile

## Summary
안드로이드 디바이스로 파일을 전송하는 액티비티

## Metadata
- group: `ANDROID`
- script: `android_send_file()`
- pattern: `device\.android_send_file\(`
- dependencies: `ANDROID`
- theme: `Accent6`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `local` | `string` | `-` | - | 전송할 파일의 원본 경로를 지정합니다.<br/>ex) "C:\sample.txt" |
| `remote` | `string` | `-` | - | 전송할 파일의 목적 경로를 지정합니다.<br/>ex) "/sdcard/sample.txt" |

## Property Notes
### `local`
전송할 파일의 원본 경로를 지정합니다.
ex) "C:\sample.txt"

### `remote`
전송할 파일의 목적 경로를 지정합니다.
ex) "/sdcard/sample.txt"

