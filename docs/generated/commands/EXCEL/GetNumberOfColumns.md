# Activity: GetNumberOfColumns

## Summary
특정 행에서 사용하고 있는 열의 개수를 Name필드에 지정된 변수에 저장하는 액티비티

## Metadata
- group: `EXCEL`
- script: `GetNumberOfColumns()`
- pattern: `excel\.GetNumberOfColumns\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `count`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `row` | `string` | `` | - | 열의 개수를 계산할 대상 행의 값을 인덱스 방식으로 지정합니다.<br/>ex) 2 |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |

## Property Notes
### `row`
열의 개수를 계산할 대상 행의 값을 인덱스 방식으로 지정합니다.
ex) 2

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

