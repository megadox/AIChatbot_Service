# Activity: MoveCursorPosition

## Summary
현재 커서의 위치를 이동하는 액티비티

## Metadata
- group: `WORD`
- script: `move_cursor_position()`
- pattern: `WORD\.move_cursor_position\(`
- dependencies: `WORD`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `count` | `string` | `1` | - | 커서의 이동 횟수를 지정합니다.<br/>ex) 5 |
| `direction` | `string` | `"right"` | `"right"`, `"left"` | 커서의 이동 방향을 지정합니다.<br/>ex) "right" |
| `unit` | `string` | `"character"` | `"character"`, `"word"`, `"sentence"` | 커서의 이동 방향을 지정합니다.<br/>"character": 글자 단위 이동<br/>"word": 단어 단위 이동<br/>"sentence": 문장 단위 이동 |

## Property Notes
### `count`
커서의 이동 횟수를 지정합니다.
ex) 5

### `direction`
커서의 이동 방향을 지정합니다.
ex) "right"

### `unit`
커서의 이동 방향을 지정합니다.
"character": 글자 단위 이동
"word": 단어 단위 이동
"sentence": 문장 단위 이동

