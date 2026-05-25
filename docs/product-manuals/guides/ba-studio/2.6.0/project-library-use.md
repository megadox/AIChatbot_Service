# BA-Studio 프로젝트에서 라이브러리 사용 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: project-library-use
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 2-5. Properties / Library / Images / Resource
- 사용자 매뉴얼 > 목차 > 3-6. 프로젝트에서 라이브러리 사용하기

## User Intent

사용자가 프로젝트에서 BA-Server에 등록된 라이브러리나 다운로드한 라이브러리 Task를 사용하는 방법을 알고 싶어 한다.

대표 질문:
- 프로젝트에서 라이브러리 사용법을 알려줘.
- Library Manager에서 받은 라이브러리를 어떻게 써?
- 라이브러리 Task를 프로젝트에 넣으려면?
- BA-Studio에서 공통 라이브러리를 프로젝트에 적용하는 방법은?

## Short Answer

프로젝트에서 라이브러리를 사용하려면 Library Manager에서 BA-Server의 라이브러리를 프로젝트로 다운로드한 뒤, Library Tree의 Task를 프로젝트 Task Editor로 Drag & Drop하여 Predefined Process 형태로 배치한다.

## Steps

1. 라이브러리를 사용할 프로젝트를 연다.
2. 메인 메뉴의 `Library > Download Library` 또는 Library 창의 Library Manager를 연다.
3. Server 목록에서 사용할 라이브러리를 찾는다.
4. 라이브러리를 현재 Project로 다운로드한다.
5. Project 쪽 Library 목록에 다운로드한 라이브러리가 표시되는지 확인한다.
6. Library Tree에서 사용할 Task를 찾는다.
7. 라이브러리 Task를 프로젝트의 Task Editor로 Drag & Drop한다.
8. 필요한 경우 해당 라이브러리 Task의 Input/Output 값을 Properties에서 설정한다.
9. 프로젝트를 실행해 라이브러리 Task가 의도한 위치에서 호출되는지 확인한다.

## Notes

- 라이브러리 Task는 프로젝트 안에서 Predefined Process 또는 Sub Task처럼 사용된다.
- 라이브러리 Task와 프로젝트 Task는 구분되어 표시될 수 있다.
- 라이브러리 Task에 Input/Output 변수가 있으면 호출하는 쪽에서 값을 연결해야 한다.
- 라이브러리는 공통 기능을 여러 프로젝트에서 재사용하기 위한 Task 묶음이다.

## Answer Style

목차를 나열하지 말고 `다운로드 -> Library Tree 확인 -> Drag & Drop -> Input/Output 설정` 순서로 답변한다.

## Related Keywords

- Library Manager
- Download Library
- Library Tree
- 라이브러리 사용
- Predefined Process
- Sub Task
- Input Output
- Drag Drop

## Example Answer

프로젝트에서 라이브러리를 사용하려면 먼저 Library Manager에서 BA-Server의 라이브러리를 현재 프로젝트로 다운로드합니다. 다운로드 후 Library Tree에서 사용할 Task를 프로젝트 Task Editor로 Drag & Drop하면 Predefined Process 형태로 배치됩니다. 라이브러리 Task가 Input/Output 변수를 사용한다면 Properties에서 값을 연결한 뒤 프로젝트를 실행해 확인하세요.
