# Activity: SetTextAutomation

## Summary
윈도우 애플리케이션 오브젝트(컨트롤)에 텍스트 입력을 수행하는 액티비티

## Metadata
- group: `WIN32`
- script: `MSAA.SetText()`
- pattern: `MSAA\.SetText\(`
- dependencies: `MSAA`
- theme: `Accent3_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `byClipboard` | `string` | `False` | `True`, `False` | 클립보드를 이용한 텍스트 입력 여부를 지정합니다.<br/>True: 클립보드를 이용하여 텍스트를 입력한다.<br/>False: 클립보드를 이용하지 않고 텍스트를 입력한다. |
| `clear` | `string` | `True` | `True`, `False` | 텍스트 입력 전에 기존 텍스트를 지우는 여부를 지정합니다.<br/>True: 기존 텍스트를 지운다.<br/>False: 기존 텍스트를 지우지 않는다. |
| `searchMode` | `string` | `"All"` | `"All"`, `"UIA"`, `"MSAA"` | 대상 윈도우 객체 검색 방식을 지정합니다.<br/>"All": UIA와 MSAA를 순차적으로 사용합니다.<br/>"UIA": UIA 방식으로만 검색합니다.<br/>"MSAA": MSAA 방식으로만 검색합니다. |
| `selector` | `string` | `-` | - | 대상 윈도우 객체를 선택합니다.<br/>ex) selector_0 |
| `text` | `string` | `-` | - | 입력할 텍스트를 문자열로 지정합니다.<br/>ex) "hello world" |
| `timeout` | `string` | `30` | `5`, `10`, `20`, `30`, `40`, `50`, `60` | 윈도우 객체를 찾는 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |

## Property Notes
### `byClipboard`
클립보드를 이용한 텍스트 입력 여부를 지정합니다.
True: 클립보드를 이용하여 텍스트를 입력한다.
False: 클립보드를 이용하지 않고 텍스트를 입력한다.

### `clear`
텍스트 입력 전에 기존 텍스트를 지우는 여부를 지정합니다.
True: 기존 텍스트를 지운다.
False: 기존 텍스트를 지우지 않는다.

### `searchMode`
대상 윈도우 객체 검색 방식을 지정합니다.
"All": UIA와 MSAA를 순차적으로 사용합니다.
"UIA": UIA 방식으로만 검색합니다.
"MSAA": MSAA 방식으로만 검색합니다.

### `selector`
대상 윈도우 객체를 선택합니다.
ex) selector_0

### `text`
입력할 텍스트를 문자열로 지정합니다.
ex) "hello world"

### `timeout`
윈도우 객체를 찾는 최대 시간을 지정합니다.
ex) 30 (단위: 초)

