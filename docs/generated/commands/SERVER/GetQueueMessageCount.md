# Activity: GetQueueMessageCount

## Summary
지정한 큐의 대기열 메시지 갯수를 가져오는 액티비티

## Metadata
- group: `SERVER`
- script: `SERVER.get_queue_message_count()`
- pattern: `SERVER\.get_queue_message_count\(`
- dependencies: `SERVER`
- theme: `Accent2`
- prefix: `count`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `messageFilter` | `string` | `-` | `"equal"`, `"startswith"` | 메시지 그룹의 필터 옵션을 지정합니다.<br/>ex) "equal" |
| `messageGroup` | `string` | `-` | - | 필터링할 메시지 그룹 코드를 입력합니다.<br/>ex) "myGroup" |
| `queueName` | `string` | `-` | `ServerAPI/GetQueueList` | 메시지를 가져올 큐를 선택합니다.<br/>ex) "myQueue" |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 연결 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |

## Property Notes
### `messageFilter`
메시지 그룹의 필터 옵션을 지정합니다.
ex) "equal"

### `messageGroup`
필터링할 메시지 그룹 코드를 입력합니다.
ex) "myGroup"

### `queueName`
메시지를 가져올 큐를 선택합니다.
ex) "myQueue"

### `timeout`
연결 대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

