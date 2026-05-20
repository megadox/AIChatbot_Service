# Activity: StartSchedule

## Summary
서버에 설정한 예약 작업을 즉시 실행하도록 명령하는 액티비티

## Metadata
- group: `SERVER`
- script: `SERVER.start_schedule()`
- pattern: `SERVER\.start_schedule\(`
- dependencies: `SERVER`
- theme: `Accent2`
- prefix: `result`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `schedules` | `string` | `-` | `ServerAPI/GetScheduleList` | 즉시 실행할 스케쥴 이름을 지정합니다.<br/>ex) "MySchedule" |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 연결 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |

## Property Notes
### `schedules`
즉시 실행할 스케쥴 이름을 지정합니다.
ex) "MySchedule"

### `timeout`
연결 대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

