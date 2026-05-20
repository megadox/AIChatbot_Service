# Activity: SetWindowsCredential

## Summary
윈도우 자격증명 관리자에 사용자 계정 정보를 저장 또는 수정하는 액티비티

## Metadata
- group: `WIN32`
- script: `ENCRYPTION.set_generic_credential()`
- pattern: `ENCRYPTION\.set_generic_credential\(`
- dependencies: `ENCRYPTION`
- theme: `Accent3_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `password` | `string` | `-` | - | 저장할 사용자 계정의 비밀번호를 지정합니다.<br/>ex) "password123" |
| `persist` | `string` | `"LOCAL_MACHINE"` | `"SESSION"`, `"LOCAL_MACHINE"`, `"ENTERPRISE"` | 자격증명 정보를 저장할 지속성을 지정합니다.<br/>"SESSION": 현재 로그인 세션에서만 유지<br/>"LOCAL_MACHINE": 해당 컴퓨터의 모든 사용자에게 유지<br/>"ENTERPRISE": 도메인에 가입된 모든 컴퓨터에서 유지 |
| `target` | `string` | `-` | - | 자격증명 관리자에 저장할 대상 이름을 지정합니다.<br/>ex) "myCredential" |
| `username` | `string` | `-` | - | 저장할 사용자 계정 이름을 지정합니다.<br/>ex) "admin" |

## Property Notes
### `password`
저장할 사용자 계정의 비밀번호를 지정합니다.
ex) "password123"

### `persist`
자격증명 정보를 저장할 지속성을 지정합니다.
"SESSION": 현재 로그인 세션에서만 유지
"LOCAL_MACHINE": 해당 컴퓨터의 모든 사용자에게 유지
"ENTERPRISE": 도메인에 가입된 모든 컴퓨터에서 유지

### `target`
자격증명 관리자에 저장할 대상 이름을 지정합니다.
ex) "myCredential"

### `username`
저장할 사용자 계정 이름을 지정합니다.
ex) "admin"

