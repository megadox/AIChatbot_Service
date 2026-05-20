# Activity: SetSelection

## Summary
MS워드 문서에서 선택영역을 지정하는 액티비티

## Metadata
- group: `WORD`
- script: `set_selection()`
- pattern: `WORD\.set_selection\(`
- dependencies: `WORD`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `end` | `string` | `9999` | - | 선택 영역의 끝이 되는 글자 수 위치를 지정합니다.<br/>ex) 100<br/>(맨 앞은 0이며, 공백도 1개의 글자로 취급한다.) |
| `start` | `string` | `0` | - | 선택 영역의 시작이 되는 글자 수 위치를 지정합니다.<br/>ex) 0<br/>(맨 앞은 0이며, 공백도 1개의 글자로 취급한다.) |

## Property Notes
### `end`
선택 영역의 끝이 되는 글자 수 위치를 지정합니다.
ex) 100
(맨 앞은 0이며, 공백도 1개의 글자로 취급한다.)

### `start`
선택 영역의 시작이 되는 글자 수 위치를 지정합니다.
ex) 0
(맨 앞은 0이며, 공백도 1개의 글자로 취급한다.)

