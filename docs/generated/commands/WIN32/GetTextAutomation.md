# Activity: GetTextAutomation

## Summary
윈도우 애플리케이션 오브젝트(컨트롤)에 이름이나 값의 텍스트 데이터를 가져오는 액티비티

## Metadata
- group: `WIN32`
- script: `MSAA.GetText()`
- pattern: `.*MSAA\.GetText\(`
- dependencies: `MSAA`
- theme: `Accent3_5`
- prefix: `text`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `item` | `string` | `"name"` | `"name"`, `"value"` | 텍스트를 추출할 객체의 속성을 선택한다.<br/>"name": 선택한 객체의 이름에 있는 텍스트를 추출한다.<br/>"value": 선택한 객체의 값에 있는 텍스트를 추출한다.<br/>(Picker 화면에 name과 value 항목이 나타나는데, 둘 항목 중에서 값이 표시되는 항목을 선택한다.) |
| `searchMode` | `string` | `"All"` | `"All"`, `"UIA"`, `"MSAA"` | 대상 윈도우 객체 검색 방식을 지정합니다.<br/>"All": UIA와 MSAA를 순차적으로 사용합니다.<br/>"UIA": UIA 방식으로만 검색합니다.<br/>"MSAA": MSAA 방식으로만 검색합니다. |
| `selector` | `string` | `-` | - | 대상 윈도우 객체를 선택합니다.<br/>ex) selector_0 |
| `timeout` | `string` | `30` | `5`, `10`, `20`, `30`, `40`, `50`, `60` | 윈도우 객체를 찾는 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |

## Property Notes
### `item`
텍스트를 추출할 객체의 속성을 선택한다.
"name": 선택한 객체의 이름에 있는 텍스트를 추출한다.
"value": 선택한 객체의 값에 있는 텍스트를 추출한다.
(Picker 화면에 name과 value 항목이 나타나는데, 둘 항목 중에서 값이 표시되는 항목을 선택한다.)

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

