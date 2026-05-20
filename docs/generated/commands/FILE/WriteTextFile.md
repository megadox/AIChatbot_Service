# Activity: WriteTextFile

## Summary
특정 파일에 텍스트를 쓰고 저장하는 액티비티

## Metadata
- group: `FILE`
- script: `FILE.WriteTextFile()`
- pattern: `FILE\.WriteTextFile`
- dependencies: `FILE`
- theme: `Accent5_5`
- prefix: `append`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `encoding` | `string` | `"utf-8"` | `"utf-8"`, `"cp949"`, `"utf-16"` | Encoding Type을 지정합니다.<br/>ex) "utf-8" |
| `path` | `string` | `-` | - | 텍스트를 저장할 파일의 저장 경로를 지정합니다.<br/>ex) "C:\temp\sample.txt" |
| `text` | `string` | `-` | - | 파일로 저장할 텍스트를 지정합니다.<br/>ex) "hello world" |

## Property Notes
### `encoding`
Encoding Type을 지정합니다.
ex) "utf-8"

### `path`
텍스트를 저장할 파일의 저장 경로를 지정합니다.
ex) "C:\temp\sample.txt"

### `text`
파일로 저장할 텍스트를 지정합니다.
ex) "hello world"

