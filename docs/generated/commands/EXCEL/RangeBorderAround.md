# Activity: RangeBorderAround

## Summary
특정 영역의 테두리 선의 스타일을 지정하는 액티비티

## Metadata
- group: `EXCEL`
- script: `RangeBorderAround()`
- pattern: `excel\.RangeBorderAround\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `range`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `color` | `string` | `"RGB(0/0/0)"` | `"RGB(0/0/0)"`, `"BLACK"`, `"WHITE"`, `"RED"`, `"GREEN"`, `"BLUE"`, `"YELLOW"`, `"PINK"`, `"ORANGE"` | 테두리 선의 색을 지정합니다.<br/>ex) "BLACK"<br/>"(RGB(0/0/0)": RGB(빨강/초록/파랑)으로 값을 지정할 수 있다.) |
| `innerColor` | `string` | `"RGB(0/0/0)"` | `"RGB(0/0/0)"`, `"BLACK"`, `"WHITE"`, `"RED"`, `"GREEN"`, `"BLUE"`, `"YELLOW"`, `"PINK"`, `"ORANGE"` | 내부 테두리 선의 색을 지정합니다.<br/>ex) "BLACK"<br/>"(RGB(0/0/0)": RGB(빨강/초록/파랑)으로 값을 지정할 수 있다.) |
| `innerLineStyle` | `string` | `"NO_LINE"` | `"CONTINUOUS"`, `"DASH"`, `"DASHDOT"`, `"DASHDOTDOT"`, `"DOT"`, `"DOULBE"`, `"NO_LINE"` | 내부 테두리 선의 스타일을 지정합니다.<br/>ex) "NO_LINE" |
| `innerWeight` | `string` | `"THIN"` | `"HAIRLINE"`, `"MEDIUM"`, `"THICK"`, `"THIN"` | 내부 테두리 선의 두께를 지정합니다.<br/>ex) "THIN" |
| `lineStyle` | `string` | `"CONTINUOUS"` | `"CONTINUOUS"`, `"DASH"`, `"DASHDOT"`, `"DASHDOTDOT"`, `"DOT"`, `"DOULBE"`, `"NO_LINE"` | 테두리 선의 스타일을 지정합니다.<br/>ex) "CONTINUOUS" |
| `range` | `string` | `-` | - | 테두리 스타일을 지정할 영역을 지정합니다.<br/>ex) "E2:E3" |
| `weight` | `string` | `"THIN"` | `"HAIRLINE"`, `"MEDIUM"`, `"THICK"`, `"THIN"` | 테두리 선의 두께를 지정합니다.<br/>ex) "THICK" |

## Property Notes
### `color`
테두리 선의 색을 지정합니다.
ex) "BLACK"
"(RGB(0/0/0)": RGB(빨강/초록/파랑)으로 값을 지정할 수 있다.)

### `innerColor`
내부 테두리 선의 색을 지정합니다.
ex) "BLACK"
"(RGB(0/0/0)": RGB(빨강/초록/파랑)으로 값을 지정할 수 있다.)

### `innerLineStyle`
내부 테두리 선의 스타일을 지정합니다.
ex) "NO_LINE"

### `innerWeight`
내부 테두리 선의 두께를 지정합니다.
ex) "THIN"

### `lineStyle`
테두리 선의 스타일을 지정합니다.
ex) "CONTINUOUS"

### `range`
테두리 스타일을 지정할 영역을 지정합니다.
ex) "E2:E3"

### `weight`
테두리 선의 두께를 지정합니다.
ex) "THICK"

