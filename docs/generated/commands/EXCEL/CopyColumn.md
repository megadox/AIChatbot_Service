# Activity: CopyColumn

## Summary
시트에서 특정 열을 복사하여 또 다른 열에 붙여넣기할 때 사용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `CopyColumn()`
- pattern: `excel\.CopyColumn\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `fromColumn` | `string` | `-` | - | 복사할 영역의 열의 값을 인덱스 방식으로 지정합니다.<br/>ex) 2 |
| `sheet` | `string` | `-` | - | 복사할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) 0<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |
| `toColumn` | `string` | `-` | - | 붙여넣기할 영역의 열의 값을 인덱스 방식으로 지정합니다.<br/>ex) 3 |

## Property Notes
### `fromColumn`
복사할 영역의 열의 값을 인덱스 방식으로 지정합니다.
ex) 2

### `sheet`
복사할 시트의 인덱스 또는 이름을 지정합니다.
ex) 0
(비어있는 경우 현재 활성화된 시트를 지정한다.)

### `toColumn`
붙여넣기할 영역의 열의 값을 인덱스 방식으로 지정합니다.
ex) 3

