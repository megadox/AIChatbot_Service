# Activity: GetValueAutomation

## Summary
안드로이드 디바이스의 컨트롤의 특정 value 값을 가져오는 액티비티

## Metadata
- group: `ANDROID`
- script: `android_get_value_automation()`
- pattern: `device\.android_get_value_automation\(`
- dependencies: `ANDROID`
- theme: `Accent6`
- prefix: `value`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `contentDesc` | `string` | `-` | - | 값을 가져올 컨트롤 설명을 지정합니다.<br/>ex) "설정" |
| `controlClass` | `string` | `-` | - | 값을 가져올 컨트롤 클래스를 지정합니다.(필수 값)<br/>ex) "android.widget.TextView" |
| `nthControl` | `string` | `1` | `1`, `2`, `3`, `4`, `5` | 정보가 동일한 컨트롤이 여러개 있을 경우 가져올 컨트롤의 인덱스를 지정합니다.<br/>ex) 1<br/>(1부터 시작) |
| `path` | `string` | `-` | - | 값을 가져올 컨트롤의 경로를 지정합니다.<br/>ex) "/node/node[3]/node/node[4]"<br/>(searchType이 path일 경우 사용) |
| `resourceId` | `string` | `-` | - | 값을 가져올 컨트롤 리소스 ID를 지정합니다.<br/>ex) "com.sec.android.app.launcher:id/hotseat_icon" |
| `searchType` | `string` | `"control"` | `"control"`, `"path"` | 검색할 타입을 지정합니다.<br/>"control": 컨트롤 정보로 검색합니다.<br/>"path": 경로로 검색합니다. |
| `text` | `string` | `-` | - | 값을 가져올 컨트롤의 텍스트를 지정합니다.<br/>ex) "설정" |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 대기할 최대 시간을 지정합니다.<br/>ex) 30000 (단위: 밀리세컨) |
| `value` | `string` | `"text"` | `"index"`, `"text"`, `"resource-id"`, `"class"`, `"package"`, `"content-desc"`, `"checkable"`, `"checked"`, `"clickable"`, `"enabled"`, `"focusable"`, `"focused"`, `"scrollable"`, `"long-clickable"`, `"password"`, `"selected"`, `"bounds"` | 가져올 value의 종류를 지정합니다.<br/>ex) "scrollable" |
| `waiting` | `string` | `True` | `True`, `False` | 컨트롤 대기 여부를 지정합니다.<br/>True: 컨트롤이 나타날 때까지 대기한다.<br/>False: 컨트롤이 나타날 때까지 대기하지 않는다. |

## Property Notes
### `contentDesc`
값을 가져올 컨트롤 설명을 지정합니다.
ex) "설정"

### `controlClass`
값을 가져올 컨트롤 클래스를 지정합니다.(필수 값)
ex) "android.widget.TextView"

### `nthControl`
정보가 동일한 컨트롤이 여러개 있을 경우 가져올 컨트롤의 인덱스를 지정합니다.
ex) 1
(1부터 시작)

### `path`
값을 가져올 컨트롤의 경로를 지정합니다.
ex) "/node/node[3]/node/node[4]"
(searchType이 path일 경우 사용)

### `resourceId`
값을 가져올 컨트롤 리소스 ID를 지정합니다.
ex) "com.sec.android.app.launcher:id/hotseat_icon"

### `searchType`
검색할 타입을 지정합니다.
"control": 컨트롤 정보로 검색합니다.
"path": 경로로 검색합니다.

### `text`
값을 가져올 컨트롤의 텍스트를 지정합니다.
ex) "설정"

### `timeout`
대기할 최대 시간을 지정합니다.
ex) 30000 (단위: 밀리세컨)

### `value`
가져올 value의 종류를 지정합니다.
ex) "scrollable"

### `waiting`
컨트롤 대기 여부를 지정합니다.
True: 컨트롤이 나타날 때까지 대기한다.
False: 컨트롤이 나타날 때까지 대기하지 않는다.

