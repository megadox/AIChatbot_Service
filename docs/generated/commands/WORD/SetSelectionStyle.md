# Activity: SetSelectionStyle

## Summary
현재 지정된 범위의 스타일을 지정하는 액티비티

## Metadata
- group: `WORD`
- script: `set_selection_style()`
- pattern: `WORD\.set_selection_style\(`
- dependencies: `WORD`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `bold` | `string` | `False` | `True`, `False` | 폰트의 굵기를 지정합니다.<br/>True: Bold로 지정한다.<br/>False: Light로 지정한다. |
| `color` | `string` | `"RGB(0/0/0)"` | `"RGB(0/0/0)"`, `"BLACK"`, `"WHITE"`, `"RED"`, `"GREEN"`, `"BLUE"`, `"YELLOW"`, `"PINK"`, `"ORANGE"` | 폰트의 색을 지정합니다.<br/>ex) "RGB(0/0/0)"<br/>"(RGB(0/0/0)": RGB(빨강/초록/파랑)으로 값을 지정할 수 있다.) |
| `fontSize` | `string` | `10` | - | 폰트의 크기를 지정합니다.<br/>ex) 11 |

## Property Notes
### `bold`
폰트의 굵기를 지정합니다.
True: Bold로 지정한다.
False: Light로 지정한다.

### `color`
폰트의 색을 지정합니다.
ex) "RGB(0/0/0)"
"(RGB(0/0/0)": RGB(빨강/초록/파랑)으로 값을 지정할 수 있다.)

### `fontSize`
폰트의 크기를 지정합니다.
ex) 11

