# Activity: ExecuteScript

## Summary
웹 브라우저에서 JavaScript를 실행하는 액티비티

## Metadata
- group: `WEB`
- script: `ExecuteScript()`
- pattern: `.*\.ExecuteScript\(`
- dependencies: `WEB`
- theme: `Accent1`
- prefix: `result`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `encoding` | `string` | `"utf-8"` | `"utf-8"`, `"cp949"`, `"utf-16"` | 웹 브라우저 기본 인코딩 값 지정합니다.<br/>ex) "utf-8" |
| `script` | `string` | `-` | - | Javascript 문법에 맞는 스크립트를 지정합니다.<br/>ex) document.querySelector(‘#query’).click()<br/>(return 구문을 가장 앞에 추가하면 결과값이 반환된다.)<br/>ex) return document.querySelector(‘#query’).innerText |

## Property Notes
### `encoding`
웹 브라우저 기본 인코딩 값 지정합니다.
ex) "utf-8"

### `script`
Javascript 문법에 맞는 스크립트를 지정합니다.
ex) document.querySelector(‘#query’).click()
(return 구문을 가장 앞에 추가하면 결과값이 반환된다.)
ex) return document.querySelector(‘#query’).innerText

