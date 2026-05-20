# Activity: GetTexts

## Summary
지정 경로의 MS워드 파일로부터 텍스트와 표의 내부 데이터를 추출하여 Name필드에 지정된 변수에 리스트 형태로 저장하는 액티비티

## Metadata
- group: `WORD`
- script: `WORD.get_all_text()`
- pattern: `WORD\.get_all_as_text\(`
- dependencies: `WORD`
- prefix: `text`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `path` | `string` | `-` | - | 데이터를 추출할 워드 파일의 경로를 지정합니다.<br/>ex) "C:\msword\sample.docx" |
| `removeNonPrint` | `string` | `True` | `True`, `False` | 출력에서는 보이지 않는 기호를 제거할지 여부를 지정합니다.<br/>True: 제거한다.<br/>False: 제거하지 않는다. |

## Property Notes
### `path`
데이터를 추출할 워드 파일의 경로를 지정합니다.
ex) "C:\msword\sample.docx"

### `removeNonPrint`
출력에서는 보이지 않는 기호를 제거할지 여부를 지정합니다.
True: 제거한다.
False: 제거하지 않는다.

