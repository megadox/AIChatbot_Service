# Activity: TapAutomation

## Summary
안드로이드 디바이스의 컨트롤을 탭하는 액티비티

## Metadata
- group: `ANDROID`
- script: `android_tap_automation()`
- pattern: `device\.android_tap_automation\(`
- dependencies: `ANDROID`
- theme: `Accent6`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `contentDesc` | `string` | `-` | - | 탭할 컨트롤 설명을 지정합니다.<br/>ex) "설정" |
| `controlClass` | `string` | `-` | - | 탭할 컨트롤 클래스를 지정합니다.(필수 값)<br/>ex) "android.widget.TextView" |
| `nthControl` | `string` | `1` | `1`, `2`, `3`, `4`, `5` | 정보가 동일한 컨트롤이 여러개 있을 경우 탭할 컨트롤의 인덱스를 지정합니다.<br/>ex) 1<br/>(1부터 시작) |
| `path` | `string` | `-` | - | 탭할 컨트롤의 경로를 지정합니다.<br/>ex) "/node/node[3]/node/node[4]"<br/>(searchType이 path일 경우 사용) |
| `resourceId` | `string` | `-` | - | 탭할 컨트롤 리소스 ID를 지정합니다.<br/>ex) "com.sec.android.app.launcher:id/hotseat_icon" |
| `searchType` | `string` | `"control"` | `"control"`, `"path"` | 검색할 타입을 지정합니다.<br/>"control": 컨트롤 정보로 검색합니다.<br/>"path": 경로로 검색합니다. |
| `taps` | `string` | `1` | `1`, `2`, `3` | 탭의 횟수를 지정합니다.<br/>ex) 2 |
| `text` | `string` | `-` | - | 탭할 컨트롤의 텍스트를 지정합니다.<br/>ex) "설정" |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |
| `waiting` | `string` | `True` | `True`, `False` | 컨트롤 대기 여부를 지정합니다.<br/>True: 컨트롤이 나타날 때까지 대기한다.<br/>False: 컨트롤이 나타날 때까지 대기하지 않는다. |

## Property Notes
### `contentDesc`
탭할 컨트롤 설명을 지정합니다.
ex) "설정"

### `controlClass`
탭할 컨트롤 클래스를 지정합니다.(필수 값)
ex) "android.widget.TextView"

### `nthControl`
정보가 동일한 컨트롤이 여러개 있을 경우 탭할 컨트롤의 인덱스를 지정합니다.
ex) 1
(1부터 시작)

### `path`
탭할 컨트롤의 경로를 지정합니다.
ex) "/node/node[3]/node/node[4]"
(searchType이 path일 경우 사용)

### `resourceId`
탭할 컨트롤 리소스 ID를 지정합니다.
ex) "com.sec.android.app.launcher:id/hotseat_icon"

### `searchType`
검색할 타입을 지정합니다.
"control": 컨트롤 정보로 검색합니다.
"path": 경로로 검색합니다.

### `taps`
탭의 횟수를 지정합니다.
ex) 2

### `text`
탭할 컨트롤의 텍스트를 지정합니다.
ex) "설정"

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

### `waiting`
컨트롤 대기 여부를 지정합니다.
True: 컨트롤이 나타날 때까지 대기한다.
False: 컨트롤이 나타날 때까지 대기하지 않는다.

