# Activity: CopyAndPasteWorksheetRange

## Summary
지정한 워크시트 또는 범위의 값을 복사 붙여넣기할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `CopyAndPasteWorksheetRange()`
- pattern: `excel\.CopyAndPasteWorksheetRange\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `fromRange` | `string` | `-` | - | 복사할 영역을 지정합니다.<br/>ex) "E2:E3" |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |
| `toRange` | `string` | `-` | - | 붙여넣기할 영역을 지정합니다.<br/>ex) "F2:F3" |

## Property Notes
### `fromRange`
복사할 영역을 지정합니다.
ex) "E2:E3"

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

### `toRange`
붙여넣기할 영역을 지정합니다.
ex) "F2:F3"

