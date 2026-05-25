# BA-Studio WEB Selector 사용 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: web-selector-use
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 3-2-1. 태스크 구현의 주요방법 > Object/Browser 인식기반으로 태스크 작성하기
- 사용자 매뉴얼 > 목차 > 2-8. 설정 > Add On

## User Intent

사용자가 웹 화면의 버튼, 입력창, 링크 같은 브라우저 객체를 Selector로 선택해 WEB Activity에 사용하는 방법을 알고 싶어 한다.

대표 질문:
- WEB Selector 사용법을 알려줘.
- 브라우저 객체를 선택하려면?
- Selector Editor는 어디서 열어?
- WEB Click에 셀렉터를 넣는 방법은?

## Short Answer

WEB Selector를 사용하려면 브라우저 확장 프로그램을 설치한 뒤 Selector Editor를 열고, 브라우저 화면에서 대상 객체를 선택해 WEB Activity의 Selector 속성에 연결한다.

## Steps

1. BA-Studio 설정에서 Chrome 또는 Edge 확장 프로그램을 설치한다.
2. 브라우저를 재실행한다.
3. Task Editor에 WEB Activity를 배치한다.
4. Selector Editor를 연다.
5. 대상 브라우저 화면을 준비한다.
6. Selector Editor에서 셀렉터 추가를 실행한다.
7. 브라우저의 대상 객체 위에서 단축키를 사용해 객체를 선택한다.
8. 선택된 객체의 Type이 WEB인지 확인한다.
9. `셀렉터 선택`을 눌러 Activity의 Selector 속성에 반영한다.
10. Task를 실행해 대상 객체가 정상 인식되는지 확인한다.

## Notes

- WEB Selector는 WEB 그룹 Activity에서 사용한다.
- 브라우저 확장 프로그램이 설치되어 있어야 안정적으로 객체를 선택할 수 있다.
- Selector가 깨지면 Selector Editor에서 다시 선택하거나 조건을 조정한다.

## Answer Style

확장 프로그램 설치 여부, Selector Editor 열기, 객체 선택, Activity 속성 반영 순서로 답변한다.

## Related Keywords

- WEB Selector
- Selector Editor
- Chrome 확장 프로그램
- Edge 확장 프로그램
- WEB Click
- WEB Wait
- 브라우저 객체

## Example Answer

WEB Selector를 쓰려면 먼저 Chrome 또는 Edge 확장 프로그램을 설치하고 브라우저를 다시 실행합니다. 그 다음 Selector Editor를 열어 브라우저의 대상 객체를 선택하고, 선택된 Selector를 WEB Activity의 Selector 속성에 넣어 사용합니다.
