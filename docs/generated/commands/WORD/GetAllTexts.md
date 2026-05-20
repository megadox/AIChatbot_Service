# Activity: GetAllTexts

## Summary
MS워드 문서에서 텍스트와 표의 내부 데이터를 추출하여 Name필드에 지정된 변수에 리스트형태로 저장하는 액티비티

## Metadata
- group: `WORD`
- script: `get_all_text_as_list()`
- pattern: `WORD\.get_all_text_as_list\(`
- dependencies: `WORD`
- prefix: `collection`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `removeNonPrint` | `string` | `True` | `True`, `False` | 출력에서는 보이지 않는 기호를 제거할지 여부를 지정합니다.<br/>True: 제거한다.<br/>False: 제거하지 않는다. |

## Property Notes
### `removeNonPrint`
출력에서는 보이지 않는 기호를 제거할지 여부를 지정합니다.
True: 제거한다.
False: 제거하지 않는다.

