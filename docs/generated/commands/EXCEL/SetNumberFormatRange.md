# Activity: SetNumberFormatRange

## Summary
특정 영역의 셀의 표현 서식을 적용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `SetNumberFormatRange()`
- pattern: `excel\.SetNumberFormatRange\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `format` | `string` | `-` | `"G/표준"`, `"0.00"`, `"0.00%"`, `"mm/dd/yyyy"`, `"hh:mm:ss"`, `"0.00E+00"`, `"@"` | 셀의 서식을 지정합니다.<br/>ex) "#.000" (소수점이하 3자리로 표현) |
| `range` | `string` | `-` | - | 서식을 적용할 영역을 지정합니다.<br/>ex) "E2:E3" |

## Property Notes
### `format`
셀의 서식을 지정합니다.
ex) "#.000" (소수점이하 3자리로 표현)

### `range`
서식을 적용할 영역을 지정합니다.
ex) "E2:E3"

