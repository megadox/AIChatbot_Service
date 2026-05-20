# Activity: GetQueueMessage

## Summary
지정한 큐의 가장 오래된 메시지를 받아오는 액티비티

## Metadata
- group: `SERVER`
- script: `SERVER.get_queue_message()`
- pattern: `SERVER\.get_queue_message\(`
- dependencies: `SERVER`
- theme: `Accent2`
- prefix: `queue_message`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `messageFilter` | `string` | `"equal"` | `"equal"`, `"startswith"` | 메시지 그룹의 필터 옵션을 지정합니다.(messageGroup 사용 시 필수 값)<br/>ex) "equal" |
| `messageGroup` | `string` | `-` | - | 메시지 그룹 코드를 입력합니다<br/>ex) "myGroup" |
| `messageId` | `string` | `-` | - | 큐 메시지의 Id를 입력합니다.<br/>ex) "4f8a-43-4e1-b9e-cbc37ba" |
| `queueName` | `string` | `-` | `ServerAPI/GetQueueList` | 메시지를 가져올 큐를 선택합니다.<br/>ex) "myQueue" |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 연결 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |

## Property Notes
### `messageFilter`
메시지 그룹의 필터 옵션을 지정합니다.(messageGroup 사용 시 필수 값)
ex) "equal"

### `messageGroup`
메시지 그룹 코드를 입력합니다
ex) "myGroup"

### `messageId`
큐 메시지의 Id를 입력합니다.
ex) "4f8a-43-4e1-b9e-cbc37ba"

### `queueName`
메시지를 가져올 큐를 선택합니다.
ex) "myQueue"

### `timeout`
연결 대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

