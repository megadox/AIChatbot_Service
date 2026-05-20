# Activity: ShowAutomation

## Summary
특정 윈도우 애플리케이션 창을 화면에 표시하는 액티비티

## Metadata
- group: `WIN32`
- script: `MSAA.Show()`
- pattern: `msaa\.Show\(`
- dependencies: `MSAA`
- theme: `Accent3_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `control` | `string` | `-` | - | 대상 윈도우 객체를 선택합니다.<br/>ex) msaa_0 |
| `focus` | `string` | `True` | `True`, `False` | 창을 표시한 이후에 해당 창에 포커스를 줄지 여부를 지정합니다.<br/>True: 창을 표시한 이후에 해당 창에 포커스를 준다.<br/>False: 창을 표시한 이후에 해당 창에 포커스를 주지 않는다. |

## Property Notes
### `control`
대상 윈도우 객체를 선택합니다.
ex) msaa_0

### `focus`
창을 표시한 이후에 해당 창에 포커스를 줄지 여부를 지정합니다.
True: 창을 표시한 이후에 해당 창에 포커스를 준다.
False: 창을 표시한 이후에 해당 창에 포커스를 주지 않는다.

