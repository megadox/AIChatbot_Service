# Activity: RunProcessUntilEnded

## Summary
특정 프로세스가 끝날 때까지 대기하는 액티비티

## Metadata
- group: `COMMON`
- script: `ENVIRONMENT.RunProcessUntilEnded()`
- pattern: `ENVIRONMENT\.RunProcessUntilEnded\(`
- dependencies: `ENVIRONMENT`
- theme: `Dark_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `processName` | `string` | `-` | - | 종료 대기할 프로세스의 이름을 지정합니다.<br/>ex) "mspaint.exe"<br/>(이름은 작업관리자에서 참조한다.) |

## Property Notes
### `processName`
종료 대기할 프로세스의 이름을 지정합니다.
ex) "mspaint.exe"
(이름은 작업관리자에서 참조한다.)

