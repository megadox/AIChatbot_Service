# BA-Studio BreakPoint 설정 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: breakpoint-use
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 2-6. 개발자도구 > Debug 창
- 사용자 매뉴얼 > 목차 > 4-2. Group별 Activities

## User Intent

사용자가 특정 Activity에서 실행을 멈추고 변수나 상태를 확인하는 방법을 알고 싶어 한다.

대표 질문:
- BreakPoint 설정 방법을 알려줘.
- 특정 액티비티에서 멈추려면?
- 디버그 중간에 멈춰서 변수 확인하는 방법은?
- BreakPoint 옵션은 어디에 있어?

## Short Answer

멈추고 싶은 Activity의 `BreakPoint` 속성을 `True`로 설정하면 디버그 실행 중 해당 Activity 지점에서 일시 정지된다. 이후 Debug 창에서 변수와 실행 프레임을 확인할 수 있다.

## Steps

1. Task Editor에서 멈추고 싶은 Activity를 선택한다.
2. Properties 창을 연다.
3. `BreakPoint` 속성을 찾는다.
4. 값을 `True`로 변경한다.
5. `Debug Task` 또는 `Debug Project`로 실행한다.
6. 해당 Activity에서 실행이 멈추면 Debug 창을 확인한다.
7. 변수 상태를 확인한 뒤 `Debug Continue` 또는 `Debug Step Over`로 진행한다.
8. 더 이상 멈추지 않으려면 `BreakPoint`를 `False`로 되돌린다.

## Notes

- BreakPoint는 일반 실행보다 디버그 실행에서 상태 확인용으로 사용한다.
- 여러 Activity에 BreakPoint를 설정할 수 있다.
- Debug 창의 BreakPoints 목록에서 설정된 중단점을 확인할 수 있다.

## Answer Style

Activity 속성에서 설정한다는 점과 디버그 실행이 필요하다는 점을 강조한다.

## Related Keywords

- BreakPoint
- 중단점
- Debug Continue
- Debug Step Over
- Variables
- Frames

## Example Answer

특정 Activity에서 실행을 멈추려면 해당 Activity를 선택하고 Properties의 `BreakPoint` 값을 `True`로 설정합니다. 이후 `Debug Task` 또는 `Debug Project`로 실행하면 그 지점에서 멈추며, Debug 창에서 변수와 실행 상태를 확인할 수 있습니다.
