# Activity: Exists

## Summary
특정 위치에 폴더나 파일이 존재하는지 확인하여 Bool값을 Name필드에 지정된 변수에 저장하는 액티비티

## Metadata
- group: `FILE`
- script: `FILE.Exists()`
- pattern: `FILE\.Exists`
- dependencies: `FILE`
- theme: `Accent5_5`
- prefix: `exist`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `path` | `string` | `-` | - | 존재를 검사할 파일/폴더의 경로를 지정합니다.<br/>ex) "C:\temp\sample.txt" |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |
| `waiting` | `string` | `True` | `True`, `False` | 파일의 존재 여부를 timeout 필드의 값 동안 대기할지 여부를 지정합니다.<br/>True: 대기 시간동안 대기한다.<br/>False: 대기하지 않는다. |

## Property Notes
### `path`
존재를 검사할 파일/폴더의 경로를 지정합니다.
ex) "C:\temp\sample.txt"

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

### `waiting`
파일의 존재 여부를 timeout 필드의 값 동안 대기할지 여부를 지정합니다.
True: 대기 시간동안 대기한다.
False: 대기하지 않는다.

