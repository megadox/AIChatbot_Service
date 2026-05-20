# Activity: ReadText

## Summary
특정 파일의 텍스트를 읽고 Name필드에 지정된 변수에 저장하는 액티비티

## Metadata
- group: `FILE`
- script: `FILE.ReadAllTextFromFile()`
- pattern: `= FILE\.ReadAllTextFromFile`
- dependencies: `FILE`
- theme: `Accent5_5`
- prefix: `text`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `encoding` | `string` | `"utf-8"` | `"utf-8"`, `"cp949"`, `"utf-16"` | Encoding Type을 지정합니다.<br/>ex) "utf-8" |
| `path` | `string` | `-` | - | 읽을 파일의 경로를 지정합니다.<br/>ex) "C:\temp\sample.txt" |

## Property Notes
### `encoding`
Encoding Type을 지정합니다.
ex) "utf-8"

### `path`
읽을 파일의 경로를 지정합니다.
ex) "C:\temp\sample.txt"

