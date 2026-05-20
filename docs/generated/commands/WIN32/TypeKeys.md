# Activity: TypeKeys

## Summary
현재 활성화된 윈도우 애플리케이션 오브젝트(컨트롤)에 특정 키보드 Key를 입력하는 액티비티

## Metadata
- group: `WIN32`
- script: `WIN32.TypeKeys()`
- pattern: `WIN32\.TypeKeys\(`
- dependencies: `WIN32`
- theme: `Accent3_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `delay` | `string` | `0` | `0`, `500`, `1000`, `1500`, `2000`, `5000`, `10000` | 여러 키를 입력할 경우 각 키 입력간 대기시간을 지정합니다.<br/>ex) 500 (단위: 밀리세컨) |
| `keys` | `string` | `-` | - | 입력할 키보드 키를 지정합니다.<br/>ex) "esc"<br/>(복합키 사용은 '+'를 이용한다.) |
| `repeat` | `string` | `1` | `1`, `2`, `3`, `4`, `5` | 입력 반복 횟수를 지정합니다.<br/>ex) 2 |
| `searchMode` | `string` | `"All"` | `"All"`, `"UIA"`, `"MSAA"` | 대상 윈도우 객체 검색 방식을 지정합니다.<br/>"All": UIA와 MSAA를 순차적으로 사용합니다.<br/>"UIA": UIA 방식으로만 검색합니다.<br/>"MSAA": MSAA 방식으로만 검색합니다. |
| `selector` | `string` | `-` | - | 대상 윈도우 객체를 선택합니다.<br/>(값이 비어 있으면 현재 활성화된 컨트롤에 keys 입력한다.)<br/>ex) selector_0 |
| `timeout` | `string` | `30` | `5`, `10`, `20`, `30`, `40`, `50`, `60` | 윈도우 객체를 찾는 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |

## Property Notes
### `delay`
여러 키를 입력할 경우 각 키 입력간 대기시간을 지정합니다.
ex) 500 (단위: 밀리세컨)

### `keys`
입력할 키보드 키를 지정합니다.
ex) "esc"
(복합키 사용은 '+'를 이용한다.)

### `repeat`
입력 반복 횟수를 지정합니다.
ex) 2

### `searchMode`
대상 윈도우 객체 검색 방식을 지정합니다.
"All": UIA와 MSAA를 순차적으로 사용합니다.
"UIA": UIA 방식으로만 검색합니다.
"MSAA": MSAA 방식으로만 검색합니다.

### `selector`
대상 윈도우 객체를 선택합니다.
(값이 비어 있으면 현재 활성화된 컨트롤에 keys 입력한다.)
ex) selector_0

### `timeout`
윈도우 객체를 찾는 최대 시간을 지정합니다.
ex) 30 (단위: 초)

