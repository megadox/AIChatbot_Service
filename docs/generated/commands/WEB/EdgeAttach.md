# Activity: EdgeAttach

## Summary
열려있는 엣지 브라우저를 객체로 가져오는 액티비티
(엣지 브라우저에 --remote-debugging-port=9223 인자 사용 필요)

## Metadata
- group: `WEB`
- script: `WEB.attach_edge()`
- pattern: `WEB\.attach_edge\(`
- dependencies: `WEB`
- theme: `Accent1`
- prefix: `browser`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `driverPath` | `string` | `` | - | 웹 드라이버의 경로를 지정합니다.<br/>ex) "C:\WebDriver\edgedriver.exe"<br/>(비어있는 경우 웹 드라이버를 자동으로 다운받아 사용한다.) |

## Property Notes
### `driverPath`
웹 드라이버의 경로를 지정합니다.
ex) "C:\WebDriver\edgedriver.exe"
(비어있는 경우 웹 드라이버를 자동으로 다운받아 사용한다.)

