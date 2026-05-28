# BA-Assist Schedule List, Log, Stop 사용 방법

- Product: BA-Assist
- Version: 2.5.0
- Topic: schedule-list-log-stop

## User Intent
사용자가 스케줄 목록 확인, 실행 로그 조회, 수동 실행 중인 Task 정지 방법을 알고 싶어 한다.

## Related Questions
- BA-Assist에서 스케줄 목록은 어디서 봐?
- Schedule List는 어떤 기능이야?
- BA-Assist 로그는 어디서 확인해?
- 실패 로그 메시지를 보려면?
- 수동으로 실행한 Task를 멈추려면?

## Short Answer
스케줄 목록은 Main 탭의 `Schedule List`에서 확인하며, 스케줄이 설정된 태스크만 모아 보여줍니다. 실행 이력과 실패 메시지는 Main 탭의 `Log`에서 확인하고, 수동으로 동작한 Task를 멈추려면 Main 탭의 `Stop` 기능을 사용합니다.

## Steps
1. Main 탭에서 `Schedule List`를 클릭해 스케줄이 설정된 태스크 목록을 확인한다.
2. 필요하면 Schedule List의 Export/Import 기능으로 스케줄을 내보내거나 불러온다.
3. Main 탭에서 `Log`를 클릭해 태스크 수행 이력을 연다.
4. Task 필드에 태스크 이름 일부 또는 전체를 입력해 로그를 검색한다.
5. 성공 여부, 수행 시간, 실패 시 로그 메시지를 확인한다.
6. 수동 실행 중인 Task를 멈춰야 하면 Main 탭의 `Stop`을 사용한다.

## Notes
- 시스템 트레이 메뉴의 `Logs`와 `Task Schedule`도 관련 화면으로 이동한다.
- 소스 확인 결과 패키지 상태 표시에는 Schedule Trigger, Next run time, Last run time, Last task result가 Tooltip으로 표시될 수 있다.
- Appendix에 따르면 로그 DB를 완전히 초기화하려면 `C:\ProgramData\BA-Assist` 아래의 `logs.db` 삭제를 안내한다.

## Related Keywords
- Schedule List
- Task Schedules
- Log
- Logs
- Stop
- 실행 이력
- 실패 메시지
- logs.db

## Example Answer
스케줄 목록은 Main 탭의 `Schedule List`에서 보고, 실행 이력은 `Log`에서 확인합니다. 로그 화면에서는 태스크 이름으로 검색할 수 있고 성공 여부와 실패 메시지를 확인할 수 있습니다. 수동 실행 중인 Task를 멈추려면 `Stop`을 사용합니다.
