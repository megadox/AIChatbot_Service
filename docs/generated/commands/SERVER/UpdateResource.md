# Activity: UpdateResource

## Summary
BA서버 특정 이름의 리소스의 값을 변경하는 액티비티

## Metadata
- group: `SERVER`
- script: `SERVER.update_resource()`
- pattern: `SERVER\.update_resource\(`
- dependencies: `SERVER`
- theme: `Accent2`
- prefix: `result`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `name` | `string` | `-` | `ServerAPI/GetResourceList/GetResourceType` | 설정할 리소스 이름을 지정합니다.<br/>ex) resource1 |
| `secureId` | `string` | `-` | - | type이 secure일 경우 자격 증명 ID를 지정합니다.<br/>ex) "user" |
| `securePw` | `string` | `-` | - | type이 secure일 경우 자격 증명 PW를 지정합니다.<br/>ex) "1234" |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 연결 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |
| `type` | `string` | `"string"` | `ServerAPI/DisableEdit` | 설정할 리소스 타입을 지정합니다.<br/>string: 문자열<br/>bool: 불린<br/>int: 정수<br/>secure: 자격 증명 |
| `value` | `string` | `-` | - | 설정할 리소스 값을 지정합니다.<br/>ex) True |

## Property Notes
### `name`
설정할 리소스 이름을 지정합니다.
ex) resource1

### `secureId`
type이 secure일 경우 자격 증명 ID를 지정합니다.
ex) "user"

### `securePw`
type이 secure일 경우 자격 증명 PW를 지정합니다.
ex) "1234"

### `timeout`
연결 대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

### `type`
설정할 리소스 타입을 지정합니다.
string: 문자열
bool: 불린
int: 정수
secure: 자격 증명

### `value`
설정할 리소스 값을 지정합니다.
ex) True

