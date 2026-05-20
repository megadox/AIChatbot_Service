# Activity: SetQueueMessageProgress

## Summary
지정한 큐 메시지의 진행률을 설정하는 액티비티

## Metadata
- group: `SERVER`
- script: `SERVER.set_queue_progress()`
- pattern: `SERVER\.set_queue_progress\(`
- dependencies: `SERVER`
- theme: `Accent2`
- prefix: `result`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `messageId` | `string` | `-` | - | 큐 메시지의 Id를 입력합니다.<br/>ex) "4f8a-43-4h1-b9e-cbd37ba" |
| `subStatus` | `string` | `-` | - | 처리 중인 메시지의 세부 진행 상태를 문자열로 입력합니다.<br/>ex) "에러처리로 넘어감." |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 연결 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |

## Property Notes
### `messageId`
큐 메시지의 Id를 입력합니다.
ex) "4f8a-43-4h1-b9e-cbd37ba"

### `subStatus`
처리 중인 메시지의 세부 진행 상태를 문자열로 입력합니다.
ex) "에러처리로 넘어감."

### `timeout`
연결 대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

