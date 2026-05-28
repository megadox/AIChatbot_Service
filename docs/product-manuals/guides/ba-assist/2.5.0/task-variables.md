# BA-Assist 패키지 변수 편집 방법

- Product: BA-Assist
- Version: 2.5.0
- Topic: task-variables

## User Intent
사용자가 BA-Assist에서 패키지 변수 값을 어디서 읽고 저장하는지, Password 타입이 어떻게 표시되는지 알고 싶어 한다.

## Related Questions
- BA-Assist 패키지 변수는 어디에 저장돼?
- Variables.json은 어떤 역할이야?
- 패키지 변수 편집은 어떻게 동작해?
- Password 변수는 화면에 어떻게 보여?
- 글로벌 변수 값을 바꾸면 어디에 저장돼?

## Short Answer
BA-Assist의 패키지 변수 편집은 패키지를 임시 폴더에 압축 해제한 뒤 `Variables.json`을 읽어 글로벌 변수 목록을 표시하고, 저장 시 변경 값을 다시 `Variables.json`의 `globals` 배열에 기록하는 방식입니다. 변수 항목은 `type`, `key`, `value`를 가지며 Password 타입 값은 화면에서 마스킹됩니다.

## Steps
1. 변수 편집 대상 패키지를 선택한다.
2. BA-Assist가 패키지를 임시 폴더에 압축 해제한다.
3. 임시 폴더의 `Variables.json`을 읽어 글로벌 변수 목록을 만든다.
4. 필요한 변수 값을 수정한다.
5. Password 타입은 화면에서 마스킹되어 표시되는지 확인한다.
6. 저장하면 변경된 값이 `Variables.json`의 `globals` 배열에 기록된다.
7. 편집 완료 후 임시 폴더는 삭제된다.

## Notes
- 소스 확인 위치는 `UI/TaskVariables.cs`이다.
- 변수 항목은 `type`, `key`, `value` 구조를 가진다.
- 날짜, 숫자, 콤보, 텍스트, Password 타입에 따라 편집 UI가 달라질 수 있다.

## Related Keywords
- 패키지 변수
- TaskVariables
- Variables.json
- globals
- type
- key
- value
- Password
- 마스킹

## Example Answer
패키지 변수는 패키지 안의 `Variables.json`에 저장됩니다. BA-Assist는 패키지를 임시 폴더에 풀어 `Variables.json`의 `globals`를 읽고, 수정 후 다시 같은 파일에 저장합니다. Password 타입 값은 화면에서 마스킹되어 표시됩니다.
