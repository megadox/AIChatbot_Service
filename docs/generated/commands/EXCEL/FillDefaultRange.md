# Activity: FillDefaultRange

## Summary
특정 열에서 셀 값이 비어 있으면 value필드의 설정값을 입력할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `FillDefaultRange()`
- pattern: `excel\.FillDefaultRange\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `keyColumn` | `string` | `` | - | value값을 채워 넣을 때 행의 수를 계산하는 기준열을 설정한다<br/>ex) 0 |
| `range` | `string` | `-` | - | 빈 셀에 채워 넣기할 영역을 지정합니다.<br/>ex) "E2:E3" |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |
| `value` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 빈 셀에 채워 넣을 값을 지정합니다.<br/>ex) 2023 또는 "ABCD" |

## Property Notes
### `keyColumn`
value값을 채워 넣을 때 행의 수를 계산하는 기준열을 설정한다
ex) 0

### `range`
빈 셀에 채워 넣기할 영역을 지정합니다.
ex) "E2:E3"

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

### `value`
빈 셀에 채워 넣을 값을 지정합니다.
ex) 2023 또는 "ABCD"

