# Activity: Today

## Summary
현재 날짜와 시간을 가져오는 액티비티

## Metadata
- group: `COMMON`
- script: `DATETIME.Today()`
- pattern: `DATETIME\.Today\(`
- dependencies: `DATETIME`
- theme: `Dark_5`
- prefix: `date`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `Day` | `string` | `0` | `-2`, `-1`, `0`, `1`, `2`, `3` | 현재 시각의 일에 대한 보정치(+/-)를 지정합니다.<br/>ex) -3 |
| `Format` | `string` | `"%Y-%m-%d %H:%M"` | - | 시간의 표시 형식을 지정합니다.<br/>ex) "%Y-%m-%d %H:%M"<br/>(파이썬 datetime 포맷을 지원한다.) |
| `Hour` | `string` | `0` | `-2`, `-1`, `0`, `1`, `2`, `3` | 현재 시각의 시간에 대한 보정치(+/-)를 지정합니다.<br/>ex) 0 |
| `Minute` | `string` | `0` | `-2`, `-1`, `0`, `1`, `2`, `3` | 현재 시각의 분에 대한 보정치(+/-)를 지정합니다.<br/>ex) 10 |
| `Month` | `string` | `0` | `-2`, `-1`, `0`, `1`, `2`, `3` | 현재 시각의 월에 대한 보정치(+/-)를 지정합니다.<br/>ex) 5 |
| `Year` | `string` | `0` | `-2`, `-1`, `0`, `1`, `2`, `3` | 현재 시각의 년에 대한 보정치(+/-)를 지정합니다.<br/>ex) -1 |

## Property Notes
### `Day`
현재 시각의 일에 대한 보정치(+/-)를 지정합니다.
ex) -3

### `Format`
시간의 표시 형식을 지정합니다.
ex) "%Y-%m-%d %H:%M"
(파이썬 datetime 포맷을 지원한다.)

### `Hour`
현재 시각의 시간에 대한 보정치(+/-)를 지정합니다.
ex) 0

### `Minute`
현재 시각의 분에 대한 보정치(+/-)를 지정합니다.
ex) 10

### `Month`
현재 시각의 월에 대한 보정치(+/-)를 지정합니다.
ex) 5

### `Year`
현재 시각의 년에 대한 보정치(+/-)를 지정합니다.
ex) -1

