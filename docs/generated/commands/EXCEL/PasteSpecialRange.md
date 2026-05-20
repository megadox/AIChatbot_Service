# Activity: PasteSpecialRange

## Summary
특정 형식으로 특정 영역에 붙여넣기 하는 액티비티

## Metadata
- group: `EXCEL`
- script: `PasteSpecialRange()`
- pattern: `excel\.PasteSpecialRange\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `range`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `beginCell` | `string` | `-` | - | 붙여넣기할 시작 셀을 지정합니다.<br/>ex) "F1"<br/>(값이 지정되면 지정된 셀부터 시작하는 영역의 데이터를 리스트 자료형으로 (Name)필드의 값에 저장한다.) |
| `range` | `string` | `-` | - | 붙여넣기할 영역을 지정합니다.<br/>ex) "E2:E3" |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |
| `type` | `string` | `"PasteValues"` | `"PasteAll"`, `"PasteAllExceptBorders"`, `"PasteAllMergingConditionalFormats"`, `"PasteAllUsingSourceTheme"`, `"PasteColumnWidths"`, `"PasteComments"`, `"PasteFormats"`, `"PasteFormulas"`, `"PasteFormulasAndNumberFormats"`, `"PasteValidation"`, `"PasteValues"`, `"PasteValuesAndNumberFormats"` | 붙여넣기할 형식을 지정합니다.<br/>ex) "PasteAll" |

## Property Notes
### `beginCell`
붙여넣기할 시작 셀을 지정합니다.
ex) "F1"
(값이 지정되면 지정된 셀부터 시작하는 영역의 데이터를 리스트 자료형으로 (Name)필드의 값에 저장한다.)

### `range`
붙여넣기할 영역을 지정합니다.
ex) "E2:E3"

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

### `type`
붙여넣기할 형식을 지정합니다.
ex) "PasteAll"

