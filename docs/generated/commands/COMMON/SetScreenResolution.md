# Activity: SetScreenResolution

## Summary
현재 사용중인 모니터의 해상도를 변경하는 액티비티

## Metadata
- group: `COMMON`
- script: `ENVIRONMENT.change_screensize()`
- pattern: `ENVIRONMENT\.change_screensize\(`
- dependencies: `ENVIRONMENT`
- theme: `Dark_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `height` | `string` | `1080` | `1080`, `900`, `768`, `720`, `600`, `480` | 변경할 모니터의 세로 해상도를 지정합니다.<br/>(기기에서 지원하는 세로 해상도 값만 사용할 수 있습니다.)<br/>ex) 1080 |
| `toDefault` | `string` | `False` | `True`, `False` | 해상도를 기본값으로 변경할지 여부를 지정합니다.<br/>True: 기본값으로 변경한다.<br/>False: 기본값으로 변경하지 않는다.<br/>(True로 설정할 경우 width와 height의 값은 무시된다.) |
| `width` | `string` | `1920` | `1920`, `1600`, `1366`, `1280`, `1024`, `800` | 변경할 모니터의 가로 해상도를 지정합니다.<br/>(기기에서 지원하는 가로 해상도 값만 사용할 수 있습니다.)<br/>ex) 1920 |

## Property Notes
### `height`
변경할 모니터의 세로 해상도를 지정합니다.
(기기에서 지원하는 세로 해상도 값만 사용할 수 있습니다.)
ex) 1080

### `toDefault`
해상도를 기본값으로 변경할지 여부를 지정합니다.
True: 기본값으로 변경한다.
False: 기본값으로 변경하지 않는다.
(True로 설정할 경우 width와 height의 값은 무시된다.)

### `width`
변경할 모니터의 가로 해상도를 지정합니다.
(기기에서 지원하는 가로 해상도 값만 사용할 수 있습니다.)
ex) 1920

