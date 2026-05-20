# Activity: RemoveDuplicates

## Summary
특정 영역의 중복 값을 제거하는 액티비티

## Metadata
- group: `EXCEL`
- script: `remove_duplicates()`
- pattern: `excel\.remove_duplicates\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `columns` | `string` | `None` | - | 중복 확인에 사용할 열을 리스트 형태(인덱스, 열 이름, 헤더명)로 지정합니다.<br/>ex) [0, 2, 3] 또는 ["A","B","C"] 또는 ["이름","나이","주소"]<br/>(값이 None 이거나 비어있는 경우 범위 내 '모든 열'을 기준으로 중복을 지운다.) |
| `header` | `string` | `False` | `True`, `False` | 첫 행을 헤더(머리글)로 처리할지 여부를 지정합니다.<br/>True: 첫 행을 헤더로 간주하여 중복 검사에서 제외합니다.<br/>False: 첫 행도 데이터로 간주하여 중복 검사에 포함합니다. |
| `range` | `string` | `-` | - | 중복을 지울 영역을 지정합니다.<br/>ex) "E2:E3" |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |

## Property Notes
### `columns`
중복 확인에 사용할 열을 리스트 형태(인덱스, 열 이름, 헤더명)로 지정합니다.
ex) [0, 2, 3] 또는 ["A","B","C"] 또는 ["이름","나이","주소"]
(값이 None 이거나 비어있는 경우 범위 내 '모든 열'을 기준으로 중복을 지운다.)

### `header`
첫 행을 헤더(머리글)로 처리할지 여부를 지정합니다.
True: 첫 행을 헤더로 간주하여 중복 검사에서 제외합니다.
False: 첫 행도 데이터로 간주하여 중복 검사에 포함합니다.

### `range`
중복을 지울 영역을 지정합니다.
ex) "E2:E3"

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

