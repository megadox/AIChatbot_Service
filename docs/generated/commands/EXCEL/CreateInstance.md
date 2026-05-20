# Activity: CreateInstance

## Summary
엑셀의 인스턴스를 생성할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `EXCEL.CreateInstance()`
- pattern: `= *EXCEL\.CreateInstance\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `excel`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `activate` | `string` | `False` | `True`, `False` | 엑셀 창을 활성화할지 여부를 지정합니다.<br/>True: 엑셀 창을 활성화합니다.<br/>False: 엑셀 창을 활성화하지 않습니다. |
| `headless` | `string` | `False` | `True`, `False` | 그래픽 사용자 인터페이스 사용 여부를 지정합니다.<br/>True: 엑셀 그래픽 인터페이스를 사용하지 않고 백그라운드에서 동작합니다.<br/>False: 엑셀 그래픽 인터페이스를 사용하여 동작합니다. |
| `retry` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 재시도 횟수를 지정합니다.<br/>ex) 3<br/>(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.) |
| `windowState` | `string` | `"Maximized"` | `"Maximized"`, `"Minimized"`, `"Normal"` | 엑셀 창의 상태를 지정합니다.<br/>"Maximized": 최대화<br/>"Minimized": 최소화<br/>"Normal": 일반 |

## Property Notes
### `activate`
엑셀 창을 활성화할지 여부를 지정합니다.
True: 엑셀 창을 활성화합니다.
False: 엑셀 창을 활성화하지 않습니다.

### `headless`
그래픽 사용자 인터페이스 사용 여부를 지정합니다.
True: 엑셀 그래픽 인터페이스를 사용하지 않고 백그라운드에서 동작합니다.
False: 엑셀 그래픽 인터페이스를 사용하여 동작합니다.

### `retry`
재시도 횟수를 지정합니다.
ex) 3
(오류가 발생하면 재시도 시 0.5초 간격으로 액티비티를 수행 내용을 재시도한다.)

### `windowState`
엑셀 창의 상태를 지정합니다.
"Maximized": 최대화
"Minimized": 최소화
"Normal": 일반

