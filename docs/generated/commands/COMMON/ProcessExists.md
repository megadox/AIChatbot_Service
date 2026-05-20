# Activity: ProcessExists

## Summary
특정 프로세스의 존재 여부를 확인하는 액티비티

## Metadata
- group: `COMMON`
- script: `ENVIRONMENT.is_exist_process()`
- pattern: `ENVIRONMENT\.is_exist_process\(`
- dependencies: `ENVIRONMENT`
- theme: `Dark_5`
- prefix: `exist`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `processName` | `string` | `-` | - | 존재 여부를 확인할 프로세스의 이름을 지정합니다.<br/>ex) "Notepad.exe"<br/>(이름은 작업관리자에서 참조한다.) |

## Property Notes
### `processName`
존재 여부를 확인할 프로세스의 이름을 지정합니다.
ex) "Notepad.exe"
(이름은 작업관리자에서 참조한다.)

