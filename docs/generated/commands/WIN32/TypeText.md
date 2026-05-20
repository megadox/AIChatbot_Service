# Activity: TypeText

## Summary
현재 활성화된 윈도우 애플리케이션 오브젝트(컨트롤)에 text필드에 입력한 텍스트 데이터를 입력하는 액티비티

## Metadata
- group: `WIN32`
- script: `WIN32.TypeWrite()`
- pattern: `WIN32\.TypeWrite\(`
- dependencies: `WIN32`
- theme: `Accent3_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `byClipboard` | `string` | `True` | `True`, `False` | 클립보드 사용 여부를 지정합니다.<br/>True: 클립보드 복사 방식을 사용한다.<br/>False: 키보드 입력 방식을 사용한다.<br/>(한글의 경우 클립보드의 방식을 사용해야 한다.) |
| `clear` | `string` | `False` | `True`, `False` | 기존에 입력된 값의 제거 여부를 지정합니다.<br/>True: 기존에 입력된 값을 삭제 후 새로운 값 입력<br/>False: 기존 값을 지우지 않고 새로운 값 추가 |
| `searchMode` | `string` | `"All"` | `"All"`, `"UIA"`, `"MSAA"` | 대상 윈도우 객체 검색 방식을 지정합니다.<br/>"All": UIA와 MSAA를 순차적으로 사용합니다.<br/>"UIA": UIA 방식으로만 검색합니다.<br/>"MSAA": MSAA 방식으로만 검색합니다. |
| `selector` | `string` | `-` | - | 대상 윈도우 객체를 선택합니다.<br/>(값이 비어 있으면 현재 활성화된 컨트롤에 text 입력한다.)<br/>ex) selector_0 |
| `text` | `string` | `` | - | 입력할 텍스트를 지정합니다.<br/>ex) "hello world" |
| `timeout` | `string` | `30` | `5`, `10`, `20`, `30`, `40`, `50`, `60` | 윈도우 객체를 찾는 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |
| `verify` | `string` | `False` | `True`, `False` | 입력 값 검증 여부를 지정합니다.<br/>True: 입력이 완료된 후 입력된 값을 다시 읽어와서 text 필드의 값과 일치하는지 검증한다.<br/>False: 입력된 값을 검증하지 않는다. |

## Property Notes
### `byClipboard`
클립보드 사용 여부를 지정합니다.
True: 클립보드 복사 방식을 사용한다.
False: 키보드 입력 방식을 사용한다.
(한글의 경우 클립보드의 방식을 사용해야 한다.)

### `clear`
기존에 입력된 값의 제거 여부를 지정합니다.
True: 기존에 입력된 값을 삭제 후 새로운 값 입력
False: 기존 값을 지우지 않고 새로운 값 추가

### `searchMode`
대상 윈도우 객체 검색 방식을 지정합니다.
"All": UIA와 MSAA를 순차적으로 사용합니다.
"UIA": UIA 방식으로만 검색합니다.
"MSAA": MSAA 방식으로만 검색합니다.

### `selector`
대상 윈도우 객체를 선택합니다.
(값이 비어 있으면 현재 활성화된 컨트롤에 text 입력한다.)
ex) selector_0

### `text`
입력할 텍스트를 지정합니다.
ex) "hello world"

### `timeout`
윈도우 객체를 찾는 최대 시간을 지정합니다.
ex) 30 (단위: 초)

### `verify`
입력 값 검증 여부를 지정합니다.
True: 입력이 완료된 후 입력된 값을 다시 읽어와서 text 필드의 값과 일치하는지 검증한다.
False: 입력된 값을 검증하지 않는다.

