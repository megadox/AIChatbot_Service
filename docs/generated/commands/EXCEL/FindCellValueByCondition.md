# Activity: FindCellValueByCondition

## Summary
조건을 만족하는 셀 데이터를 검색한 후, 그 셀을 포함하는 행에서 특정 열의 값을 Name필드에 지정된 변수에 저장하는 액티비티 (인덱스 기준 : 데이터가 있는 열이 인덱스 0)

## Metadata
- group: `EXCEL`
- script: `FindCellValueByCondition()`
- pattern: `excel\.FindCellValueByCondition\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `value`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `beginRow` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 검색을 시작할 행을 인덱스 방식으로 지정합니다.<br/>ex) 0 |
| `column` | `string` | `-` | - | 검색할 열의 인덱스를 지정합니다.<br/>ex) 0<br/>(데이터가 있는 열부터 인덱스 0부터 시작한다.) |
| `condition` | `string` | `-` | - | 찾을 데이터의 조건을 지정합니다.<br/>ex) "1\|사고건수"<br/>(열인덱스\|셀값 순으로 검색 조건을 입력한다.) |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) 0<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) 또는 "Sheet 1" |

## Property Notes
### `beginRow`
검색을 시작할 행을 인덱스 방식으로 지정합니다.
ex) 0

### `column`
검색할 열의 인덱스를 지정합니다.
ex) 0
(데이터가 있는 열부터 인덱스 0부터 시작한다.)

### `condition`
찾을 데이터의 조건을 지정합니다.
ex) "1|사고건수"
(열인덱스|셀값 순으로 검색 조건을 입력한다.)

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) 0
(비어있는 경우 현재 활성화된 시트를 지정한다.) 또는 "Sheet 1"

