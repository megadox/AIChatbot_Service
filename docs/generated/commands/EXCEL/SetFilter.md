# Activity: SetFilter

## Summary
특정 영역에 필터 기능을 적용하는 액티비티

## Metadata
- group: `EXCEL`
- script: `SetFilter()`
- pattern: `excel\.SetFilter\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `criteria1` | `string` | `-` | - | 필터의 조건1을 지정합니다.<br/>ex) "물건A" |
| `criteria2` | `string` | `-` | - | 필터의 조건2을 지정합니다.<br/>ex) "물건C" |
| `field` | `string` | `1` | - | 필터를 적용할 열의 번호를 지정합니다.<br/>ex) 1<br/>(range 필드를 통해 선택한 열의 순서로 지정된다.) |
| `operator` | `string` | `-` | `"AND"`, `"OR"`, `` | 조건의 연산자를 지정합니다.<br/>"AND": criteria1과 criteria2의 AND 연산 수행<br/>"OR": cirteria1과 criteria2의 OR연산 수행 |
| `range` | `string` | `-` | - | 필터 기능을 적용할 영역을 지정합니다.<br/>ex) "E2:E3" |

## Property Notes
### `criteria1`
필터의 조건1을 지정합니다.
ex) "물건A"

### `criteria2`
필터의 조건2을 지정합니다.
ex) "물건C"

### `field`
필터를 적용할 열의 번호를 지정합니다.
ex) 1
(range 필드를 통해 선택한 열의 순서로 지정된다.)

### `operator`
조건의 연산자를 지정합니다.
"AND": criteria1과 criteria2의 AND 연산 수행
"OR": cirteria1과 criteria2의 OR연산 수행

### `range`
필터 기능을 적용할 영역을 지정합니다.
ex) "E2:E3"

