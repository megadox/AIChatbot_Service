# Activity: ConnectDevice

## Summary
안드로이드 디바이스와 연결하는 액티비티

## Metadata
- group: `ANDROID`
- script: `ANDROID.android_connect_device()`
- pattern: `ANDROID\.android_connect_device\(`
- dependencies: `ANDROID`
- theme: `Accent6`
- prefix: `android`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `host` | `string` | `"localhost"` | - | 안드로이드 디바이스와 연결할 클라이언트 호스트를 지정합니다.<br/>ex) "127.0.0.1" |
| `port` | `string` | `5037` | - | 안드로이드 디바이스와 연결할 클라이언트 포트를 지정합니다.<br/>ex) 5037 |
| `serial` | `string` | `-` | - | 안드로이드 디바이스의 시리얼 넘버를 지정합니다.<br/>ex) "123AB67CD0" |
| `wakeup` | `string` | `True` | `True`, `False` | 안드로이드 디바이스를 깨우는 옵션을 지정합니다.<br/>True: 디바이스를 깨운다.<br/>False: 디바이스를 깨우지 않는다. |

## Property Notes
### `host`
안드로이드 디바이스와 연결할 클라이언트 호스트를 지정합니다.
ex) "127.0.0.1"

### `port`
안드로이드 디바이스와 연결할 클라이언트 포트를 지정합니다.
ex) 5037

### `serial`
안드로이드 디바이스의 시리얼 넘버를 지정합니다.
ex) "123AB67CD0"

### `wakeup`
안드로이드 디바이스를 깨우는 옵션을 지정합니다.
True: 디바이스를 깨운다.
False: 디바이스를 깨우지 않는다.

