# Activity: SendMail(SMTP)

## Summary
SMTP를 통하여 이메일을 전송하는 액티비티

## Metadata
- group: `EMAIL`
- script: `EMAIL.SendMessage()`
- pattern: `EMAIL\.SendMessage`
- dependencies: `EMAIL`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `attach` | `string` | `-` | - | 첨부할 파일의 경로를 지정합니다.<br/>ex) "C:\mergeResult\result.pdf"<br/>(복수의 첨부파일 입력시 「,」로 구분한다.) |
| `body` | `string` | `-` | - | 이메일의 본문을 지정합니다.<br/>ex) "영업 1부의 상반기 예상수익을 보내드립니다." |
| `from` | `string` | `-` | - | 발신자의 이메일 주소를 지정합니다.<br/>ex) "bot@batem.com" |
| `imaphost` | `string` | `-` | - | 보낸 메일함 저장을 위한 IMAP서버의 주소를 지정합니다.<br/>(비어있을 경우 보낸 메일함 저장을 하지 않습니다.)\r<br/>ex) "imap.batem.com" |
| `imapport` | `string` | `-` | - | 보낸 메일함 저장을 위한 IMAP서버의 포트를 지정합니다.<br/>ex) 993 |
| `imaptype` | `string` | `"SSL"` | `"SSL"`, `"TLS"`, `"None"` | 보낸 메일함 저장을 위한 IMAP서버의 암호화 프로토콜을 지정합니다.<br/>"SSL": SSL을 사용한다.<br/>"TLS": TLS를 사용한다.<br/>"None": 암호화를 사용하지 않는다. |
| `smtphost` | `string` | `-` | - | SMTP서버의 주소를 지정합니다.<br/>ex) "smtp.batem.com" |
| `smtppassword` | `string` | `-` | - | SMTP서버의 사용자 비밀번호를 지정합니다.<br/>ex) "batem12345" |
| `smtpport` | `string` | `-` | - | SMTP서버의 포트 번호를 지정합니다.<br/>ex) 465 |
| `smtptype` | `string` | `"SSL"` | `"SSL"`, `"TLS"`, `"None"` | SMTP서버의 암호화 프로토콜을 지정합니다.<br/>"SSL": SSL을 사용한다.<br/>"TLS": TLS를 사용한다.<br/>"None": 암호화를 사용하지 않는다. |
| `smtpuser` | `string` | `-` | - | SMTP서버의 사용자 아이디를 지정합니다.<br/>ex) "batem" |
| `subject` | `string` | `-` | - | 이메일의 제목을 지정합니다.<br/>ex) "상반기예상수익" |
| `to` | `string` | `-` | - | 수신자의 이메일 주소를 지정합니다.<br/>ex) "rpa@batem.com" |

## Property Notes
### `attach`
첨부할 파일의 경로를 지정합니다.
ex) "C:\mergeResult\result.pdf"
(복수의 첨부파일 입력시 「,」로 구분한다.)

### `body`
이메일의 본문을 지정합니다.
ex) "영업 1부의 상반기 예상수익을 보내드립니다."

### `from`
발신자의 이메일 주소를 지정합니다.
ex) "bot@batem.com"

### `imaphost`
보낸 메일함 저장을 위한 IMAP서버의 주소를 지정합니다.
(비어있을 경우 보낸 메일함 저장을 하지 않습니다.)\r
ex) "imap.batem.com"

### `imapport`
보낸 메일함 저장을 위한 IMAP서버의 포트를 지정합니다.
ex) 993

### `imaptype`
보낸 메일함 저장을 위한 IMAP서버의 암호화 프로토콜을 지정합니다.
"SSL": SSL을 사용한다.
"TLS": TLS를 사용한다.
"None": 암호화를 사용하지 않는다.

### `smtphost`
SMTP서버의 주소를 지정합니다.
ex) "smtp.batem.com"

### `smtppassword`
SMTP서버의 사용자 비밀번호를 지정합니다.
ex) "batem12345"

### `smtpport`
SMTP서버의 포트 번호를 지정합니다.
ex) 465

### `smtptype`
SMTP서버의 암호화 프로토콜을 지정합니다.
"SSL": SSL을 사용한다.
"TLS": TLS를 사용한다.
"None": 암호화를 사용하지 않는다.

### `smtpuser`
SMTP서버의 사용자 아이디를 지정합니다.
ex) "batem"

### `subject`
이메일의 제목을 지정합니다.
ex) "상반기예상수익"

### `to`
수신자의 이메일 주소를 지정합니다.
ex) "rpa@batem.com"

