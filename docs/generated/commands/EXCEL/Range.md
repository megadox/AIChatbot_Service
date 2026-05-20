# Activity: Range

## Summary
특정 Range오브젝트를 Name필드에 지정된 변수에 저장하는 액티비티

## Metadata
- group: `EXCEL`
- script: `Range()`
- pattern: `excel\.Range\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `range`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `range` | `string` | `-` | - | 오브젝트로 가져올 영역을 지정합니다.<br/>ex) "E2:E3" |
| `select` | `string` | `True` | `True`, `False` | 오브젝트로 가져올 영역의 선택 여부를 지정합니다.<br/>True: 범위를 선택한다.<br/>False: 범위를 선택하지 않는다. |

## Property Notes
### `range`
오브젝트로 가져올 영역을 지정합니다.
ex) "E2:E3"

### `select`
오브젝트로 가져올 영역의 선택 여부를 지정합니다.
True: 범위를 선택한다.
False: 범위를 선택하지 않는다.

