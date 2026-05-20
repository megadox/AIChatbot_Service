# Activity: MultiThread

## Summary
병렬 실행을 지원하는 액티비티

## Metadata
- group: `BuiltIn`
- script: `'MultiThread'`
- pattern: `-`
- dependencies: `__BuiltIn_Thread__`
- symbol: `thread`
- theme: `Accent3`
- prefix: `thread`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `breakOnError` | `string` | `False` | `True`, `False` | 스레드 실행 중 오류가 발생하면 모든 스레드를 중단할지 여부를 지정합니다.<br/>True: 스레드 실행 중 오류가 발생하면 모든 스레드를 중단합니다.<br/>False: 스레드 실행 중 오류가 발생해도 모든 스레드를 중단하지 않습니다. |
| `range` | `string` | `-` | - | 스레드별 반복 실행을 위한 리스트 객체를 지정합니다..<br/>ex) List |

## Property Notes
### `breakOnError`
스레드 실행 중 오류가 발생하면 모든 스레드를 중단할지 여부를 지정합니다.
True: 스레드 실행 중 오류가 발생하면 모든 스레드를 중단합니다.
False: 스레드 실행 중 오류가 발생해도 모든 스레드를 중단하지 않습니다.

### `range`
스레드별 반복 실행을 위한 리스트 객체를 지정합니다..
ex) List

