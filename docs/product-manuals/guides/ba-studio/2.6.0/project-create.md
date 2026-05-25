# BA-Studio 프로젝트 생성 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: project-create
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 3-3. Project 구현하기 > 3-3-1. Project 구현하기
- 사용자 매뉴얼 > 목차 > 02. User Interface 이해하기

## User Intent

사용자가 BA-Studio에서 새 프로젝트를 만드는 방법을 알고 싶어 한다.

대표 질문:
- 프로젝트 생성방법을 알려줘.
- BA-Studio에서 새 프로젝트는 어떻게 만들어?
- New Project는 어디에서 만들 수 있어?
- 프로젝트를 만들고 Task를 추가하는 순서를 알려줘.

## Short Answer

BA-Studio에서 프로젝트는 시작 페이지의 `New Project` 또는 메인 메뉴의 `File > New > New Project`에서 생성한다. 프로젝트 이름과 저장 경로를 지정하면 Project Tree에 기본 `Task1`이 생성되고, 이후 필요한 Task를 추가해 자동화 흐름을 구성한다.

## Steps

1. BA-Studio를 실행한다.
2. 시작 페이지에서 `New Project`를 선택한다.
3. 또는 메인 메뉴에서 `File > New > New Project`를 선택한다.
4. 프로젝트 이름을 입력한다.
5. 프로젝트를 저장할 경로를 지정한다.
6. 프로젝트 생성을 완료한다.
7. Project Tree에 기본 `Task1`이 생성되었는지 확인한다.
8. 추가 작업이 필요하면 Project Tree에서 프로젝트를 우클릭한 뒤 `Add Task`를 선택해 Task를 추가한다.
9. 각 Task의 역할이 구분되도록 Task를 우클릭해 `Rename`으로 이름을 변경한다.
10. 시작 Task를 변경해야 하면 시작할 Task를 우클릭하고 `Set Start Task`를 선택한다.

## Notes

- 프로젝트 생성 자체는 `New Project` 선택, 이름 입력, 저장 경로 지정까지가 핵심이다.
- 프로젝트를 생성하면 기본 Task인 `Task1`이 만들어진다.
- 여러 Task로 자동화 흐름을 나누면 프로젝트를 구조적으로 관리할 수 있다.
- `Start Task`는 프로젝트 실행 시 처음 시작되는 진입점이다.
- 최초 생성 시에는 보통 `Task1`이 Start Task로 설정된다.
- `Add Task`, `Rename`, `Set Start Task`는 프로젝트 생성 이후 자동화 흐름을 구성하는 작업이다.

## Answer Style

답변은 절차형으로 작성한다.

권장 답변 구조:

1. 한 문장 요약
2. 단계별 생성 절차
3. 생성 후 확인할 항목
4. 필요 시 Task 추가/이름 변경/Start Task 설정 안내

사용자에게 소스 코드 파일명이나 내부 구현 클래스명은 보여주지 않는다.

## Related Keywords

- 프로젝트 생성
- 새 프로젝트
- New Project
- File > New > New Project
- Project Tree
- Task1
- Add Task
- Rename
- Set Start Task
- Start Task

## Example Answer

BA-Studio에서 프로젝트를 생성하는 기본 절차는 다음과 같습니다.

1. BA-Studio를 실행합니다.
2. 시작 페이지에서 `New Project`를 선택하거나, 메인 메뉴에서 `File > New > New Project`를 선택합니다.
3. 프로젝트 이름과 저장 경로를 지정합니다.
4. 생성이 완료되면 Project Tree에 기본 `Task1`이 만들어집니다.
5. 작업을 더 나누려면 프로젝트를 우클릭한 뒤 `Add Task`로 Task를 추가합니다.
6. 필요한 경우 Task를 우클릭해 `Rename`으로 이름을 바꾸고, 시작할 Task를 `Set Start Task`로 지정합니다.

프로젝트 생성만 보면 1-3번이 핵심이고, 4번 이후는 생성된 프로젝트를 실제 자동화 흐름으로 구성하는 단계입니다.
