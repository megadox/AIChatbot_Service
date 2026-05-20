# Activity: KillProcess

## Summary
특정 프로세스를 강제 종료시키는 액티비티

## Metadata
- group: `COMMON`
- script: `ENVIRONMENT.KillProcess()`
- pattern: `ENVIRONMENT\.KillProcess\(`
- dependencies: `ENVIRONMENT`
- theme: `Dark_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `processName` | `string` | `-` | - | 실행중인 프로세스의 이름을 지정합니다.<br/>ex) "mspaint.exe"<br/>(이름은 작업관리자에서 참조한다.) |

## Property Notes
### `processName`
실행중인 프로세스의 이름을 지정합니다.
ex) "mspaint.exe"
(이름은 작업관리자에서 참조한다.)

