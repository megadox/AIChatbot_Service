# Activity: MoveLatestFile

## Summary
특정 폴더에 있는 가장 최신 파일을 특정 경로로 이동시키는 액티비티

## Metadata
- group: `FILE`
- script: `FILE.move_latest_file()`
- pattern: `FILE\.move_latest_file\(`
- dependencies: `FILE`
- theme: `Accent5_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `destPath` | `string` | `-` | - | 이동시켜 저장할 폴더의 경로를 지정합니다.<br/>ex) "C:\result" |
| `ensureDir` | `string` | `True` | `True`, `False` | 목적 폴더가 없을 경우 생성 여부를 지정합니다.<br/>True: 없다면 폴더를 생성한다.<br/>False: 생성하지 않는다. |
| `extension` | `string` | `-` | - | 이동시키고자 하는 특정 확장자명을 지정합니다.<br/>ex) "xlsx" |
| `srcPath` | `string` | `-` | - | 이동 전 원본파일이 위치한 폴더의 경로를 지정합니다.<br/>ex) "C:\temp" |

## Property Notes
### `destPath`
이동시켜 저장할 폴더의 경로를 지정합니다.
ex) "C:\result"

### `ensureDir`
목적 폴더가 없을 경우 생성 여부를 지정합니다.
True: 없다면 폴더를 생성한다.
False: 생성하지 않는다.

### `extension`
이동시키고자 하는 특정 확장자명을 지정합니다.
ex) "xlsx"

### `srcPath`
이동 전 원본파일이 위치한 폴더의 경로를 지정합니다.
ex) "C:\temp"

