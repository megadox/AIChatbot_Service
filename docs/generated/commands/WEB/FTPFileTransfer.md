# Activity: FTPFileTransfer

## Summary
FTP를 이용해 파일을 전송하는 액티비티

## Metadata
- group: `WEB`
- script: `WEB.ftp_file_move()`
- pattern: `WEB\.ftp_file_move\(`
- dependencies: `WEB`
- theme: `Accent1`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `destPath` | `string` | `-` | - | 업로드 또는 다운로드할 파일의 목적 경로를 지정합니다.<br/>ex) "C:\download\sample.txt" |
| `encoding` | `string` | `"utf-8"` | `"utf-8"`, `"euc-kr"`, `"cp949"`, `"utf-16"` | 웹 브라우저 기본 인코딩 값 지정합니다.<br/>ex) "utf-8" |
| `host` | `string` | `-` | - | 연결할 호스트 ip 주소를 지정합니다.<br/>ex) 66.123.45.678 |
| `option` | `string` | `"upload"` | `"upload"`, `"download"` | 파일 전송 방식을 지정합니다.<br/>"upload": 파일을 업로드한다.<br/>"download": 파일을 다운로드한다. |
| `port` | `string` | `-` | - | 연결할 호스트 ip 주소의 port 번호를 지정합니다.<br/>ex) 4321 |
| `srcPath` | `string` | `-` | - | 업로드 또는 다운로드할 소스 파일의 경로를 지정합니다.<br/>ex) "D:\test\sample.txt" |
| `timeout` | `string` | `30` | `30`, `60`, `120`, `300` | 대기할 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |
| `userId` | `string` | `-` | - | 연결할 호스트의 아이디를 지정합니다.<br/>ex) "batem" |
| `userPw` | `string` | `-` | - | 연결할 호스트의 패스워드를 지정합니다.<br/>ex) "batem12345" |

## Property Notes
### `destPath`
업로드 또는 다운로드할 파일의 목적 경로를 지정합니다.
ex) "C:\download\sample.txt"

### `encoding`
웹 브라우저 기본 인코딩 값 지정합니다.
ex) "utf-8"

### `host`
연결할 호스트 ip 주소를 지정합니다.
ex) 66.123.45.678

### `option`
파일 전송 방식을 지정합니다.
"upload": 파일을 업로드한다.
"download": 파일을 다운로드한다.

### `port`
연결할 호스트 ip 주소의 port 번호를 지정합니다.
ex) 4321

### `srcPath`
업로드 또는 다운로드할 소스 파일의 경로를 지정합니다.
ex) "D:\test\sample.txt"

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30 (단위: 초)

### `userId`
연결할 호스트의 아이디를 지정합니다.
ex) "batem"

### `userPw`
연결할 호스트의 패스워드를 지정합니다.
ex) "batem12345"

