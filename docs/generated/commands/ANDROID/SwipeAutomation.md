# Activity: SwipeAutomation

## Summary
안드로이드 디바이스의 컨트롤을 스와이프하는 액티비티

## Metadata
- group: `ANDROID`
- script: `android_swipe_automation()`
- pattern: `device\.android_swipe_automation\(`
- dependencies: `ANDROID`
- theme: `Accent6`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `duration` | `string` | `1000` | `1000`, `2000`, `3000`, `4000`, `5000` | 스와이프하는 시간을 지정합니다.<br/>ex) 2000 |
| `endContentDesc` | `string` | `-` | - | 스와이프할 끝 컨트롤 설명을 지정합니다.<br/>ex) "설정" |
| `endControlClass` | `string` | `-` | - | 스와이프할 끝 컨트롤 클래스를 지정합니다.(필수 값)<br/>ex) "android.widget.TextView" |
| `endNthControl` | `string` | `1` | `1`, `2`, `3`, `4`, `5` | 정보가 동일한 컨트롤이 여러개 있을 경우 스와이프할 끝 컨트롤의 인덱스를 지정합니다.<br/>ex) 1<br/>(1부터 시작) |
| `endPath` | `string` | `-` | - | 스와이프할 끝 컨트롤의 경로를 지정합니다.<br/>ex) "/node/node[3]/node/node[4]"<br/>(endSearchType이 path일 경우 사용) |
| `endResourceId` | `string` | `-` | - | 스와이프할 끝 컨트롤 리소스 ID를 지정합니다.<br/>ex) "com.sec.android.app.launcher:id/hotseat_icon" |
| `endSearchType` | `string` | `"control"` | `"control"`, `"path"` | 끝 컨트롤 검색 타입을 지정합니다.<br/>"control": 컨트롤 정보로 검색합니다.<br/>"path": 경로로 검색합니다. |
| `endText` | `string` | `-` | - | 스와이프할 끝 컨트롤의 텍스트를 지정합니다.<br/>ex) "설정" |
| `startContentDesc` | `string` | `-` | - | 스와이프할 시작 컨트롤 설명을 지정합니다.<br/>ex) "전화" |
| `startControlClass` | `string` | `-` | - | 스와이프할 시작 컨트롤 클래스를 지정합니다.(필수 값)<br/>ex) "android.widget.TextView" |
| `startNthControl` | `string` | `1` | `1`, `2`, `3`, `4`, `5` | 정보가 동일한 컨트롤이 여러개 있을 경우 스와이프할 시작 컨트롤의 인덱스를 지정합니다.<br/>ex) 1<br/>(1부터 시작) |
| `startPath` | `string` | `-` | - | 시작 컨트롤의 경로를 지정합니다.<br/>ex) "/node/node[3]/node/node[1]"<br/>(startSearchType이 path일 경우 사용) |
| `startResourceId` | `string` | `-` | - | 스와이프할 시작 컨트롤 리소스 ID를 지정합니다.<br/>ex) "com.sec.android.app.launcher:id/hotseat_icon" |
| `startSearchType` | `string` | `"control"` | `"control"`, `"path"` | 시작 컨트롤 검색 타입을 지정합니다.<br/>"control": 컨트롤 정보로 검색합니다.<br/>"path": 경로로 검색합니다. |
| `startText` | `string` | `-` | - | 스와이프할 시작 컨트롤의 텍스트를 지정합니다.<br/>ex) "전화" |
| `timeout` | `string` | `60000` | `30000`, `60000`, `120000`, `300000` | 대기할 최대 시간을 지정합니다.(시작, 끝 컨트롤 모두 적용)<br/>ex) 30000 (단위: 밀리세컨) |
| `waiting` | `string` | `True` | `True`, `False` | 컨트롤 대기 여부를 지정합니다.(시작, 끝 컨트롤 모두 적용)<br/>True: 컨트롤이 나타날 때까지 대기한다.<br/>False: 컨트롤이 나타날 때까지 대기하지 않는다. |

## Property Notes
### `duration`
스와이프하는 시간을 지정합니다.
ex) 2000

### `endContentDesc`
스와이프할 끝 컨트롤 설명을 지정합니다.
ex) "설정"

### `endControlClass`
스와이프할 끝 컨트롤 클래스를 지정합니다.(필수 값)
ex) "android.widget.TextView"

### `endNthControl`
정보가 동일한 컨트롤이 여러개 있을 경우 스와이프할 끝 컨트롤의 인덱스를 지정합니다.
ex) 1
(1부터 시작)

### `endPath`
스와이프할 끝 컨트롤의 경로를 지정합니다.
ex) "/node/node[3]/node/node[4]"
(endSearchType이 path일 경우 사용)

### `endResourceId`
스와이프할 끝 컨트롤 리소스 ID를 지정합니다.
ex) "com.sec.android.app.launcher:id/hotseat_icon"

### `endSearchType`
끝 컨트롤 검색 타입을 지정합니다.
"control": 컨트롤 정보로 검색합니다.
"path": 경로로 검색합니다.

### `endText`
스와이프할 끝 컨트롤의 텍스트를 지정합니다.
ex) "설정"

### `startContentDesc`
스와이프할 시작 컨트롤 설명을 지정합니다.
ex) "전화"

### `startControlClass`
스와이프할 시작 컨트롤 클래스를 지정합니다.(필수 값)
ex) "android.widget.TextView"

### `startNthControl`
정보가 동일한 컨트롤이 여러개 있을 경우 스와이프할 시작 컨트롤의 인덱스를 지정합니다.
ex) 1
(1부터 시작)

### `startPath`
시작 컨트롤의 경로를 지정합니다.
ex) "/node/node[3]/node/node[1]"
(startSearchType이 path일 경우 사용)

### `startResourceId`
스와이프할 시작 컨트롤 리소스 ID를 지정합니다.
ex) "com.sec.android.app.launcher:id/hotseat_icon"

### `startSearchType`
시작 컨트롤 검색 타입을 지정합니다.
"control": 컨트롤 정보로 검색합니다.
"path": 경로로 검색합니다.

### `startText`
스와이프할 시작 컨트롤의 텍스트를 지정합니다.
ex) "전화"

### `timeout`
대기할 최대 시간을 지정합니다.(시작, 끝 컨트롤 모두 적용)
ex) 30000 (단위: 밀리세컨)

### `waiting`
컨트롤 대기 여부를 지정합니다.(시작, 끝 컨트롤 모두 적용)
True: 컨트롤이 나타날 때까지 대기한다.
False: 컨트롤이 나타날 때까지 대기하지 않는다.

