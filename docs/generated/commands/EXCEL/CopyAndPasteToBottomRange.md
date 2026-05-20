# Activity: CopyAndPasteToBottomRange

## Summary
지정한 셀 또는 범위의 값을 복사하여 아래 방향으로 keyColumn 의 마지막 행까지 적용할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `CopyAndPasteToBottomRange()`
- pattern: `excel\.CopyAndPasteToBottomRange\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `keyColumn` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 기준열을 인덱스 방식으로 지정합니다.<br/>ex) 1<br/>(기준열의 마지막 행까지 복사한다.) |
| `range` | `string` | `-` | - | 복사할 셀 또는 영역을 지정합니다.<br/>ex) E2:E3<br/>ex) "E2" |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) 0<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) 또는 "Sheet 1" |

## Property Notes
### `keyColumn`
기준열을 인덱스 방식으로 지정합니다.
ex) 1
(기준열의 마지막 행까지 복사한다.)

### `range`
복사할 셀 또는 영역을 지정합니다.
ex) E2:E3
ex) "E2"

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) 0
(비어있는 경우 현재 활성화된 시트를 지정한다.) 또는 "Sheet 1"

