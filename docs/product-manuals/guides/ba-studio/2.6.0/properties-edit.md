# BA-Studio Activity Properties 설정 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: properties-edit
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 2-5. Properties / Library / Images / Resource
- 사용자 매뉴얼 > 목차 > 2-9. 프로퍼티 설정 창

## User Intent

사용자가 Activity의 속성 값을 입력하거나 Code Editor/Text Editor로 편집하는 방법을 알고 싶어 한다.

대표 질문:
- Activity 속성은 어디서 설정해?
- Properties 창 사용법을 알려줘.
- Code Editor로 속성을 입력하려면?
- Title, Description, Activation은 뭔가?

## Short Answer

Activity를 선택하면 Properties 창에서 해당 Activity의 속성을 설정할 수 있다. 긴 값이나 코드 형태의 값은 속성별 편집 버튼을 통해 Code Editor 또는 Text Editor에서 입력할 수 있다.

## Steps

1. Task Editor에서 설정할 Activity를 선택한다.
2. 오른쪽 또는 하단의 Properties 창을 연다.
3. 필수 속성 값을 입력한다.
4. `Title`에는 다이어그램에 표시할 이름을 입력한다.
5. `Description`에는 Activity 수행 내용을 기록한다.
6. `Activation`으로 실행 여부를 설정한다.
7. 긴 텍스트나 스크립트성 값은 편집 버튼을 눌러 Code Editor 또는 Text Editor에서 입력한다.
8. 입력 후 Task를 실행해 속성이 정상 반영되는지 확인한다.

## Notes

- `Activation=True`이면 실행 시 해당 Activity가 수행된다.
- `Activation=False`이면 실행 시 해당 Activity가 건너뛰어질 수 있다.
- 속성 값에는 Variables 창에서 선언한 변수를 참조할 수 있다.

## Answer Style

Activity 선택 후 Properties 창에서 설정한다는 기본 흐름을 먼저 설명한다.

## Related Keywords

- Properties
- 속성 설정
- Code Editor
- Text Editor
- Title
- Description
- Activation
- BreakPoint

## Example Answer

Activity 속성은 Task Editor에서 Activity를 선택한 뒤 Properties 창에서 설정합니다. 필수 입력값을 채우고, 긴 텍스트나 코드 형태의 값은 속성 편집 버튼을 눌러 Code Editor 또는 Text Editor에서 입력하면 됩니다.
