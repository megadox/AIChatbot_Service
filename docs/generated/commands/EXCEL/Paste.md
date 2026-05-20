# Activity: Paste

## Summary
특정 셀에 붙여넣기 하는 액티비티

## Metadata
- group: `EXCEL`
- script: `Paste()`
- pattern: `excel\.Paste\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `cell` | `string` | `-` | - | 붙여넣기할 셀을 지정합니다.<br/>ex) "A10" |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |

## Property Notes
### `cell`
붙여넣기할 셀을 지정합니다.
ex) "A10"

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

