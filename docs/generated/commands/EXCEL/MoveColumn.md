# Activity: MoveColumn

## Summary
지정한 열을 이동하는 액티비티

## Metadata
- group: `EXCEL`
- script: `MoveColumn()`
- pattern: `excel\.MoveColumn\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `fromColumn` | `string` | `-` | - | 이동할 열의 이동 전 열을 인덱스 방식으로 지정합니다.<br/>ex) 0 |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |
| `toColumn` | `string` | `-` | - | 이동할 열의 이동 후 열을 인덱스 방식으로 지정합니다.<br/>ex) 2 |

## Property Notes
### `fromColumn`
이동할 열의 이동 전 열을 인덱스 방식으로 지정합니다.
ex) 0

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

### `toColumn`
이동할 열의 이동 후 열을 인덱스 방식으로 지정합니다.
ex) 2

