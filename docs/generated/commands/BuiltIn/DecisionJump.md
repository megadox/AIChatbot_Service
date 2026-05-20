# Activity: DecisionJump

## Summary
조건의 결과(True/False)에 따라 다른 액티비티로 Jump하는 액티비티

## Metadata
- group: `BuiltIn`
- script: `DecisionJump()`
- pattern: `^DecisionJump\(`
- dependencies: `__BuiltIn__`
- symbol: `decision_jump`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `False` | `string` | `-` | - | 조건이 거짓(False)일 경우 Jump할 액티비티명을 지정합니다.<br/>ex) "message_1" |
| `True` | `string` | `-` | - | 조건이 참(True)일 경우 Jump할 액티비티명을 지정합니다.<br/>ex) "message_0" |

## Property Notes
### `False`
조건이 거짓(False)일 경우 Jump할 액티비티명을 지정합니다.
ex) "message_1"

### `True`
조건이 참(True)일 경우 Jump할 액티비티명을 지정합니다.
ex) "message_0"

