# Activity: OpenBrowser

## Summary
웹 브라우저를 새로 열 때 사용하는 액티비티

## Metadata
- group: `WEB`
- script: `WEB.Open()`
- pattern: `WEB\.Open\(`
- dependencies: `WEB`
- theme: `Accent1`
- prefix: `browser`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `browser` | `string` | `"Chrome"` | `"IE"`, `"EdgeIeMode"`, `"Chrome"`, `"Edge"` | 사용할 웹 브라우저 종류를 지정합니다.<br/>ex) "Edge" |
| `downloadDialog` | `string` | `True` | `True`, `False` | 다운로드 프롬프트(창)의 사용 여부를 지정합니다.<br/>True: 다운로드 시 다운로드 경로를 지정할 수 있는 프롬프트(창)를 연다.<br/>False: 다운로드 시 브라우저 다운로드 기본 경로값에 다운로드를 지정한다.<br/>(IE, EdgeIeMode에서 사용 불가) |
| `driverPath` | `string` | `` | - | 웹 드라이버의 경로를 지정합니다.<br/>ex) "C:\WebDriver\chromedriver.exe"<br/>(비어있는 경우 내장된 웹 드라이버를 사용한다.) |
| `encoding` | `string` | `"utf-8"` | `"utf-8"`, `"cp949"`, `"utf-16"` | 웹 브라우저 기본 인코딩 값 지정합니다.<br/>ex) "utf-8" |
| `gpuAcceleration` | `string` | `False` | `True`, `False` | GPU 가속 사용 여부를 지정합니다.<br/>True: GPU 가속을 사용한다.<br/>False: GPU 가속을 사용하지 않는다.<br/>(IE, EdgeIeMode에서 사용 불가) |
| `headless` | `string` | `False` | `True`, `False` | 그래픽 사용자 인터페이스 사용 여부를 지정합니다.<br/>True: 웹 브라우저 그래픽 인터페이스를 사용하지 않고 백그라운드에서 동작합니다.<br/>False: 웹 브라우저 그래픽 인터페이스를 사용하여 동작합니다.<br/>(IE, EdgeIeMode에서 사용 불가) |
| `infoBar` | `string` | `True` | `True`, `False` | 브라우저 상단의 자동화 메시지 표시 여부를 지정합니다.<br/>True: 웹 브라우저 상단에 자동화 안내 메시지를 띄운다.<br/>False: 웹 브라우저 상단에 자동화 안내 메시지를 띄우지 않는다.<br/>(Chrome에서만 사용 가능) |
| `reuse` | `string` | `True` | `True`, `False` | 마지막으로 사용한 웹 브라우저 객체의 재사용 여부를 지정합니다.<br/>True: 기존의 브라우저에서 URL에 지정된 페이지를 연다.<br/>False: 새로운 브라우저에서 URL에 지정된 페이지를 연다. |
| `timeout` | `string` | `60` | `30`, `60`, `120`, `180`, `240` | 웹 페이지 오픈의 최대 대기시간을 지정합니다.<br/>ex) 30 (단위: 초) |
| `url` | `string` | `-` | - | 접속할 URL 주소를 지정합니다.<br/>ex) "https://www.batem.com" |
| `userData` | `string` | `"None"` | `"None"`, `"C:\path\to\UserData"` | 브라우저 유저 데이터를 사용 여부를 지정합니다.<br/>"None": 웹 브라우저 유저 데이터를 사용하지 않는다.<br/>"C:\path\to\UserData": 사용자가 지정한 경로의 유저 데이터를 사용한다.<br/>(IE, EdgeIeMode에서 사용 불가) |
| `windowState` | `string` | `"Maximized"` | `"Maximized"`, `"Minimized"`, `"Normal"` | 웹 브라우저의 창 상태를 지정합니다.<br/>"Maximized": 최대화<br/>"Minimized": 최소화<br/>"Normal": 일반 크기 |

## Property Notes
### `browser`
사용할 웹 브라우저 종류를 지정합니다.
ex) "Edge"

### `downloadDialog`
다운로드 프롬프트(창)의 사용 여부를 지정합니다.
True: 다운로드 시 다운로드 경로를 지정할 수 있는 프롬프트(창)를 연다.
False: 다운로드 시 브라우저 다운로드 기본 경로값에 다운로드를 지정한다.
(IE, EdgeIeMode에서 사용 불가)

### `driverPath`
웹 드라이버의 경로를 지정합니다.
ex) "C:\WebDriver\chromedriver.exe"
(비어있는 경우 내장된 웹 드라이버를 사용한다.)

### `encoding`
웹 브라우저 기본 인코딩 값 지정합니다.
ex) "utf-8"

### `gpuAcceleration`
GPU 가속 사용 여부를 지정합니다.
True: GPU 가속을 사용한다.
False: GPU 가속을 사용하지 않는다.
(IE, EdgeIeMode에서 사용 불가)

### `headless`
그래픽 사용자 인터페이스 사용 여부를 지정합니다.
True: 웹 브라우저 그래픽 인터페이스를 사용하지 않고 백그라운드에서 동작합니다.
False: 웹 브라우저 그래픽 인터페이스를 사용하여 동작합니다.
(IE, EdgeIeMode에서 사용 불가)

### `infoBar`
브라우저 상단의 자동화 메시지 표시 여부를 지정합니다.
True: 웹 브라우저 상단에 자동화 안내 메시지를 띄운다.
False: 웹 브라우저 상단에 자동화 안내 메시지를 띄우지 않는다.
(Chrome에서만 사용 가능)

### `reuse`
마지막으로 사용한 웹 브라우저 객체의 재사용 여부를 지정합니다.
True: 기존의 브라우저에서 URL에 지정된 페이지를 연다.
False: 새로운 브라우저에서 URL에 지정된 페이지를 연다.

### `timeout`
웹 페이지 오픈의 최대 대기시간을 지정합니다.
ex) 30 (단위: 초)

### `url`
접속할 URL 주소를 지정합니다.
ex) "https://www.batem.com"

### `userData`
브라우저 유저 데이터를 사용 여부를 지정합니다.
"None": 웹 브라우저 유저 데이터를 사용하지 않는다.
"C:\path\to\UserData": 사용자가 지정한 경로의 유저 데이터를 사용한다.
(IE, EdgeIeMode에서 사용 불가)

### `windowState`
웹 브라우저의 창 상태를 지정합니다.
"Maximized": 최대화
"Minimized": 최소화
"Normal": 일반 크기

