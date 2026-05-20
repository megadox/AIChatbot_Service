# Activity: Attach

## Summary
열려있는 엑셀파일을 연결할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `EXCEL.Attach()`
- pattern: `= *EXCEL\.Attach\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `excel`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `retry` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 재시도 횟수를 지정합니다.<br/>ex) 3<br/>(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.) |

## Property Notes
### `retry`
재시도 횟수를 지정합니다.
ex) 3
(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.)

