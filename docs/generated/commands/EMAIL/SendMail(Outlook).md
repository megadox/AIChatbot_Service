# Activity: SendMail(Outlook)

## Summary
Outlook을 통하여 이메일을 전송하는 액티비티

## Metadata
- group: `EMAIL`
- script: `EMAIL.SendMessageWithOutlook()`
- pattern: `EMAIL\.SendMessageWithOutlook`
- dependencies: `EMAIL`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `attach` | `string` | `-` | - | 첨부할 파일의 경로를 지정합니다.<br/>ex) "C:\mergeResult\result.pdf"<br/>(복수의 첨부파일 입력시 「,」로 구분한다.) |
| `bcc` | `string` | `-` | - | 숨은 참조자의 이메일 주소를 지정합니다.<br/>ex) "bcc-rpa@batem.com"<br/>(복수의 수신자 입력시 「;」로 구분한다.) |
| `body` | `string` | `-` | - | 이메일의 본문을 지정합니다.<br/>ex) "영업 1부의 상반기 예상수익을 보내드립니다." |
| `cc` | `string` | `-` | - | 참조자의 이메일 주소를 지정합니다.<br/>ex) "cc-rpa@batem.com"<br/>(복수의 수신자 입력시 「;」로 구분한다.) |
| `subject` | `string` | `-` | - | 이메일의 제목을 지정합니다.<br/>ex) "상반기예상수익" |
| `to` | `string` | `-` | - | 수신자의 이메일 주소를 지정합니다.<br/>ex) "rpa@batem.com" |
| `waiting` | `string` | `3` | - | 메일 전송 완료를 위한 대기 시간을 지정합니다.<br/>ex) 3 (단위: 초) |

## Property Notes
### `attach`
첨부할 파일의 경로를 지정합니다.
ex) "C:\mergeResult\result.pdf"
(복수의 첨부파일 입력시 「,」로 구분한다.)

### `bcc`
숨은 참조자의 이메일 주소를 지정합니다.
ex) "bcc-rpa@batem.com"
(복수의 수신자 입력시 「;」로 구분한다.)

### `body`
이메일의 본문을 지정합니다.
ex) "영업 1부의 상반기 예상수익을 보내드립니다."

### `cc`
참조자의 이메일 주소를 지정합니다.
ex) "cc-rpa@batem.com"
(복수의 수신자 입력시 「;」로 구분한다.)

### `subject`
이메일의 제목을 지정합니다.
ex) "상반기예상수익"

### `to`
수신자의 이메일 주소를 지정합니다.
ex) "rpa@batem.com"

### `waiting`
메일 전송 완료를 위한 대기 시간을 지정합니다.
ex) 3 (단위: 초)

