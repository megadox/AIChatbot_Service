# Activity: SwitchBrowserWindow

## Summary
웹 브라우저의 자식 브라우저(팝업)로 컨트롤 타겟을 변경하는 액티비티

## Metadata
- group: `WEB`
- script: `SwitchToWindow()`
- pattern: `\.SwitchToWindow\(`
- dependencies: `WEB`
- theme: `Accent1`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `activate` | `string` | `False` | `True`, `False` | 컨트롤을 전환한 웹 브라우저 창을 활성화할지 여부를 지정합니다.<br/>True: 컨트롤을 전환한 웹 브라우저 창을 활성화한다.<br/>False: 컨트롤을 전환한 웹 브라우저 창을 활성화하지 않는다. |
| `nthChild` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 컨트롤을 전환할 웹 브라우저 창의 인덱스 번호를 지정합니다.<br/>ex) 2<br/>(기본 창의 인덱스 번호는 0이다.) |

## Property Notes
### `activate`
컨트롤을 전환한 웹 브라우저 창을 활성화할지 여부를 지정합니다.
True: 컨트롤을 전환한 웹 브라우저 창을 활성화한다.
False: 컨트롤을 전환한 웹 브라우저 창을 활성화하지 않는다.

### `nthChild`
컨트롤을 전환할 웹 브라우저 창의 인덱스 번호를 지정합니다.
ex) 2
(기본 창의 인덱스 번호는 0이다.)

