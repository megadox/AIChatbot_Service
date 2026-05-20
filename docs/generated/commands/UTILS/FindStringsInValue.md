# Activity: FindStringsInValue

## Summary
특정 문자열에서 숫자를 뺀 순수 문자열만 추출하는 액티비티

## Metadata
- group: `UTILS`
- script: `STRINGS.find_strings()`
- pattern: `STRINGS\.find_strings\(`
- dependencies: `STRINGS`
- theme: `Accent5_5`
- prefix: `text`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `join` | `string` | `False` | `True`, `False` | 문자열을 추출할 때 문자열의 결합/분리 여부를 지정합니다.<br/>True: 문자열을 추출할 때 분리된 문자열을 하나로 결합하여 저장한다.<br/>False: 문자열을 추출할 때 분리된 문자열을 리스트 자료형으로 저장한다. |
| `joiner` | `string` | `-` | - | Join필드가 True로 설정되었을 때 문자열 결합시 결합표시기호를 지정합니다.<br/>ex) "+" |
| `value` | `string` | `-` | - | 문자 또는 숫자를 포함하고 있는 문자열을 지정합니다.<br/>ex) "abcd 12xyz34 efgh56" |

## Property Notes
### `join`
문자열을 추출할 때 문자열의 결합/분리 여부를 지정합니다.
True: 문자열을 추출할 때 분리된 문자열을 하나로 결합하여 저장한다.
False: 문자열을 추출할 때 분리된 문자열을 리스트 자료형으로 저장한다.

### `joiner`
Join필드가 True로 설정되었을 때 문자열 결합시 결합표시기호를 지정합니다.
ex) "+"

### `value`
문자 또는 숫자를 포함하고 있는 문자열을 지정합니다.
ex) "abcd 12xyz34 efgh56"

