# Activity: ListToDataFrame

## Summary
리스트의 데이터를 데이터프레임으로 변환하는 액티비티

## Metadata
- group: `DATA`
- script: `DATA.list_to_dataframe()`
- pattern: `DATA\.list_to_dataframe\(`
- dependencies: `DATA`
- theme: `Accent2`
- prefix: `dataFrame`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `columns` | `string` | `None` | - | 데이터프레임으로 변환할 열 이름의 리스트를 지정합니다.<br/>None: 열 이름 사용하지 않음<br/>['A그룹', 'B그룹']: 열 이름 리스트 |
| `data` | `string` | `-` | - | 데이터프레임으로 변환할 리스트를 지정합니다.<br/>ex) [[1, 2], [3, 4]] |
| `index` | `string` | `None` | - | 데이터프레임으로 변환할 인덱스 열을 지정합니다.<br/>None: 행 이름 사용하지 않음<br/>['1행', '2행']: 행 이름 리스트 |

## Property Notes
### `columns`
데이터프레임으로 변환할 열 이름의 리스트를 지정합니다.
None: 열 이름 사용하지 않음
['A그룹', 'B그룹']: 열 이름 리스트

### `data`
데이터프레임으로 변환할 리스트를 지정합니다.
ex) [[1, 2], [3, 4]]

### `index`
데이터프레임으로 변환할 인덱스 열을 지정합니다.
None: 행 이름 사용하지 않음
['1행', '2행']: 행 이름 리스트

