# Activity: ReQueueMessage

## Summary
지정한 큐 메시지를 대기열에 재등록하는 액티비티

## Metadata
- group: `SERVER`
- script: `SERVER.re_register_queue_message()`
- pattern: `SERVER\.re_register_queue_message\(`
- dependencies: `SERVER`
- theme: `Accent2`
- prefix: `result`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `messageId` | `string` | `-` | - | 큐 메시지의 Id를 입력합니다.<br/>ex) "4f8a-43-4e1-b9e-cbc37ba" |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 연결 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |

## Property Notes
### `messageId`
큐 메시지의 Id를 입력합니다.
ex) "4f8a-43-4e1-b9e-cbc37ba"

### `timeout`
연결 대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

