# Activity: SetRegistryValue

## Summary
윈도우 레지스트리에 값을 설정하는 액티비티.  
반환값) Boolean

## Metadata
- group: `WIN32`
- script: `WIN32.set_registry_value()`
- pattern: `WIN32\.set_registry_value\(`
- dependencies: `WIN32`
- theme: `Accent3_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `rootKey` | `string` | `"HKEY_CURRENT_USER"` | `"HKEY_CLASSES_ROOT"`, `"HKEY_CURRENT_USER"`, `"HKEY_LOCAL_MACHINE"`, `"HKEY_USERS"`, `"HKEY_CURRENT_CONFIG"` | 루트 키 이름을 선택합니다.<br/>ex) "HKEY_CURRENT_USER |
| `subKey` | `string` | `-` | - | 루트 키 아래의 서브 키 경로를 입력합니다.<br/>ex) r"Software\BATEM\Selector" |
| `valueData` | `string` | `-` | - | 설정할 값을 입력합니다.문자열 또는 정수로 입력합니다.<br/>ex) 1 또는 "batem" |
| `valueName` | `string` | `-` | - | 설정할 값의 이름을 입력합니다.<br/>ex) "UseTimeOut" |
| `valueType` | `string` | `"REG_DWORD"` | `"REG_SZ"`, `"REG_DWORD"`, `"REG_QWORD"`, `"REG_MULTI_SZ"`, `"REG_EXPAND_SZ"` | 레지스트리의 값 타입을 선택합니다.<br/>"REG_SZ": 문자열 값<br/>"REG_DWORD": 32비트 DWORD 값<br/>"REG_QWORD": 64비트 QWORD 값<br/>"REG_MULTI_SZ": 다중 문자열 값<br/>"REG_EXPAND_SZ": 확장 가능 문자열 값 |

## Property Notes
### `rootKey`
루트 키 이름을 선택합니다.
ex) "HKEY_CURRENT_USER

### `subKey`
루트 키 아래의 서브 키 경로를 입력합니다.
ex) r"Software\BATEM\Selector"

### `valueData`
설정할 값을 입력합니다.문자열 또는 정수로 입력합니다.
ex) 1 또는 "batem"

### `valueName`
설정할 값의 이름을 입력합니다.
ex) "UseTimeOut"

### `valueType`
레지스트리의 값 타입을 선택합니다.
"REG_SZ": 문자열 값
"REG_DWORD": 32비트 DWORD 값
"REG_QWORD": 64비트 QWORD 값
"REG_MULTI_SZ": 다중 문자열 값
"REG_EXPAND_SZ": 확장 가능 문자열 값

