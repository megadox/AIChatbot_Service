# Activity: GetMail(POP3)

## Summary
POP3를 이용하여 조건에 해당하는 이메일의 정보를 딕셔너리로 가져오는 액티비티

## Metadata
- group: `EMAIL`
- script: `EMAIL.get_message_pop3()`
- pattern: `EMAIL\.get_message_pop3`
- dependencies: `EMAIL`
- prefix: `mail`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `attachmentSavePath` | `string` | `-` | - | 첨부파일을 저장할 경로를 지정합니다.<br/>(미입력시 첨부파일 저장 안함)<br/>ex) "C:\attachment" |
| `mailIndex` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 여러 개의 메일이 검색될 경우 가져올 이메일의 인덱스를 지정합니다.<br/>(메일은 수신 날짜 기준으로 정렬됩니다.)<br/>ex) 0 |
| `POP3Host` | `string` | `-` | - | POP3서버의 주소를 지정합니다.<br/>ex) "pop3.batem.com" |
| `POP3Password` | `string` | `-` | - | POP3서버의 사용자 비밀번호를 지정합니다.<br/>ex) "batem12345" |
| `POP3Port` | `string` | `995` | - | POP3서버의 포트 번호를 지정합니다.<br/>ex) 995 |
| `POP3User` | `string` | `-` | - | POP3서버의 사용자 아이디를 지정합니다.<br/>ex) "batem" |
| `searchDate` | `string` | `-` | - | 가져올 이메일의 날짜를 지정합니다.<br/>(미입력시 전체 날짜 검색)<br/>ex) "2024-01-01" |
| `searchLimit` | `string` | `100` | `10`, `50`, `100`, `150`, `200` | 검색할 이메일의 최대 개수를 지정합니다.<br/>ex) 100 |
| `sender` | `string` | `-` | - | 가져올 이메일의 보낸 사람의 이메일 주소를 지정합니다.<br/>(미입력시 전체 발신자 검색)<br/>ex) "rpa@batem.com" |
| `subject` | `string` | `-` | - | 가져올 이메일 제목에 포함된 문장 또는 단어를 지정합니다.<br/>(미입력시 전체 제목 검색)<br/>ex) "상반기예상수익" |

## Property Notes
### `attachmentSavePath`
첨부파일을 저장할 경로를 지정합니다.
(미입력시 첨부파일 저장 안함)
ex) "C:\attachment"

### `mailIndex`
여러 개의 메일이 검색될 경우 가져올 이메일의 인덱스를 지정합니다.
(메일은 수신 날짜 기준으로 정렬됩니다.)
ex) 0

### `POP3Host`
POP3서버의 주소를 지정합니다.
ex) "pop3.batem.com"

### `POP3Password`
POP3서버의 사용자 비밀번호를 지정합니다.
ex) "batem12345"

### `POP3Port`
POP3서버의 포트 번호를 지정합니다.
ex) 995

### `POP3User`
POP3서버의 사용자 아이디를 지정합니다.
ex) "batem"

### `searchDate`
가져올 이메일의 날짜를 지정합니다.
(미입력시 전체 날짜 검색)
ex) "2024-01-01"

### `searchLimit`
검색할 이메일의 최대 개수를 지정합니다.
ex) 100

### `sender`
가져올 이메일의 보낸 사람의 이메일 주소를 지정합니다.
(미입력시 전체 발신자 검색)
ex) "rpa@batem.com"

### `subject`
가져올 이메일 제목에 포함된 문장 또는 단어를 지정합니다.
(미입력시 전체 제목 검색)
ex) "상반기예상수익"

