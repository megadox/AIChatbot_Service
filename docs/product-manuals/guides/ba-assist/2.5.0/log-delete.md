# BA-Assist 로그 삭제 방법

- Product: BA-Assist
- Version: 2.5.0
- Topic: log-delete

## User Intent
사용자가 BA-Assist에서 생성된 로그 메시지를 삭제하거나 로그 DB를 초기화하는 방법을 알고 싶어 한다.

## Related Questions
- BA-Assist 로그 메시지를 지우려면?
- logs.db는 어디에 있어?
- BA-Assist 로그 DB를 초기화하려면?
- 로그를 삭제하면 다시 생성돼?
- 수행 이력 DB를 지우는 방법은?

## Short Answer
BA-Assist에서 생성된 로그 메시지를 초기화하려면 데이터 관리 폴더 `C:\ProgramData\BA-Assist` 아래의 `logs.db` 파일을 찾아 삭제합니다. 삭제된 `logs.db`는 BA-Assist가 다시 동작할 때 자동으로 생성됩니다.

## Steps
1. BA-Assist를 종료한다.
2. `C:\ProgramData\BA-Assist` 폴더로 이동한다.
3. `logs.db` 파일을 찾는다.
4. 필요한 경우 백업을 만든다.
5. `logs.db`를 삭제한다.
6. BA-Assist를 다시 실행해 로그 DB가 자동 생성되는지 확인한다.

## Notes
- Appendix Q3의 공식 FAQ 내용에 근거한다.
- 삭제 전 기존 수행 이력이 필요한지 확인하는 것이 좋다.
- 운영 중인 BA-Assist가 파일을 잡고 있을 수 있으므로 종료 후 처리하는 편이 안전하다.

## Related Keywords
- 로그 삭제
- logs.db
- C:\ProgramData\BA-Assist
- 수행 이력
- 로그 메시지
- 로그 초기화

## Example Answer
BA-Assist 로그를 초기화하려면 `C:\ProgramData\BA-Assist` 폴더에서 `logs.db` 파일을 삭제합니다. 삭제된 `logs.db`는 BA-Assist 실행 시 다시 자동 생성됩니다.
