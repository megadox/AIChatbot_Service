# Activity: WriteDataFrame

## Summary
현재 활성화된 시트 특정 영역에 데이터프레임 형태의 데이터를 쓰는 액티비티

## Metadata
- group: `EXCEL`
- script: `WriteDataFrame()`
- pattern: `excel\.WriteDataFrame\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `beginCol` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 데이터프레임을 엑셀에 작성할 시작 열을 지정합니다.<br/>ex) 1 |
| `beginRow` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5` | 데이터프레임을 엑셀에 작성할 시작 행을 지정합니다.<br/>ex) 1 |
| `data` | `string` | `-` | - | 현재 활성화된 엑셀 시트에 작성할 데이터프레임을 지정합니다.<br/>ex) DataFrame_1 |
| `header` | `string` | `True` | `True`, `False` | 데이터프레임의 열 이름을 포함하여 엑셀에 작성할지 여부를 지정합니다.<br/>True: 행 이름 포함<br/>False: 열 이름 포함하지 않음 |
| `index` | `string` | `True` | `True`, `False` | 데이터프레임의 행 이름을 포함하여 엑셀에 작성할지 여부를 지정합니다.<br/>True: 열 이름 포함<br/>False: 행 이름 포함하지 않음 |
| `mergeCells` | `string` | `True` | `True`, `False` | 데이터프레임의 멀티인덱스를 병합된 셀로 작성할지 여부를 지정합니다.<br/>True: 셀 병합 표시<br/>False: 셀 병합 표시하지 않음 |
| `nanRep` | `string` | `-` | - | 데이터프레임의 결측치(NaN)를 Excel에서 어떤 값으로 표현할지 지정합니다.<br/>ex) 0 |

## Property Notes
### `beginCol`
데이터프레임을 엑셀에 작성할 시작 열을 지정합니다.
ex) 1

### `beginRow`
데이터프레임을 엑셀에 작성할 시작 행을 지정합니다.
ex) 1

### `data`
현재 활성화된 엑셀 시트에 작성할 데이터프레임을 지정합니다.
ex) DataFrame_1

### `header`
데이터프레임의 열 이름을 포함하여 엑셀에 작성할지 여부를 지정합니다.
True: 행 이름 포함
False: 열 이름 포함하지 않음

### `index`
데이터프레임의 행 이름을 포함하여 엑셀에 작성할지 여부를 지정합니다.
True: 열 이름 포함
False: 행 이름 포함하지 않음

### `mergeCells`
데이터프레임의 멀티인덱스를 병합된 셀로 작성할지 여부를 지정합니다.
True: 셀 병합 표시
False: 셀 병합 표시하지 않음

### `nanRep`
데이터프레임의 결측치(NaN)를 Excel에서 어떤 값으로 표현할지 지정합니다.
ex) 0

