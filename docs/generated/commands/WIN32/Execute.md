# Activity: Execute

## Summary
윈도우 커맨드창(CMD)에서 명령어를 입력하는 액티비티

## Metadata
- group: `WIN32`
- script: `WIN32.Execute()`
- pattern: `= *WIN32\.Execute\(`
- dependencies: `WIN32`
- theme: `Accent3_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `command` | `string` | `-` | - | 수행할 명령어를 지정합니다.<br/>ex) "notepad" |
| `encoding` | `string` | `"cp949"` | `"utf-8"`, `"cp949"`, `"utf-16"` | Encoding Type을 지정합니다.<br/>ex) "cp949" |
| `params` | `string` | `-` | - | command 필드에 값에 뒤 따르는 입력 파라미터를 지정합니다.<br/>ex) "-f -u -p"<br/>(각 파라미터는 공백을 기준으로 구분한다.) |
| `returnHandle` | `string` | `False` | `True`, `False` | 객체 핸들의 반환 여부를 지정합니다.<br/>True: 객체의 핸들을 Name필드의 값에 반환한다.<br/>False: 객체의 핸들을 반환하지 않는다. |
| `startdir` | `string` | `"C:\"` | - | 명령을 수행할 시작 디렉토리를 지정합니다.<br/>ex) "C:\logs" |
| `timeout` | `string` | `-` | - | 대기할 최대 시간을 지정합니다.<br/>ex) 30 (단위: 초) |
| `waiting` | `string` | `False` | `True`, `False` | 실행 명령의 수행 완료 대기 여부를 지정합니다.<br/>True: 실행 명령의 수행 완료 시까지 대기한다.<br/>False: 실행 명령의 수행 완료 시까지 대기하지 않는다. |

## Property Notes
### `command`
수행할 명령어를 지정합니다.
ex) "notepad"

### `encoding`
Encoding Type을 지정합니다.
ex) "cp949"

### `params`
command 필드에 값에 뒤 따르는 입력 파라미터를 지정합니다.
ex) "-f -u -p"
(각 파라미터는 공백을 기준으로 구분한다.)

### `returnHandle`
객체 핸들의 반환 여부를 지정합니다.
True: 객체의 핸들을 Name필드의 값에 반환한다.
False: 객체의 핸들을 반환하지 않는다.

### `startdir`
명령을 수행할 시작 디렉토리를 지정합니다.
ex) "C:\logs"

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30 (단위: 초)

### `waiting`
실행 명령의 수행 완료 대기 여부를 지정합니다.
True: 실행 명령의 수행 완료 시까지 대기한다.
False: 실행 명령의 수행 완료 시까지 대기하지 않는다.

