# Activity: GetTableDataAutomation

## Summary
윈도우 애플리케이션 테이블 형태 오브젝트(컨트롤)의 데이터를 반환하는 액티비티

## Metadata
- group: `WIN32`
- script: `MSAA.get_table_data()`
- pattern: `.*MSAA\.get_table_data\(`
- dependencies: `MSAA`
- theme: `Accent3_5`
- prefix: `collection`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `header` | `string` | `False` | `True`, `False` | 테이블의 헤더를 포함할지 여부를 지정합니다.<br/>True: 헤더를 포함한다.<br/>False: 헤더를 포함하지 않는다. |
| `maxRow` | `string` | `100` | `10`, `20`, `30`, `40`, `50`, `60`, `70`, `80`, `90`, `100` | 반환할 최대 행의 수를 지정합니다.<br/>ex) 10 |
| `preview` | `string` | `-` | - | - |
| `searchMode` | `string` | `"All"` | `"All"`, `"UIA"`, `"MSAA"` | 대상 윈도우 객체 검색 방식을 지정합니다.<br/>"All": UIA와 MSAA를 순차적으로 사용합니다.<br/>"UIA": UIA 방식으로만 검색합니다.<br/>"MSAA": MSAA 방식으로만 검색합니다. |
| `selector` | `string` | `-` | - | 대상 윈도우 객체를 선택합니다.<br/>ex) selector_0 |
| `timeout` | `string` | `30` | `5`, `10`, `20`, `30`, `40`, `50`, `60` | 윈도우 객체를 찾는 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |

## Property Notes
### `header`
테이블의 헤더를 포함할지 여부를 지정합니다.
True: 헤더를 포함한다.
False: 헤더를 포함하지 않는다.

### `maxRow`
반환할 최대 행의 수를 지정합니다.
ex) 10

### `searchMode`
대상 윈도우 객체 검색 방식을 지정합니다.
"All": UIA와 MSAA를 순차적으로 사용합니다.
"UIA": UIA 방식으로만 검색합니다.
"MSAA": MSAA 방식으로만 검색합니다.

### `selector`
대상 윈도우 객체를 선택합니다.
ex) selector_0

### `timeout`
윈도우 객체를 찾는 최대 시간을 지정합니다.
ex) 30 (단위: 초)

