# Activity: SwitchFrame

## Summary
웹 페이지에서 컨트롤할 frame을 인덱스 방식으로 변경하는 액티비티(frame 내부에서 ExecuteScript을 사용하기 위한 액티비티)

## Metadata
- group: `WEB`
- script: `SwitchToFrame()`
- pattern: `\.SwitchToFrame\(`
- dependencies: `WEB`
- theme: `Accent1`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `frameName` | `string` | `-` | - | 컨트롤할 프레임의 name 속성 또는 id 속성을 지정합니다.<br/>(해당 프로퍼티가 nth_frame 프로퍼티보다 우선순위를 가진다.)<br/>ex) "frame1" |
| `frameTag` | `string` | `"iframe"` | `"frame"`, `"iframe"` | 컨트롤할 프레임 태그를 지정합니다.<br/>ex) "iframe" |
| `nthFrame` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 컨트롤할 frame/iframe 태그의 인덱스 번호를 지정합니다.<br/>(frameName 프로퍼티가 빈 값일 경우 동작)<br/>ex) 2 |
| `switchDefault` | `string` | `False` | `True`, `False` | 기본 프레임으로 전환 여부를 지정합니다.<br/>True: 기본 프레임으로 전환한다.<br/>False: nth_frame에서 지정한 프레임으로 전환한다. |

## Property Notes
### `frameName`
컨트롤할 프레임의 name 속성 또는 id 속성을 지정합니다.
(해당 프로퍼티가 nth_frame 프로퍼티보다 우선순위를 가진다.)
ex) "frame1"

### `frameTag`
컨트롤할 프레임 태그를 지정합니다.
ex) "iframe"

### `nthFrame`
컨트롤할 frame/iframe 태그의 인덱스 번호를 지정합니다.
(frameName 프로퍼티가 빈 값일 경우 동작)
ex) 2

### `switchDefault`
기본 프레임으로 전환 여부를 지정합니다.
True: 기본 프레임으로 전환한다.
False: nth_frame에서 지정한 프레임으로 전환한다.

