# Activity: WaitForElement

## Summary
SAP내 특정 엘리먼트가 나타나기를 기다리는 액티비티

## Metadata
- group: `SAP`
- script: `WaitForElement()`
- pattern: `sap.*\.WaitForElement\(`
- dependencies: `SAP`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `path` | `string` | `-` | - | 대기할 SAP 엘리먼트의 경로를 지정합니다. |
| `second` | `string` | `-` | - | 대기할 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |

## Property Notes
### `path`
대기할 SAP 엘리먼트의 경로를 지정합니다.

### `second`
대기할 최대 시간을 지정합니다.
ex) 30 (단위: 초)

