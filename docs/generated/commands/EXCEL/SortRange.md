# Activity: SortRange

## Summary
특정 범위의 값을 정렬시켜주는 액티비티

## Metadata
- group: `EXCEL`
- script: `SortRange()`
- pattern: `excel\.SortRange\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `header` | `string` | `True` | `True`, `False` | 헤더행의 포함 여부를 지정합니다.<br/>True: 헤더를 포함하지 않고 정렬한다.<br/>False: 헤더를 포함하여 정렬한다. |
| `keyRange` | `string` | `"A1"` | - | 정렬을 위한 기준 열의 임의의 셀의 값 또는 기준 영역을 지정합니다.<br/>ex) "E2" or "E2:E3" |
| `order` | `string` | `"asc"` | `"asc"`, `"desc"` | 정렬 방식을 지정합니다.<br/>"asc": 오름차순 정렬<br/>"desc": 내림차순 정렬 |
| `range` | `string` | `-` | - | 정렬할 영역을 지정합니다.<br/>ex) "E2:E3" |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |

## Property Notes
### `header`
헤더행의 포함 여부를 지정합니다.
True: 헤더를 포함하지 않고 정렬한다.
False: 헤더를 포함하여 정렬한다.

### `keyRange`
정렬을 위한 기준 열의 임의의 셀의 값 또는 기준 영역을 지정합니다.
ex) "E2" or "E2:E3"

### `order`
정렬 방식을 지정합니다.
"asc": 오름차순 정렬
"desc": 내림차순 정렬

### `range`
정렬할 영역을 지정합니다.
ex) "E2:E3"

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

