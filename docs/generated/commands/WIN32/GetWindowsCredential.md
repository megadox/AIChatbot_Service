# Activity: GetWindowsCredential

## Summary
윈도우 자격증명 관리자에 저장된 사용자 계정 정보를 반환하는 액티비티 (없다면 None 반환)

## Metadata
- group: `WIN32`
- script: `ENCRYPTION.get_generic_credential()`
- pattern: `ENCRYPTION\.get_generic_credential\(`
- dependencies: `ENCRYPTION`
- theme: `Accent3_5`
- prefix: `secure`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `target` | `string` | `-` | - | 자격증명 관리자에서 가져올 대상 이름을 지정합니다.<br/>ex) "myCredential" |

## Property Notes
### `target`
자격증명 관리자에서 가져올 대상 이름을 지정합니다.
ex) "myCredential"

