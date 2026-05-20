# Activity: GetSMS

## Summary
안드로이드 디바이스의 SMS를 가져오는 액티비티

## Metadata
- group: `ANDROID`
- script: `android_get_sms()`
- pattern: `device\.android_get_sms\(`
- dependencies: `ANDROID`
- theme: `Accent6`
- prefix: `sms`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `date` | `string` | `-` | - | SMS를 가져올 날짜를 지정합니다.(없으면 모든 날짜 검색)<br/>ex) "2019-01-01" (날짜 형식 : %Y-%m-%d) |
| `index` | `string` | `0` | `0`, `1`, `2`, `3`, `4` | 검색 결과가 여러개 있을 경우 SMS의 인덱스를 지정합니다.<br/>ex) 1<br/>(0부터 시작하며 날짜 순서대로 정렬됩니다.) |
| `number` | `string` | `-` | - | SMS를 가져올 번호를 지정합니다.(없으면 모든 번호 검색)<br/>ex) "01012345678" |

## Property Notes
### `date`
SMS를 가져올 날짜를 지정합니다.(없으면 모든 날짜 검색)
ex) "2019-01-01" (날짜 형식 : %Y-%m-%d)

### `index`
검색 결과가 여러개 있을 경우 SMS의 인덱스를 지정합니다.
ex) 1
(0부터 시작하며 날짜 순서대로 정렬됩니다.)

### `number`
SMS를 가져올 번호를 지정합니다.(없으면 모든 번호 검색)
ex) "01012345678"

