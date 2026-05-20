# Activity: AppendToTextFile

## Summary
기존 생성된 파일의 텍스트에 이어서 새로운 텍스트를 붙여 저장하는 액티비티

## Metadata
- group: `FILE`
- script: `FILE.AppendToTextFile()`
- pattern: `FILE\.AppendToTextFile`
- dependencies: `FILE`
- theme: `Accent5_5`
- prefix: `append`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `newline` | `string` | `True` | `True`, `False` | 줄바꿈 수행 여부를 지정합니다.<br/>True: 줄 바꾼 후 텍스트를 덧붙인다.<br/>False: 마지막 줄과 같은 줄에 옆으로 덧붙인다 |
| `path` | `string` | `-` | - | 텍스트를 이어 붙일 파일의 경로를 지정합니다.<br/>ex) "C:\text\sample.txt" |
| `text` | `string` | `-` | - | 이어 붙일 텍스트를 지정합니다.<br/>ex) "hello" |

## Property Notes
### `newline`
줄바꿈 수행 여부를 지정합니다.
True: 줄 바꾼 후 텍스트를 덧붙인다.
False: 마지막 줄과 같은 줄에 옆으로 덧붙인다

### `path`
텍스트를 이어 붙일 파일의 경로를 지정합니다.
ex) "C:\text\sample.txt"

### `text`
이어 붙일 텍스트를 지정합니다.
ex) "hello"

