## Improve Question Handling Guide

### Next Question
- 다른것은?
- 비슷한것은?
- A와 차이는?
- A와 다른점은?
- A와 B를 비교해줘.
- 함께 사용할 수 있는 것은?
- 같은 기능을 하는 것은?


### Expected Action
- 다른것은? : 이전 답변과 비슷한 기능이 있는 것 추천, 같은 그룹, 이전 답변을 제외 후 다음 후보
- 비슷한것은? : 이전 답변과 비슷한 기능이 있는 것 추천, 같은 그룹, 이전 답변을 제외 후 다음 후보
- A와 차이는? : 이전 답변과 A와 비교한다.
- A와 다른점은? : 이전 답변과 A와 비교한다.
- A와 B를 비교해줘. : A와 B를 비교한다.
- 함께 사용할 수 있는 것은? : 단독으로 사용할 수 없지만, 함께 사용될 수 있는 것을 추천한다.
- 같은 기능을 하는 것은? : 대체 기능을 하는 것을 추천한다. 같은 그룹, 다른 그룹에서도 찾을 수 있다.

### Ambiguous Cases
- 후보가 여러개면 ? : 바로 답하기
- 같은 목적의 액티비티기가 없으면 : 없다면 그냥 없다고 안내한다.
- 이전 대화 문맥 유지 : 기본은 계속 유지, 다만 질문 입력창 상단에 "문맥 유지" 체크박스를 생성해서 사용자가 문맥을 유지 할지 선택하게 한다. 체크 박스가 다시 설정된 시점부터 문맥은 유지된다.

### Regression Test
1) 이메일을 보내는
- EMAIL/SendMail(Outlook).md
- 다른 것은?
- EMAIL/SendMail(SMTP).md : 기대 답변은 EMAIL/SendMail(SMTP).md 이지만 후순위는 EMAIL/GetMail(Outlook).md 이다. 이 부분에 대한 보완이 필요하다.

2) 반복하는 액티비티는?
- BuiltIn/ForEach.md
- 비슷한 것은?
- BuiltIn/While.md : 기대 답변은 BuiltIn/While.md 이지만, 후순위는 BuiltIn/ForEach.md / Properties (1.746) 로 되어 있다. 결국 동일한 것인데, 별도인 것으로 취급되고 있다. 보완 필요하다, 그리고 다음에는  BuiltIn/DoWhile.md / Summary (0.906) 가 4순위로 되어 있는데, 이 역시 보완 필요하다.

3) A와 다른 점은?
- 반복하는 액티비티는?
- BuiltIn/ForEach.md
- While과 다른점은?
- 정의와 속성을 비교한다.

4) A와 B를 비교해줘
- TypeText와 TypeKey를 비교해줘
- TypeText와 TypeKey의 정의와 속성을 비교해서 다른점을 알려준다.

5) 같은 기능을 하는 것은?
- WIN32의 ClickAutomation와 같은 기능을 하는 것을 WEB에서 찾아줘.
- 기대 답변 : WEB/Click.md - WIN32가 취급하는 컨트롤에는 브라우저도 포함되므로 WIN32 액티비티가 상위호환이 된다. 

6) 함께 사용할 수 있는 것은?
- MultiThread 와 함께 사용할 수 있는 것은? 함께 사용되는 것은?
- 기대 답변 : COMMON/BreakThread.md, COMMON/GetThreadName.md 등 이 있다. 이 액티비티는 단독으로 사용될 수 없기에 MultiThread와 함께 사용 될 수 있는 것들이다.
