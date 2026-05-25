# BA-Studio Windows Selector 사용 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: window-selector-use
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 3-2-1. 태스크 구현의 주요방법 > Object/Windows 인식 기반으로 태스크 작성하기

## User Intent

사용자가 Windows 애플리케이션의 입력창, 버튼, 컨트롤을 Selector로 선택해 WIN32 Activity에 사용하는 방법을 알고 싶어 한다.

대표 질문:
- 윈도우 객체 선택 방법을 알려줘.
- Notepad 입력창을 Selector로 잡으려면?
- WIN32 Selector 사용법은?
- Windows Object 기반 자동화는 어떻게 해?

## Short Answer

Windows 객체는 Selector Editor에서 추가한 뒤 대상 프로그램의 컨트롤을 선택해 WIN32 Activity의 Selector 속성에 연결한다. 선택된 객체 Type이 Windows 계열인지 확인하고 사용한다.

## Steps

1. 자동화할 Windows 프로그램을 실행한다.
2. Task Editor에 WIN32 Activity를 배치한다.
3. Selector Editor를 연다.
4. Selector 추가를 시작한다.
5. 대상 프로그램의 입력창, 버튼 등 컨트롤에 포커스를 둔다.
6. 지정된 단축키로 객체를 선택한다.
7. Selector Editor에 객체 정보가 표시되는지 확인한다.
8. `셀렉터 선택`을 눌러 Activity 속성에 반영한다.
9. 필요한 입력값이나 클릭 동작 속성을 설정한다.
10. Task를 실행해 대상 컨트롤이 정상 제어되는지 확인한다.

## Notes

- Windows 객체는 WIN32 그룹 Activity에서 사용한다.
- 화면 좌표보다 Selector 기반 방식이 UI 변경에 더 안정적일 수 있다.
- 프로그램 권한이나 UI 구조에 따라 객체 선택이 제한될 수 있다.

## Answer Style

대상 프로그램 실행, Selector 선택, WIN32 Activity 연결을 순서대로 안내한다.

## Related Keywords

- Windows Selector
- WIN32 Selector
- Selector Editor
- Notepad
- WIN32 Click
- WIN32 Type Keys
- Windows Object

## Example Answer

Windows 객체를 사용하려면 대상 프로그램을 실행한 뒤 Selector Editor에서 객체 추가를 시작합니다. 자동화할 입력창이나 버튼을 선택하고 `셀렉터 선택`을 누르면 WIN32 Activity의 Selector 속성에 연결해 사용할 수 있습니다.
