# Activity: GetTableData

## Summary
특정 웹 테이블 엘리먼트를 리스트로 반환하는 액티비티

## Metadata
- group: `WEB`
- script: `table_to_list()`
- pattern: `browser.*\.table_to_list\(`
- dependencies: `WEB`
- theme: `Accent1`
- prefix: `collection`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `header` | `string` | `False` | `True`, `False` | 테이블의 헤더를 포함할지 여부를 지정합니다.<br/>True: 헤더를 포함한다.<br/>False: 헤더를 포함하지 않는다. |
| `maxRow` | `string` | `100` | `10`, `20`, `30`, `40`, `50`, `60`, `70`, `80`, `90`, `100` | 반환할 최대 행의 수를 지정합니다.<br/>ex) 10 |
| `preview` | `string` | `-` | - | - |
| `selector` | `string` | `-` | - | 대상 웹 객체를 선택합니다.<br/>ex) selector_1 |
| `waitParams` | `string` | `True` | `True`, `False` | params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.<br/>True: 웹 엘리먼트의 로딩을 대기한다.<br/>False: 웹 엘리먼트의 로딩을 대기하지 않는다. |

## Property Notes
### `header`
테이블의 헤더를 포함할지 여부를 지정합니다.
True: 헤더를 포함한다.
False: 헤더를 포함하지 않는다.

### `maxRow`
반환할 최대 행의 수를 지정합니다.
ex) 10

### `selector`
대상 웹 객체를 선택합니다.
ex) selector_1

### `waitParams`
params에 지정된 웹 엘리먼트의 로딩 대기 여부를 지정합니다.
True: 웹 엘리먼트의 로딩을 대기한다.
False: 웹 엘리먼트의 로딩을 대기하지 않는다.

