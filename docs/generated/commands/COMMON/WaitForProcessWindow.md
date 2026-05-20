# Activity: WaitForProcessWindow

## Summary
특정 윈도우 애플리케이션의 창이 닫힐 때까지 대기하는 액티비티

## Metadata
- group: `COMMON`
- script: `ENVIRONMENT.WaitForProcessWindow()`
- pattern: `ENVIRONMENT\.WaitForProcessWindow\(`
- dependencies: `ENVIRONMENT`
- theme: `Dark_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `processTitle` | `string` | `-` | - | 종료 대기할 윈도우 창의 이름을 지정합니다.<br/>ex) "메모장" |
| `timeout` | `string` | `30000` | - | 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |

## Property Notes
### `processTitle`
종료 대기할 윈도우 창의 이름을 지정합니다.
ex) "메모장"

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

