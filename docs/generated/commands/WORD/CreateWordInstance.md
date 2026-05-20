# Activity: CreateWordInstance

## Summary
MS워드 문서의 인스턴스를 생성하는 액티비티

## Metadata
- group: `WORD`
- script: `WORD.create_word_instance()`
- pattern: `WORD\.create_word_instance\(`
- dependencies: `WORD`
- prefix: `msword`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `activate` | `string` | `False` | `True`, `False` | MS워드 창을 활성화할지 여부를 지정합니다.<br/>True: 활성화한다.<br/>False: 비활성화한다. |
| `headless` | `string` | `False` | `True`, `False` | 그래픽 사용자 인터페이스 사용 여부를 지정합니다.<br/>True: MS워드 그래픽 인터페이스를 사용하지 않고 백그라운드에서 동작합니다.<br/>False: MS워드 그래픽 인터페이스를 사용하여 동작합니다. |
| `windowState` | `string` | `"Maximized"` | `"Maximized"`, `"Minimized"`, `"Normal"` | MS워드 창의 상태를 지정합니다.<br/>"Maximized": 최대화<br/>"Minimized": 최소화<br/>"Normal": 일반 크기 |

## Property Notes
### `activate`
MS워드 창을 활성화할지 여부를 지정합니다.
True: 활성화한다.
False: 비활성화한다.

### `headless`
그래픽 사용자 인터페이스 사용 여부를 지정합니다.
True: MS워드 그래픽 인터페이스를 사용하지 않고 백그라운드에서 동작합니다.
False: MS워드 그래픽 인터페이스를 사용하여 동작합니다.

### `windowState`
MS워드 창의 상태를 지정합니다.
"Maximized": 최대화
"Minimized": 최소화
"Normal": 일반 크기

