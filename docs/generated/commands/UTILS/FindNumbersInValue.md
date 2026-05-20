# Activity: FindNumbersInValue

## Summary
특정 문자열에서 숫자만 추출하는 액티비티

## Metadata
- group: `UTILS`
- script: `STRINGS.find_numbers()`
- pattern: `STRINGS\.find_numbers\(`
- dependencies: `STRINGS`
- theme: `Accent5_5`
- prefix: `num`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `forceConvert` | `string` | `False` | `True`, `False` | 정수형으로 형 변환 여부를 지정합니다.<br/>True: 정수형 데이터로 형 변환한다.<br/>False: 정수형 데이터로 형 변환하지 않는다. |
| `join` | `string` | `False` | `True`, `False` | 숫자열을 추출할 때 숫자열의 결합/분리 여부를 지정합니다.<br/>True: 숫자열을 추출할 때 분리된 숫자열을 하나로 결합하여 저장한다.<br/>False: 숫자열을 추출할 때 분리된 숫자열을 리스트 자료형으로 저장한다. |
| `joiner` | `string` | `-` | - | Join필드가 True로 설정되었을 때 숫자열 결합시 결합표시기호를 지정합니다.<br/>ex) "+" |
| `value` | `string` | `-` | - | 문자 또는 숫자를 포함하고 있는 문자열을 지정합니다.<br/>ex) "abcd 12xyz34 efgh56" |

## Property Notes
### `forceConvert`
정수형으로 형 변환 여부를 지정합니다.
True: 정수형 데이터로 형 변환한다.
False: 정수형 데이터로 형 변환하지 않는다.

### `join`
숫자열을 추출할 때 숫자열의 결합/분리 여부를 지정합니다.
True: 숫자열을 추출할 때 분리된 숫자열을 하나로 결합하여 저장한다.
False: 숫자열을 추출할 때 분리된 숫자열을 리스트 자료형으로 저장한다.

### `joiner`
Join필드가 True로 설정되었을 때 숫자열 결합시 결합표시기호를 지정합니다.
ex) "+"

### `value`
문자 또는 숫자를 포함하고 있는 문자열을 지정합니다.
ex) "abcd 12xyz34 efgh56"

