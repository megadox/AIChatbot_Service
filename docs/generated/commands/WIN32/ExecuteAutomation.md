# Activity: ExecuteAutomation

## Summary
다이얼로그를 통하여 실행파일을 찾아서 윈도우 애플리케이션을 실행하는 액티비티

## Metadata
- group: `WIN32`
- script: `MSAA.Execute()`
- pattern: `msaa = MSAA\.Execute\(`
- dependencies: `MSAA`
- theme: `Accent3_5`
- prefix: `msaa`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `encoding` | `string` | `"cp949"` | `"utf-8"`, `"cp949"`, `"utf-16"` | Encoding Type을 지정합니다.<br/>ex) "cp949" |
| `file` | `string` | `-` | - | 실행할 파일의 경로를 지정합니다.<br/>ex) "C:\sample\sample.exe"<br/>(selector를 이용하여 경로를 가져올 수 있다.) |
| `params` | `string` | `-` | - | file 필드에 값에 뒤 따르는 입력 파라미터를 지정합니다.<br/>ex) "-f -u -p"<br/><br/>(각 파라미터는 공백을 기준으로 구분한다.) |
| `returnHandle` | `string` | `True` | `True`, `False` | 객체 핸들의 반환 여부를 지정합니다.<br/>True: 객체의 핸들을 Name필드의 값에 반환한다.<br/>False: 객체의 핸들을 반환하지 않는다. |
| `waiting` | `string` | `False` | `True`, `False` | 실행 명령의 수행 완료 대기 여부를 지정합니다.<br/>True: 실행 명령의 수행 완료 시까지 대기한다.<br/>False: 실행 명령의 수행 완료 시까지 대기하지 않는다. |

## Property Notes
### `encoding`
Encoding Type을 지정합니다.
ex) "cp949"

### `file`
실행할 파일의 경로를 지정합니다.
ex) "C:\sample\sample.exe"
(selector를 이용하여 경로를 가져올 수 있다.)

### `params`
file 필드에 값에 뒤 따르는 입력 파라미터를 지정합니다.
ex) "-f -u -p"

(각 파라미터는 공백을 기준으로 구분한다.)

### `returnHandle`
객체 핸들의 반환 여부를 지정합니다.
True: 객체의 핸들을 Name필드의 값에 반환한다.
False: 객체의 핸들을 반환하지 않는다.

### `waiting`
실행 명령의 수행 완료 대기 여부를 지정합니다.
True: 실행 명령의 수행 완료 시까지 대기한다.
False: 실행 명령의 수행 완료 시까지 대기하지 않는다.

