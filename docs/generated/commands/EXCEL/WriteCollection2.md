# Activity: WriteCollection2

## Summary
특정 영역에 리스트 형태의 데이터를 쓰는 액티비티 (WriteCollection보다 속도가 향상된 액티비티)

## Metadata
- group: `EXCEL`
- script: `WriteCollection2()`
- pattern: `excel\.WriteCollection2\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `beginCol` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 컬렉션 데이터 작성을 시작할 열을 인덱스 방식으로 지정합니다.<br/>ex) 0 |
| `beginRow` | `string` | `0` | `0`, `1`, `2`, `3`, `4`, `5`, `6`, `7`, `8`, `9` | 컬렉션 데이터 작성을 시작할 행을 인덱스 방식으로 지정합니다.<br/>ex) 0 |
| `collection` | `string` | `-` | - | 엑셀 문서에 쓸 컬렉션 데이터를 지정합니다.<br/>ex) a_list |
| `forceText` | `string` | `False` | `True`, `False` | 텍스트 형식 사용 여부를 지정합니다.<br/>True: 텍스트 형식으로 작성한다.<br/>False: 입력된 형식 그대로 작성한다. |
| `keyColumn` | `string` | `-1` | - | 작성 기준 열을 지정합니다.<br/>ex) 0<br/>(값이 0보다 클 경우, 해당 인덱스의 열 마지막 행을 찾아 그 다음 행부터 컬렉션 데이터를 쓴다.) |

## Property Notes
### `beginCol`
컬렉션 데이터 작성을 시작할 열을 인덱스 방식으로 지정합니다.
ex) 0

### `beginRow`
컬렉션 데이터 작성을 시작할 행을 인덱스 방식으로 지정합니다.
ex) 0

### `collection`
엑셀 문서에 쓸 컬렉션 데이터를 지정합니다.
ex) a_list

### `forceText`
텍스트 형식 사용 여부를 지정합니다.
True: 텍스트 형식으로 작성한다.
False: 입력된 형식 그대로 작성한다.

### `keyColumn`
작성 기준 열을 지정합니다.
ex) 0
(값이 0보다 클 경우, 해당 인덱스의 열 마지막 행을 찾아 그 다음 행부터 컬렉션 데이터를 쓴다.)

