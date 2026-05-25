# BA-Studio 프로젝트 패키지 내보내기 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: package-export
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 2-1. 메인 메뉴 > Project
- 사용자 매뉴얼 > 목차 > 2-3. Project 창

## User Intent

사용자가 프로젝트나 Task를 패키지 파일로 내보내는 방법을 알고 싶어 한다.

대표 질문:
- 프로젝트를 패키지로 내보내는 방법은?
- Export Project는 어떻게 써?
- Task를 fpx로 만들려면?
- BA-Assist에서 실행할 패키지를 만들고 싶어.

## Short Answer

프로젝트를 패키지로 내보내려면 프로젝트를 연 뒤 Project 메뉴 또는 Project Tree 팝업 메뉴에서 `Export Project`를 선택한다. 단일 Task를 내보낼 때는 `Export Task`를 사용한다.

## Steps

1. 내보낼 프로젝트를 연다.
2. 필요한 Task와 리소스가 모두 포함되어 있는지 확인한다.
3. 프로젝트 전체를 내보내려면 Project 메뉴에서 `Export Project`를 선택한다.
4. 또는 Project Tree에서 프로젝트를 우클릭하고 `Export Project`를 선택한다.
5. 특정 Task만 내보내려면 해당 Task를 선택하고 `Export Task`를 사용한다.
6. 저장 위치와 파일명을 지정한다.
7. 생성된 패키지 파일을 BA-Assist 등 실행 환경에서 사용할 수 있는지 확인한다.

## Notes

- 프로젝트 전체 패키징과 Task 단위 내보내기는 목적이 다르다.
- 패키징 전 Resource, Images, Library 의존성을 확인하는 것이 좋다.
- Trial 버전이나 권한에 따라 일부 메뉴가 제한될 수 있다.

## Answer Style

Export Project와 Export Task의 차이를 명확히 설명한다.

## Related Keywords

- Export Project
- Export Task
- 패키지
- fpx
- BA-Assist
- 프로젝트 내보내기

## Example Answer

프로젝트 전체를 패키지로 만들려면 프로젝트를 연 뒤 `Project > Export Project`를 선택합니다. 특정 Task만 내보내려면 해당 Task에서 `Export Task`를 사용합니다. 내보내기 전에 리소스와 라이브러리 의존성이 빠지지 않았는지 확인하세요.
