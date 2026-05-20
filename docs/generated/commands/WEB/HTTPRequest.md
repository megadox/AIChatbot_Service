# Activity: HTTPRequest

## Summary
URL로 HTTP 메시지를 보내는 액티비티

## Metadata
- group: `WEB`
- script: `WEB.Request()`
- pattern: `WEB\.Request\(`
- dependencies: `WEB`
- theme: `Accent1`
- prefix: `response`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `authentication` | `string` | `-` | - | HTTP Basic Auth 인증 정보를 지정합니다.<br/>ex) userId,password |
| `cookies` | `string` | `-` | - | HTTP 요청에 포함할 쿠키의 정보를 지정합니다.<br/>ex) {"c":"exmple1", "d":"exmple2"}<br/>(python dictionary 형태의 문자열을 지원한다.) |
| `headers` | `string` | `-` | - | HTTP 요청의 헤더 정보를 지정합니다.<br/>ex) {"a":"123", "b":"456"}<br/>(python dictionary 형태의 문자열을 지원한다.) |
| `method` | `string` | `"GET"` | `"Get"`, `"Post"` | HTTP 메소드 종류를 지정합니다.<br/>ex) "GET"<br/>(GET과 POST 메소드를 지원한다.) |
| `params` | `string` | `-` | - | HTTP 요청의 파라미터를 지정합니다.<br/>ex) "a=1&b=2"<br/>(python dictionary 형태 또는 유효한 queryString 포맷의 문자열을 지원한다.) |
| `returnType` | `string` | `"Text"` | `"Text"`, `"Json"`, `"Object"` | 요청에 대한 응답 값의 타입을 지정합니다.<br/>ex) "Json" |
| `timeout` | `string` | `30000` | `30000`, `60000`, `120000`, `300000` | 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |
| `url` | `string` | `-` | - | HTTP 요청을 보낼 URL 주소를 지정합니다.<br/>ex) "http://apis.data.go.kr/12345" |
| `verify` | `string` | `True` | `True`, `False`, `r"path\to\pem"` | SSL 인증서 검증 여부를 지정합니다.<br/>True: SSL 인증서를 검증한다.<br/>False: SSL 인증서를 검증하지 않는다.<br/>또는 PEM 형식의 인증서 파일 경로를 지정할 수 있다.<br/>ex) True |

## Property Notes
### `authentication`
HTTP Basic Auth 인증 정보를 지정합니다.
ex) userId,password

### `cookies`
HTTP 요청에 포함할 쿠키의 정보를 지정합니다.
ex) {"c":"exmple1", "d":"exmple2"}
(python dictionary 형태의 문자열을 지원한다.)

### `headers`
HTTP 요청의 헤더 정보를 지정합니다.
ex) {"a":"123", "b":"456"}
(python dictionary 형태의 문자열을 지원한다.)

### `method`
HTTP 메소드 종류를 지정합니다.
ex) "GET"
(GET과 POST 메소드를 지원한다.)

### `params`
HTTP 요청의 파라미터를 지정합니다.
ex) "a=1&b=2"
(python dictionary 형태 또는 유효한 queryString 포맷의 문자열을 지원한다.)

### `returnType`
요청에 대한 응답 값의 타입을 지정합니다.
ex) "Json"

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

### `url`
HTTP 요청을 보낼 URL 주소를 지정합니다.
ex) "http://apis.data.go.kr/12345"

### `verify`
SSL 인증서 검증 여부를 지정합니다.
True: SSL 인증서를 검증한다.
False: SSL 인증서를 검증하지 않는다.
또는 PEM 형식의 인증서 파일 경로를 지정할 수 있다.
ex) True

