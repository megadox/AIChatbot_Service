# Activity: Navigate

## Summary
웹 브라우저에서 새로운 웹 사이트로 이동하는 액티비티

## Metadata
- group: `WEB`
- script: `Navigate()`
- pattern: `.*\.Navigate\(`
- dependencies: `WEB`
- theme: `Accent1`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `encoding` | `string` | `"utf-8"` | `"utf-8"`, `"cp949"`, `"utf-16"` | 웹 브라우저 기본 인코딩 값 지정합니다.<br/>ex) "utf-8" |
| `url` | `string` | `-` | - | 이동할 사이트의 URL 주소를 지정합니다.<br/>ex) "https://www.batem.com" |

## Property Notes
### `encoding`
웹 브라우저 기본 인코딩 값 지정합니다.
ex) "utf-8"

### `url`
이동할 사이트의 URL 주소를 지정합니다.
ex) "https://www.batem.com"

