# BA-Assist 스케줄 옵션과 다중 인스턴스 설명

- Product: BA-Assist
- Version: 2.5.0
- Topic: schedule-options

## User Intent
사용자가 스케줄 반복 옵션, One Time/Daily/Weekly/Monthly, Period, Job Repeat Interval, 다중 인스턴스 옵션의 의미를 알고 싶어 한다.

## Related Questions
- BA-Assist 스케줄 옵션은 뭐가 있어?
- One Time, Daily, Weekly, Monthly 차이는?
- Job Repeat Interval과 Period는 뭐야?
- 다중 인스턴스 옵션을 설명해줘.
- IgnoreNew, Parallel, Queue, StopExisting 차이는?

## Short Answer
BA-Assist 스케줄은 One Time, Daily, Weekly, Monthly 반복 주기를 지원하며, 반복 실행이 필요하면 Job Repeat Interval과 Period로 반복 간격과 반복 유지 시간을 설정합니다. 다중 인스턴스 정책은 `IgnoreNew`, `Parallel`, `Queue`, `StopExisting`를 지원하며 각각 새 실행 무시, 병렬 실행, 대기열 등록, 기존 실행 중지를 의미합니다.

## Steps
1. Schedule 창에서 Trigger 시작 날짜와 시간을 정한다.
2. Times 영역에서 One Time, Daily, Weekly, Monthly 중 하나를 선택한다.
3. 반복 실행이 필요하면 Job Repeat Interval을 설정한다.
4. 반복을 유지할 시간 범위가 필요하면 Period를 설정한다.
5. Weekly는 요일을, Monthly는 특정 날짜 또는 특정 주차/요일 방식을 설정한다.
6. 이미 실행 중인 태스크와 겹칠 수 있으면 다중 인스턴스 정책을 선택한다.

## Notes
- 소스의 `TaskScheduleler_Info.cs` 기준 Trigger 유형은 `OneTime`, `Daily`, `Weekly`, `Monthly`이다.
- 반복 간격 단위는 `minute`, `hour`, `day`를 지원한다.
- 다중 인스턴스 정책은 `IgnoreNew`, `Parallel`, `Queue`, `StopExisting`이다.
- `Parallel`은 동시에 작업이 진행되어 오류 가능성이 높을 수 있어 주의가 필요하다.
- `Queue` 사용 시 Setting에서 Queue 사용 설정을 확인해야 한다.

## Related Keywords
- One Time
- Daily
- Weekly
- Monthly
- Period
- Job Repeat Interval
- IgnoreNew
- Parallel
- Queue
- StopExisting
- 다중 인스턴스

## Example Answer
스케줄 반복은 One Time, Daily, Weekly, Monthly 중에서 선택합니다. 같은 태스크가 이미 실행 중일 때의 처리는 `IgnoreNew`(새 실행 무시), `Parallel`(병렬 실행), `Queue`(대기), `StopExisting`(기존 실행 중지) 중에서 선택합니다.
