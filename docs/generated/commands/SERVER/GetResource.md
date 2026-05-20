# Activity: GetResource

## Summary
BA서버 특정 이름의 리소스의 값을 가져오는 액티비티

## Metadata
- group: `SERVER`
- script: `SERVER.get_resource()`
- pattern: `SERVER\.get_resource\(`
- dependencies: `SERVER`
- theme: `Accent2`
- prefix: `resource`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `name` | `string` | `-` | `ServerAPI/GetResourceList` | 가져올 리소스 이름을 지정합니다.<br/>ex) resource1 |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 연결 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |

## Property Notes
### `name`
가져올 리소스 이름을 지정합니다.
ex) resource1

### `timeout`
연결 대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

