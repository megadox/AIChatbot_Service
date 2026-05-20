# Activity: MaxInRange

## Summary
특정 범위의 최대값을 구하는 액티비티

## Metadata
- group: `EXCEL`
- script: `MaxInRange()`
- pattern: `excel\.MaxInRange\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `value`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `range` | `string` | `-` | - | 최대값을 계산할 영역을 지정합니다.<br/>ex) "A1:B1" |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |

## Property Notes
### `range`
최대값을 계산할 영역을 지정합니다.
ex) "A1:B1"

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

