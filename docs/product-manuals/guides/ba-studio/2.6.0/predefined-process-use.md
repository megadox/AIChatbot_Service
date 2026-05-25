# BA-Studio Predefined Process 사용 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: predefined-process-use
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 3-2-4. Predefined Process 사용하기
- 사용자 매뉴얼 > 목차 > 3-6. 프로젝트에서 라이브러리 사용하기

## User Intent

사용자가 다른 Task나 라이브러리 Task를 현재 Task 안에서 호출하는 방법을 알고 싶어 한다.

대표 질문:
- Predefined Process 사용법을 알려줘.
- 다른 Task를 호출하려면?
- Sub Task를 Drag & Drop해서 쓰는 방법은?
- 라이브러리 Task를 현재 Task에서 실행하려면?

## Short Answer

Predefined Process는 프로젝트 안의 다른 Task 또는 라이브러리 Task를 현재 Task 흐름 안에서 호출할 때 사용한다. Project Tree나 Library Tree에서 호출할 Task를 Task Editor로 Drag & Drop하여 배치한다.

## Steps

1. 호출할 대상 Task가 프로젝트나 라이브러리에 준비되어 있는지 확인한다.
2. 현재 작업 중인 Task를 연다.
3. Project Tree 또는 Library Tree에서 호출할 Task를 찾는다.
4. 해당 Task를 Task Editor의 원하는 위치로 Drag & Drop한다.
5. 생성된 Predefined Process 항목을 선택한다.
6. 필요한 경우 Properties에서 Input/Output 값을 설정한다.
7. 실행하여 호출 순서와 반환 값이 의도대로 동작하는지 확인한다.

## Notes

- Predefined Process는 반복 사용되는 흐름을 별도 Task로 분리해 재사용할 때 유용하다.
- Input 변수는 호출되는 Task로 전달할 값이다.
- Output 변수는 호출된 Task에서 결과를 받아올 때 사용한다.
- 라이브러리 Task도 프로젝트 Task와 유사하게 Predefined Process 방식으로 사용할 수 있다.

## Answer Style

Drag & Drop과 Input/Output 연결을 핵심으로 설명한다.

## Related Keywords

- Predefined Process
- Sub Task
- Task 호출
- Library Task
- Input 변수
- Output 변수
- Drag Drop

## Example Answer

다른 Task를 호출하려면 Project Tree나 Library Tree에서 사용할 Task를 현재 Task Editor로 Drag & Drop합니다. 배치된 항목은 Predefined Process처럼 동작하며, 필요한 경우 Properties에서 Input/Output 값을 연결해 호출 Task와 데이터를 주고받을 수 있습니다.
