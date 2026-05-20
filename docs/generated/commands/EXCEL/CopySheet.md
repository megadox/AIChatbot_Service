# Activity: CopySheet

## Summary
특정 시트를 복사하여 새로운 시트에 붙여넣기할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `CopySheet()`
- pattern: `excel\.CopySheet\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `dstSheetName` | `string` | `-` | - | 붙여넣기할 시트의 이름을 지정합니다.<br/>ex) "Sheet 2" |
| `idx` | `string` | `-1` | `-4`, `-3`, `-2`, `-1`, `0`, `1`, `2`, `3`, `4`, `5` | 붙여넣기할 시트의 위치를 인덱스 방식으로 지정합니다.<br/>ex) 1<br/>(-1인 경우, 마지막 시트 뒤에 붙여넣습니다.) |
| `srcSheetName` | `string` | `-` | - | 복사할 시트의 이름을 지정합니다.<br/>ex) "Sheet 1" |

## Property Notes
### `dstSheetName`
붙여넣기할 시트의 이름을 지정합니다.
ex) "Sheet 2"

### `idx`
붙여넣기할 시트의 위치를 인덱스 방식으로 지정합니다.
ex) 1
(-1인 경우, 마지막 시트 뒤에 붙여넣습니다.)

### `srcSheetName`
복사할 시트의 이름을 지정합니다.
ex) "Sheet 1"

