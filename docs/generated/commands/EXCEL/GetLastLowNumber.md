# Activity: GetLastLowNumber

## Summary
특정 열의 마지막 행의 값을 가져오는 액티비티

## Metadata
- group: `EXCEL`
- script: `get_last_low_number()`
- pattern: `excel\.get_last_low_number\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `num`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `keyColumn` | `string` | `-1` | - | 마지막 행의 값을 가져올 열을 A열을 기준으로 인덱스 방식으로 지정합니다.<br/>(-1의 경우 사용된 범위에서 제일 마지막 행의 번호를 반환한다.)<br/>ex) 1 |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |

## Property Notes
### `keyColumn`
마지막 행의 값을 가져올 열을 A열을 기준으로 인덱스 방식으로 지정합니다.
(-1의 경우 사용된 범위에서 제일 마지막 행의 번호를 반환한다.)
ex) 1

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

