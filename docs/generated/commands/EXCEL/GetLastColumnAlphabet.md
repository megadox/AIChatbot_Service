# Activity: GetLastColumnAlphabet

## Summary
특정 행의 마지막 열의 알파벳 값을 가져오는 액티비티(특정 행에 데이터가 없는 경우 None을 반환한다.)

## Metadata
- group: `EXCEL`
- script: `get_last_column_alphabet()`
- pattern: `excel\.get_last_column_alphabet\(`
- dependencies: `EXCEL`
- theme: `Accent6_5`
- prefix: `alphabet`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `keyRow` | `string` | `-1` | - | 마지막 열의 값을 가져올 행을 1행을 기준으로 인덱스 방식으로 지정합니다.<br/>(-1의 경우 사용된 범위에서 제일 마지막 열의 알파벳을 반환한다.)<br/>ex) 1 |
| `sheet` | `string` | `-` | - | 작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.<br/>ex) "Sheet 1"<br/>(비어있는 경우 현재 활성화된 시트를 지정한다.) |

## Property Notes
### `keyRow`
마지막 열의 값을 가져올 행을 1행을 기준으로 인덱스 방식으로 지정합니다.
(-1의 경우 사용된 범위에서 제일 마지막 열의 알파벳을 반환한다.)
ex) 1

### `sheet`
작업을 수행할 시트의 인덱스 또는 이름을 지정합니다.
ex) "Sheet 1"
(비어있는 경우 현재 활성화된 시트를 지정한다.)

