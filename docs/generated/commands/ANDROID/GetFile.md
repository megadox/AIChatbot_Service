# Activity: GetFile

## Summary
안드로이드 디바이스의 파일을 가져오는 액티비티

## Metadata
- group: `ANDROID`
- script: `android_get_file()`
- pattern: `device\.android_get_file\(`
- dependencies: `ANDROID`
- theme: `Accent6`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `local` | `string` | `-` | - | 가져올 파일의 목적 경로를 지정합니다.<br/>ex) "C:\sample.txt" |
| `remote` | `string` | `-` | - | 가져올 파일의 원본 경로를 지정합니다.<br/>ex) "/sdcard/sample.txt" |

## Property Notes
### `local`
가져올 파일의 목적 경로를 지정합니다.
ex) "C:\sample.txt"

### `remote`
가져올 파일의 원본 경로를 지정합니다.
ex) "/sdcard/sample.txt"

