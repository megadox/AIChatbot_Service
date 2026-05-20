# Activity: TransactQueueMessage

## Summary
지정한 큐 메시지의 처리 상태를 변경하는 액티비티

## Metadata
- group: `SERVER`
- script: `SERVER.transact_queue_message()`
- pattern: `SERVER\.transact_queue_message\(`
- dependencies: `SERVER`
- theme: `Accent2`
- prefix: `result`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `comment` | `string` | `-` | - | 지정한 상태에 대한 설명을 입력합니다.<br/>ex) "에러 상태 변경" |
| `errorType` | `string` | `"System"` | `"System"`, `"Business"` | 에러의 타입을 지정합니다.<br/>ex) "System" |
| `messageId` | `string` | `-` | - | 큐 메시지의 Id를 입력합니다.<br/>ex) "4f8a-43-4e1-b9e-cbc37ba" |
| `status` | `string` | `"SUCCESS"` | `"SUCCESS"`, `"FAILURE"` | 큐 메시지의 상태를 지정합니다.<br/>ex) "FAILURE" |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 연결 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |

## Property Notes
### `comment`
지정한 상태에 대한 설명을 입력합니다.
ex) "에러 상태 변경"

### `errorType`
에러의 타입을 지정합니다.
ex) "System"

### `messageId`
큐 메시지의 Id를 입력합니다.
ex) "4f8a-43-4e1-b9e-cbc37ba"

### `status`
큐 메시지의 상태를 지정합니다.
ex) "FAILURE"

### `timeout`
연결 대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

