# Activity: SSHCommand

## Summary
SSH 서버로 명령을 보내는 액티비티

## Metadata
- group: `WEB`
- script: `WEB.ssh_command()`
- pattern: `WEB\.ssh_command\(`
- dependencies: `WEB`
- theme: `Accent1`
- prefix: `result`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `command` | `string` | `-` | - | SSH 서버로 보낼 명령을 지정합니다.<br/>ex) "ls-al" |
| `host` | `string` | `-` | - | SSH 서버의 주소를 지정합니다.<br/>ex)123.45.6.7 |
| `password` | `string` | `-` | - | SSH 서버의 비밀번호를 지정합니다.<br/>ex) "1234" |
| `port` | `string` | `22` | - | SSH 서버의 포트 번호를 지정합니다.<br/>ex) 22 |
| `timeout` | `string` | `30` | `15`, `30`, `60`, `120` | 대기할 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |
| `user` | `string` | `-` | - | SSH 서버의 사용자 이름을 지정합니다.<br/>ex) "root" |

## Property Notes
### `command`
SSH 서버로 보낼 명령을 지정합니다.
ex) "ls-al"

### `host`
SSH 서버의 주소를 지정합니다.
ex)123.45.6.7

### `password`
SSH 서버의 비밀번호를 지정합니다.
ex) "1234"

### `port`
SSH 서버의 포트 번호를 지정합니다.
ex) 22

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30 (단위: 초)

### `user`
SSH 서버의 사용자 이름을 지정합니다.
ex) "root"

