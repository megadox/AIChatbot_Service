# Activity: RemoveIf

## Summary
특정 조건에 해당하는 셀을 모두 찾아서 셀을 포함하는 행을 삭제하는 액티비티

## Metadata
- group: `EXCEL`
- script: `RemoveIf()`
- pattern: `excel\.RemoveIf\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `beginRow` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 검색을 시작할 행을 인덱스 방식으로 지정합니다.<br/>ex) 0 |
| `condition` | `string` | `-` | - | 찾을 데이터의 조건을 지정합니다.<br/>ex) "1\|사고건수"<br/>(열인덱스\|셀값 순으로 검색 조건을 입력한다.) |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |

## Property Notes
### `beginRow`
검색을 시작할 행을 인덱스 방식으로 지정합니다.
ex) 0

### `condition`
찾을 데이터의 조건을 지정합니다.
ex) "1|사고건수"
(열인덱스|셀값 순으로 검색 조건을 입력한다.)

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

