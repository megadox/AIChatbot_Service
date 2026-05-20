# Activity: SetRangeFormat

## Summary
특정 영역의 셀 스타일을 지정하는 액티비티

## Metadata
- group: `EXCEL`
- script: `SetRangeFormat()`
- pattern: `excel\.SetRangeFormat\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `backColor` | `string` | `"RGB(0/0/0)"` | ``, `"RGB(0/0/0)"`, `"BLACK"`, `"WHITE"`, `"RED"`, `"GREEN"`, `"BLUE"`, `"YELLOW"`, `"PINK"`, `"ORANGE"` | 셀의 배경색을 지정합니다.<br/>ex) "YELLOW"<br/>"(RGB(0/0/0)": RGB(빨강/초록/파랑)으로 값을 지정할 수 있다.) |
| `bold` | `string` | `False` | `True`, `False` | 폰트의 굵기를 지정합니다.<br/>True: Bold로 지정한다.<br/>False: Light로 지정한다. |
| `fontColor` | `string` | `"RGB(0/0/0)"` | ``, `"RGB(0/0/0)"`, `"BLACK"`, `"WHITE"`, `"RED"`, `"GREEN"`, `"BLUE"`, `"YELLOW"`, `"PINK"`, `"ORANGE"` | 폰트의 색을 지정합니다.<br/>ex) "RED"<br/>"(RGB(0/0/0)": RGB(빨강/초록/파랑)으로 값을 지정할 수 있다.) |
| `fontSize` | `string` | `11` | - | 폰트의 크기를 지정합니다.<br/>ex) 11 |
| `merge` | `string` | `False` | `True`, `False` | 셀 병합 여부를 지정합니다.<br/>True: 병합한다.<br/>False: 병합하지 않는다. |
| `mergeAlign` | `string` | `"CENTER"` | `"CENTER"`, `"LEFT"`, `"RIGHT"` | 셀 병합시 정렬 방식을 지정합니다.<br/>"CENTER": 가운데 정렬<br/>"LEFT": 왼쪽 정렬<br/>"RIGHT": 오른쪽 정렬 |
| `range` | `string` | `-` | - | 서식을 적용할 영역을 지정합니다.<br/>ex) "E2:E3" |

## Property Notes
### `backColor`
셀의 배경색을 지정합니다.
ex) "YELLOW"
"(RGB(0/0/0)": RGB(빨강/초록/파랑)으로 값을 지정할 수 있다.)

### `bold`
폰트의 굵기를 지정합니다.
True: Bold로 지정한다.
False: Light로 지정한다.

### `fontColor`
폰트의 색을 지정합니다.
ex) "RED"
"(RGB(0/0/0)": RGB(빨강/초록/파랑)으로 값을 지정할 수 있다.)

### `fontSize`
폰트의 크기를 지정합니다.
ex) 11

### `merge`
셀 병합 여부를 지정합니다.
True: 병합한다.
False: 병합하지 않는다.

### `mergeAlign`
셀 병합시 정렬 방식을 지정합니다.
"CENTER": 가운데 정렬
"LEFT": 왼쪽 정렬
"RIGHT": 오른쪽 정렬

### `range`
서식을 적용할 영역을 지정합니다.
ex) "E2:E3"

