# BA-Studio 변수 사용 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: variables-use
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 2-6. 개발자도구 > Variables창
- 사용자 매뉴얼 > 목차 > 3-2-2. 변수의 사용

## User Intent

사용자가 Local, Global, Input, Output 변수를 만들고 Activity 속성에서 사용하는 방법을 알고 싶어 한다.

대표 질문:
- 변수 사용법을 알려줘.
- Local 변수와 Global 변수 차이는?
- Input Output 변수는 언제 써?
- Activity 속성에 변수를 넣으려면?

## Short Answer

BA-Studio의 변수는 Variables 창에서 추가해 사용한다. Task 안에서만 쓰는 값은 Local, Task 간 공유 값은 Global, Predefined Process나 Library 호출 시 전달 값은 Input/Output 변수로 관리한다.

## Steps

1. 개발자도구의 `Variables` 창을 연다.
2. 사용할 변수 종류를 선택한다.
3. 변수 이름과 값을 입력한다.
4. Task 안에서만 사용할 값은 Local 변수로 만든다.
5. 여러 Task가 공유해야 하는 값은 Global 변수로 만든다.
6. 호출되는 Task로 값을 전달하려면 Input 변수를 사용한다.
7. 호출된 Task의 결과를 받으려면 Output 변수를 사용한다.
8. Activity 속성 또는 Code Editor에서 변수명을 참조해 사용한다.

## Notes

- 변수는 먼저 Variables 창에 선언한 뒤 사용하는 것이 안전하다.
- Input/Output 변수는 Predefined Process와 Library 사용 시 특히 중요하다.
- 변수 이름은 의미가 드러나게 작성하면 유지보수가 쉬워진다.

## Answer Style

변수 종류별 용도를 먼저 설명하고, 이후 생성 절차를 안내한다.

## Related Keywords

- Variables
- Local 변수
- Global 변수
- Input 변수
- Output 변수
- 변수 선언
- 변수 참조

## Example Answer

변수는 `Variables` 창에서 추가합니다. 현재 Task 안에서만 쓸 값은 Local, 여러 Task에서 공유할 값은 Global로 만들고, Predefined Process나 Library Task에 값을 전달하거나 결과를 받을 때는 Input/Output 변수를 사용합니다.
