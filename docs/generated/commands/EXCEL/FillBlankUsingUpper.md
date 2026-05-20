# Activity: FillBlankUsingUpper

## Summary
특정 열에서 셀 값이 비어 있으면 위에 채워진 셀 값으로 복사할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `FillBlankUsingUpper()`
- pattern: `excel\.FillBlankUsingUpper\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `count`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `beginRow` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 채워 넣기를 시작할 행을 인덱스 방식으로 지정합니다.<br/>ex) 0 |
| `column` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 빈 셀을 채워 넣을 열의 인덱스를 지정합니다.<br/>ex) 0 |
| `limit` | `string` | `-` | - | 최대 수행 건수를 지정합니다.<br/>ex) 10 |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |
| `until` | `string` | `-` | - | 빈 셀을 채워 넣기 적용의 한계를 지정합니다.<br/>ex) "0 \|울산"<br/>(열인덱스\|셀값 순으로 입력하며 조건 전까지만 채워넣기가 수행된다.) |

## Property Notes
### `beginRow`
채워 넣기를 시작할 행을 인덱스 방식으로 지정합니다.
ex) 0

### `column`
빈 셀을 채워 넣을 열의 인덱스를 지정합니다.
ex) 0

### `limit`
최대 수행 건수를 지정합니다.
ex) 10

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

### `until`
빈 셀을 채워 넣기 적용의 한계를 지정합니다.
ex) "0 |울산"
(열인덱스|셀값 순으로 입력하며 조건 전까지만 채워넣기가 수행된다.)

