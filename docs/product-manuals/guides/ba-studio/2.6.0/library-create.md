# BA-Studio 라이브러리 생성 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: library-create
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 2-1. 메인 메뉴
- 사용자 매뉴얼 > 목차 > 3-5. 라이브러리 구현하기

## User Intent

사용자가 재사용 가능한 라이브러리를 새로 만들거나 Library 프로젝트를 구성하는 방법을 알고 싶어 한다.

대표 질문:
- 라이브러리 생성 방법을 알려줘.
- New Library는 어디서 만들어?
- 공통 Task 라이브러리를 만들고 싶어.
- BA-Studio에서 Library는 어떻게 작성해?

## Short Answer

라이브러리는 시작 페이지의 `New Library` 또는 메인 메뉴의 `File > New > New Library`에서 생성한다. 생성 후 Library Tree에서 Task를 구성하고, 공통 기능 단위로 작성해 프로젝트에서 재사용할 수 있다.

## Steps

1. BA-Studio를 실행한다.
2. 시작 페이지에서 `New Library`를 선택한다.
3. 또는 메인 메뉴에서 `File > New > New Library`를 선택한다.
4. 라이브러리 이름과 저장 경로를 지정한다.
5. Library Tree에서 필요한 Task를 추가하고 이름을 정리한다.
6. 각 Task에 공통으로 사용할 자동화 흐름을 작성한다.
7. 필요한 경우 Input/Output 변수를 정의해 프로젝트에서 값을 주고받을 수 있게 한다.
8. 라이브러리를 저장하고 프로젝트에서 다운로드 또는 참조해 사용한다.

## Notes

- 라이브러리는 공통 기능을 모아 여러 프로젝트에서 재사용하기 위한 Task 묶음이다.
- Library에는 프로젝트의 `Start Task`와 같은 개념이 없을 수 있다.
- 라이브러리 Task는 프로젝트에서 Predefined Process 방식으로 사용할 수 있다.

## Answer Style

프로젝트 생성과 라이브러리 생성을 비교하되, 라이브러리의 재사용 목적을 강조한다.

## Related Keywords

- New Library
- File > New > New Library
- Library 생성
- Library Tree
- 공통 Task
- 재사용
- Input Output

## Example Answer

라이브러리를 만들려면 시작 페이지의 `New Library` 또는 `File > New > New Library`를 선택합니다. 이름과 저장 경로를 지정한 뒤 Library Tree에서 공통 기능 Task를 작성하고, 필요한 경우 Input/Output 변수를 정의해 프로젝트에서 재사용할 수 있게 구성합니다.
