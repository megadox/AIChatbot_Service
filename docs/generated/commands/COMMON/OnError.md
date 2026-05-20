# Activity: OnError

## Summary
에러가 발생했을 경우 대체 업무를 수행하기 위한 액티비티

## Metadata
- group: `COMMON`
- script: `PRE-PROCESSING!SetOnError()`
- pattern: `ONERROR`
- theme: `Dark_5`
- prefix: `onerror`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `comment` | `string` | `"On Error"` | - | On Error 액티비티를 수행한 경우 남기는 메시지를 지정합니다.<br/>ex) "ON ERROR" |
| `importance` | `string` | `100` | - | 복수개의 On Error 액티비티가 존재할 때 중요도를 지정합니다.<br/>ex) 100<br/>(숫자가 높을수록 중요도가 높다.) |
| `MaxCount` | `string` | `50` | - | On Error 액티비티의 최대 수행가능 횟수를 지정합니다.<br/>ex) 50 |

## Property Notes
### `comment`
On Error 액티비티를 수행한 경우 남기는 메시지를 지정합니다.
ex) "ON ERROR"

### `importance`
복수개의 On Error 액티비티가 존재할 때 중요도를 지정합니다.
ex) 100
(숫자가 높을수록 중요도가 높다.)

### `MaxCount`
On Error 액티비티의 최대 수행가능 횟수를 지정합니다.
ex) 50

