# Activity: WaitForProcess

## Summary
특정 프로세스가 동작할 때까지 대기하는 액티비티

## Metadata
- group: `COMMON`
- script: `ENVIRONMENT.WaitForProcess()`
- pattern: `ENVIRONMENT\.WaitForProcess\(`
- dependencies: `ENVIRONMENT`
- theme: `Dark_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `processName` | `string` | `-` | - | 대기할 프로세스의 이름을 지정합니다.<br/>ex) "mspaint.exe"<br/>(이름은 작업관리자에서 참조한다.) |
| `timeout` | `string` | `30000` | - | 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |

## Property Notes
### `processName`
대기할 프로세스의 이름을 지정합니다.
ex) "mspaint.exe"
(이름은 작업관리자에서 참조한다.)

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

