# Activity: HideAutomation

## Summary
특정 윈도우 애플리케이션 창을 화면에서 숨기는 액티비티
(숨긴 창의 컨트롤을 반환합니다.)

## Metadata
- group: `WIN32`
- script: `MSAA.Hide()`
- pattern: `msaa\.Hide\(`
- dependencies: `MSAA`
- theme: `Accent3_5`
- prefix: `msaa`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `control` | `string` | `-` | - | 대상 윈도우 객체를 선택합니다.<br/>ex) msaa_0<br/>(값이 비어 있을 경우 selector 프로퍼티를 사용하여 윈도우 객체를 선택합니다.) |
| `searchMode` | `string` | `"All"` | `"All"`, `"UIA"`, `"MSAA"` | 대상 윈도우 객체 검색 방식을 지정합니다.<br/>"All": UIA와 MSAA를 순차적으로 사용합니다.<br/>"UIA": UIA 방식으로만 검색합니다.<br/>"MSAA": MSAA 방식으로만 검색합니다. |
| `selector` | `string` | `-` | - | 대상 윈도우 객체를 선택합니다.<br/>ex) selector_0<br/>(control 프로퍼티 값이 비어 있을 경우에 사용합니다.) |
| `timeout` | `string` | `30` | `5`, `10`, `20`, `30`, `40`, `50`, `60` | 윈도우 객체를 찾는 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |

## Property Notes
### `control`
대상 윈도우 객체를 선택합니다.
ex) msaa_0
(값이 비어 있을 경우 selector 프로퍼티를 사용하여 윈도우 객체를 선택합니다.)

### `searchMode`
대상 윈도우 객체 검색 방식을 지정합니다.
"All": UIA와 MSAA를 순차적으로 사용합니다.
"UIA": UIA 방식으로만 검색합니다.
"MSAA": MSAA 방식으로만 검색합니다.

### `selector`
대상 윈도우 객체를 선택합니다.
ex) selector_0
(control 프로퍼티 값이 비어 있을 경우에 사용합니다.)

### `timeout`
윈도우 객체를 찾는 최대 시간을 지정합니다.
ex) 30 (단위: 초)

