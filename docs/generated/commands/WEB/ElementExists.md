# Activity: ElementExists

## Summary
특정 웹 엘리먼트의 존재 여부를 확인하는 액티비티

## Metadata
- group: `WEB`
- script: `element_exists()`
- pattern: `browser\.element_exists\(`
- dependencies: `WEB`
- theme: `Accent1`
- prefix: `exist`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `selector` | `string` | `-` | - | 대상 웹 객체를 선택합니다.<br/>ex) selector_1 |
| `timeout` | `string` | `30` | `30`, `40`, `50`, `60`, `100` | 엘리먼트를 찾는 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |

## Property Notes
### `selector`
대상 웹 객체를 선택합니다.
ex) selector_1

### `timeout`
엘리먼트를 찾는 최대 시간을 지정합니다.
ex) 30 (단위: 초)

