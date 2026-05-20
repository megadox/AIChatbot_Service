# Activity: CopyRange

## Summary
시트에서 특정 범위를 복사할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `CopyRange()`
- pattern: `excel\.CopyRange\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `range`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `range` | `string` | `-` | - | 복사할 영역을 지정합니다.<br/>ex) "E2:E3" |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |

## Property Notes
### `range`
복사할 영역을 지정합니다.
ex) "E2:E3"

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

