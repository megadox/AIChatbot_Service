# Activity: CreateDirectory

## Summary
특정 위치에 폴더를 생성하는 액티비티

## Metadata
- group: `FILE`
- script: `FILE.CreateDirectory()`
- pattern: `FILE\.CreateDirectory`
- dependencies: `FILE`
- theme: `Accent5_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `createAllDir` | `string` | `True` | `True`, `False` | 상위 디렉터리가 없을 경우 모두 생성할지 여부를 지정합니다. True로 설정하면 상위 디렉터리를 포함하여 경로의 모든 디렉터리를 생성합니다 |
| `path` | `string` | `-` | - | 생성할 폴더의 경로를 지정합니다.<br/>ex) "C:\makeDir" |

## Property Notes
### `createAllDir`
상위 디렉터리가 없을 경우 모두 생성할지 여부를 지정합니다. True로 설정하면 상위 디렉터리를 포함하여 경로의 모든 디렉터리를 생성합니다

### `path`
생성할 폴더의 경로를 지정합니다.
ex) "C:\makeDir"

