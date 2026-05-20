# Activity: FilteredRows

## Summary
필터된 행의 데이터를 리스트형태로 Name필드에 지정된 변수에 저장하는 액티비티

## Metadata
- group: `EXCEL`
- script: `FilteredRows()`
- pattern: `excel\.FilteredRows\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `collection`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `header` | `string` | `True` | `True`, `False` | 헤더행의 포함 여부를 지정합니다.<br/>True: 헤더를 포함하여 리스트형 자료로 변환한다.<br/>False: 헤더를 포함하지 않고 리스트형 자료로 변환한다. |
| `indexReturn` | `string` | `False` | `True`, `False` | 원래의 행 인덱스를 반환 여부를 지정합니다.<br/>True: 반환 리스트의 첫 번째 인자로 원래 행 인덱스 번호를 추가하여 반환한다.<br/>False: 원래 행 인덱스 번호를 반환하지 않는다. |

## Property Notes
### `header`
헤더행의 포함 여부를 지정합니다.
True: 헤더를 포함하여 리스트형 자료로 변환한다.
False: 헤더를 포함하지 않고 리스트형 자료로 변환한다.

### `indexReturn`
원래의 행 인덱스를 반환 여부를 지정합니다.
True: 반환 리스트의 첫 번째 인자로 원래 행 인덱스 번호를 추가하여 반환한다.
False: 원래 행 인덱스 번호를 반환하지 않는다.

