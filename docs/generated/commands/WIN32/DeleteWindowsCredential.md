# Activity: DeleteWindowsCredential

## Summary
윈도우 자격증명 관리자에 저장된 사용자 계정 정보를 삭제하는 액티비티

## Metadata
- group: `WIN32`
- script: `ENCRYPTION.delete_generic_credential()`
- pattern: `ENCRYPTION\.delete_generic_credential\(`
- dependencies: `ENCRYPTION`
- theme: `Accent3_5`
- prefix: `result`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `target` | `string` | `-` | - | 자격증명 관리자에서 삭제할 대상 이름을 지정합니다.<br/>ex) "myCredential" |

## Property Notes
### `target`
자격증명 관리자에서 삭제할 대상 이름을 지정합니다.
ex) "myCredential"

