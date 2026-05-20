# Activity: GetRegistryValue

## Summary
윈도우 레지스트리에 값을 가져오는 액티비티. 
반환값) 문자열 또는 정수

## Metadata
- group: `WIN32`
- script: `WIN32.get_registry_value()`
- pattern: `WIN32\.get_registry_value\(`
- dependencies: `WIN32`
- theme: `Accent3_5`
- prefix: `value`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `rootKey` | `string` | `"HKEY_CURRENT_USER"` | `"HKEY_CLASSES_ROOT"`, `"HKEY_CURRENT_USER"`, `"HKEY_LOCAL_MACHINE"`, `"HKEY_USERS"`, `"HKEY_CURRENT_CONFIG"` | 루트 키 이름을 선택합니다.<br/>ex) "HKEY_CURRENT_USER" |
| `subKey` | `string` | `-` | - | 루트 키 아래의 서브 키 경로를 입력합니다.<br/>ex) r"Software\BATEM\Selector" |
| `valueName` | `string` | `-` | - | 가져올 값의 이름을 입력합니다.<br/>ex) "UseTimeOut" |

## Property Notes
### `rootKey`
루트 키 이름을 선택합니다.
ex) "HKEY_CURRENT_USER"

### `subKey`
루트 키 아래의 서브 키 경로를 입력합니다.
ex) r"Software\BATEM\Selector"

### `valueName`
가져올 값의 이름을 입력합니다.
ex) "UseTimeOut"

