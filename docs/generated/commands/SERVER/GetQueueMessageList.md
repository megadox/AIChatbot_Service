# Activity: GetQueueMessageList

## Summary
필터를 사용하여 메시지 리스트 결과를 받아오는 액티비티

## Metadata
- group: `SERVER`
- script: `SERVER.get_queue_message_list()`
- pattern: `SERVER\.get_queue_message_list\(`
- dependencies: `SERVER`
- theme: `Accent2`
- prefix: `queue_message_list`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `count` | `string` | `10` | - | 결과의 앞 부분부터 가져올 메시지의 갯수를 입력합니다.<br/>ex) 10 |
| `messageFilter` | `string` | `"equal"` | `"equal"`, `"startswith"` | 메시지 그룹의 필터 옵션을 지정합니다.(messageGroup 사용 시 필수 값)<br/>ex) "equal" |
| `messageGroup` | `string` | `-` | - | 필터링할 메시지 그룹 코드를 입력합니다.<br/>ex) "myGroup" |
| `queueName` | `string` | `-` | `ServerAPI/GetQueueList` | 메시지를 가져올 큐를 선택합니다.<br/>ex) "myQueue" |
| `skipCount` | `string` | `0` | - | 결과의 맨 앞부터 스킵할 카운트 갯수를 입력합니다.<br/>ex) 3 |
| `status` | `string` | `"WAITING"` | `MultipleCheckBox/statusList` | 메시지의 상태를 지정합니다.<br/>ex) "SUCCESS" |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 연결 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |

## Property Notes
### `count`
결과의 앞 부분부터 가져올 메시지의 갯수를 입력합니다.
ex) 10

### `messageFilter`
메시지 그룹의 필터 옵션을 지정합니다.(messageGroup 사용 시 필수 값)
ex) "equal"

### `messageGroup`
필터링할 메시지 그룹 코드를 입력합니다.
ex) "myGroup"

### `queueName`
메시지를 가져올 큐를 선택합니다.
ex) "myQueue"

### `skipCount`
결과의 맨 앞부터 스킵할 카운트 갯수를 입력합니다.
ex) 3

### `status`
메시지의 상태를 지정합니다.
ex) "SUCCESS"

### `timeout`
연결 대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

