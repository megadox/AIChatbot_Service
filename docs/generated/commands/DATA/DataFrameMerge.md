# Activity: DataFrameMerge

## Summary
두 데이터프레임의 데이터를 병합하여 Name필드에 설정된 변수에 저장하는 액티비티

## Metadata
- group: `DATA`
- script: `DATA.dataframe_merge()`
- pattern: `DATA\.dataframe_merge\(`
- dependencies: `DATA`
- theme: `Accent2`
- prefix: `dataFrame`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `mergeMode` | `string` | `-` | - | source1, source2의 컬럼이 일치하지 않을때 처리 모드. <br/>Add : source2에 컬럼 추가 <br/>Ignore : source2에 없는 컬럼은 무시 <br/>Error : Column 정보가 일치하지 않는 경우 Error 발생 |
| `source1` | `string` | `-` | - | 병합 할 데이터프레임 1 |
| `source2` | `string` | `-` | - | 병합 할 데이터프레임 2 |

## Property Notes
### `mergeMode`
source1, source2의 컬럼이 일치하지 않을때 처리 모드. 
Add : source2에 컬럼 추가 
Ignore : source2에 없는 컬럼은 무시 
Error : Column 정보가 일치하지 않는 경우 Error 발생

### `source1`
병합 할 데이터프레임 1

### `source2`
병합 할 데이터프레임 2

