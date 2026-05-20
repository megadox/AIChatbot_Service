# Activity: GetControls

## Summary
조건에 해당하는 모든 윈도우 애플리케이션의 오브젝트(컨트롤)들을 반환하는 액티비티

## Metadata
- group: `WIN32`
- script: `MSAA.get_controls()`
- pattern: `msaa\.get_controls\(`
- dependencies: `MSAA`
- theme: `Accent3_5`
- prefix: `collection`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `controlClassName` | `string` | `-` | - | 반환할 윈도우 객체의 클래스 이름을 지정합니다.<br/>ex) "Group"<br/>(찾을 객체 클래스 이름에 포함된 단어 또는 클래스 이름 전체를 사용할 수 있습니다.) |
| `controlName` | `string` | `-` | - | 반환할 윈도우 객체의 이름을 지정합니다.<br/>ex) "메모장"<br/>(찾을 객체 이름에 포함된 단어 또는 이름 전체를 사용할 수 있습니다.) |
| `controlType` | `string` | `-` | - | 반환할 윈도우 객체의 컨트롤 타입을 지정합니다.<br/>ex) "WindowControl"<br/>(필수 값입니다.) |

## Property Notes
### `controlClassName`
반환할 윈도우 객체의 클래스 이름을 지정합니다.
ex) "Group"
(찾을 객체 클래스 이름에 포함된 단어 또는 클래스 이름 전체를 사용할 수 있습니다.)

### `controlName`
반환할 윈도우 객체의 이름을 지정합니다.
ex) "메모장"
(찾을 객체 이름에 포함된 단어 또는 이름 전체를 사용할 수 있습니다.)

### `controlType`
반환할 윈도우 객체의 컨트롤 타입을 지정합니다.
ex) "WindowControl"
(필수 값입니다.)

