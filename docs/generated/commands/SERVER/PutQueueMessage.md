# Activity: PutQueueMessage

## Summary
큐에 새로운 메시지 데이터를 추가하는 액티비티

## Metadata
- group: `SERVER`
- script: `SERVER.put_queue_message()`
- pattern: `SERVER\.put_queue_message\(`
- dependencies: `SERVER`
- theme: `Accent2`
- prefix: `result`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `individualRow` | `string` | `False` | `True`, `False` | 추가할 데이터를 row별로 등록할 지 여부를 선택합니다.<br/>ex) True<br/>(True인 경우 데이터 타입은 List 타입이어야 합니다.) |
| `messageGroup` | `string` | `-` | - | 필터링할 메시지 그룹 코드를 입력합니다.<br/>ex) "myGroup" |
| `payload` | `string` | `-` | - | 큐에 추가할 데이터 객체를 입력합니다.<br/>ex) new_queue<br/>(string or list) |
| `queueName` | `string` | `-` | `ServerAPI/GetQueueList` | 메시지를 가져올 큐를 선택합니다.<br/>ex) "myQueue" |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 연결 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |

## Property Notes
### `individualRow`
추가할 데이터를 row별로 등록할 지 여부를 선택합니다.
ex) True
(True인 경우 데이터 타입은 List 타입이어야 합니다.)

### `messageGroup`
필터링할 메시지 그룹 코드를 입력합니다.
ex) "myGroup"

### `payload`
큐에 추가할 데이터 객체를 입력합니다.
ex) new_queue
(string or list)

### `queueName`
메시지를 가져올 큐를 선택합니다.
ex) "myQueue"

### `timeout`
연결 대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

