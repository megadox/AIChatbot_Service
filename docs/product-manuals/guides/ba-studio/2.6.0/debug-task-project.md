# BA-Studio 디버깅 실행 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: debug-task-project
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 2-1. 메인 메뉴 > Debug
- 사용자 매뉴얼 > 목차 > 2-2. 툴바
- 사용자 매뉴얼 > 목차 > 3-4. Debugging 사용하기

## User Intent

사용자가 Task 또는 Project를 디버그 모드로 실행하고 한 단계씩 확인하는 방법을 알고 싶어 한다.

대표 질문:
- 디버깅 사용법을 알려줘.
- Debug Task와 Debug Project 차이는?
- 한 단계씩 실행하려면?
- 디버그 중 계속 실행하려면?

## Short Answer

현재 Task는 `Debug Task`, 프로젝트 전체는 `Debug Project`로 디버그 실행한다. 디버그 중에는 `Debug Continue`, `Debug Step Over`, `Debug Stop`을 사용해 계속 실행, 한 단계 실행, 중지를 제어한다.

## Steps

1. 디버그할 Task 또는 Project를 연다.
2. 현재 Task만 확인하려면 `Debug Task`를 선택한다.
3. 프로젝트 전체 흐름을 확인하려면 `Debug Project`를 선택한다.
4. 중단점이 필요한 Activity에는 BreakPoint를 설정한다.
5. 디버그 실행 중 다음 단계만 진행하려면 `Debug Step Over`를 사용한다.
6. 다음 중단점까지 계속 실행하려면 `Debug Continue`를 사용한다.
7. 디버깅을 종료하려면 `Debug Stop`을 사용한다.
8. Debug 창의 Console, Frames, Variables, BreakPoints 정보를 확인한다.

## Notes

- Debug Task는 현재 Task 기준으로 확인할 때 사용한다.
- Debug Project는 Start Task부터 프로젝트 흐름을 확인할 때 사용한다.
- BreakPoint를 설정하면 해당 Activity 실행 지점에서 흐름을 멈춰 상태를 확인할 수 있다.

## Answer Style

디버그 실행 대상과 제어 버튼의 역할을 구분해서 설명한다.

## Related Keywords

- Debug Task
- Debug Project
- Debug Continue
- Debug Step Over
- Debug Stop
- BreakPoint
- Debug 창

## Example Answer

현재 Task만 디버깅하려면 `Debug Task`, 프로젝트 전체 흐름을 확인하려면 `Debug Project`를 사용합니다. 실행 중 한 단계씩 보려면 `Debug Step Over`, 계속 진행하려면 `Debug Continue`, 중지하려면 `Debug Stop`을 사용하세요.
