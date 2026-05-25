# BA-Studio Logs 및 Debug 창 확인 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: logs-debug-window
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 2-6. 개발자도구
- 사용자 매뉴얼 > 목차 > 2-8. 설정

## User Intent

사용자가 실행 로그, 디버그 콘솔, 변수 상태, 오류 목록을 확인하는 방법을 알고 싶어 한다.

대표 질문:
- 실행 로그는 어디서 봐?
- Debug 창에는 뭐가 보여?
- 변수 상태를 확인하려면?
- 로그 출력 설정은 어디서 해?

## Short Answer

실행 로그는 개발자도구의 `Logs` 창에서 확인하고, 디버그 실행 중 상태는 `Debug` 창의 Console, Frames, Variables, BreakPoints 영역에서 확인한다. 로그 출력 수준은 Settings의 General 옵션에서 조정할 수 있다.

## Steps

1. BA-Studio 하단 또는 개발자도구 영역을 확인한다.
2. 실행 기록은 `Logs` 창을 연다.
3. 디버그 실행 결과는 `Debug` 창의 Console을 확인한다.
4. 현재 실행 프레임은 Frames 영역에서 확인한다.
5. 현재 변수 값은 Variables 영역에서 확인한다.
6. 설정된 중단점은 BreakPoints 영역에서 확인한다.
7. 로그 출력이 부족하면 `Options > Setting > General`에서 로그 관련 설정을 확인한다.
8. 오류가 발생하면 오류목록 창에서 항목을 선택해 관련 Task나 Activity로 이동한다.

## Notes

- Logs 창은 Task 실행 중 기록되는 로그를 표시한다.
- Debug 창은 디버그 실행 중 상태 확인에 사용한다.
- 오류목록은 SubTask 참조 오류나 Activity 스크립트 오류 확인에 유용하다.

## Answer Style

Logs, Debug, 오류목록의 용도를 구분해서 답변한다.

## Related Keywords

- Logs 창
- Debug 창
- Console
- Frames
- Variables
- BreakPoints
- 오류목록
- Log print mode

## Example Answer

실행 로그는 개발자도구의 `Logs` 창에서 확인합니다. 디버그 실행 중에는 `Debug` 창에서 Console 출력, 현재 Frames, Variables, BreakPoints를 볼 수 있고, 오류가 있으면 오류목록 창에서 관련 Task나 Activity로 이동할 수 있습니다.
