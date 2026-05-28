# BA-Assist Dashboard 설정과 DB Sync 방법

- Product: BA-Assist
- Version: 2.5.0
- Topic: settings-dashboard

## User Intent
사용자가 BA-Assist에서 Dashboard 사용 설정, 서버 접속 정보 입력, 연결 테스트, DB Sync 절차를 알고 싶어 한다.

## Related Questions
- BA-Assist Dashboard 설정 방법을 알려줘.
- Use Dashboard는 어떻게 써?
- Dashboard Set에서 무엇을 입력해야 해?
- Connection Test는 언제 눌러?
- DB Sync 버튼은 어떤 기능이야?

## Short Answer
Dashboard를 사용하려면 Setting에서 `Use Dashboard`를 체크하고 `Dashboard Set`을 열어 BASE Url, Sub Path, Remote Task 사용 여부, RDA Nick Name/RDA ID 등을 설정합니다. `Connection Test`로 서버 연결을 확인하고 저장한 뒤, Dashboard와 BA-Assist 데이터를 동기화하려면 `DB Sync`를 실행합니다.

## Steps
1. Main 탭에서 `Setting`을 연다.
2. Assist Configuration에서 `Use Dashboard`를 체크한다.
3. `Dashboard Set` 버튼을 클릭한다.
4. Dashboard Information의 BASE Url과 Sub Path를 입력한다.
5. Remote Task 사용 여부를 설정한다.
6. `Connection Test`를 눌러 접속 가능 여부를 확인한다.
7. RDA Information에 Nick Name을 입력하고 필요하면 `Create RDA ID`를 누른다.
8. `Save`로 설정을 저장한다.
9. Dashboard와 동기화가 필요하면 `DB Sync`를 클릭한다.

## Notes
- 소스 확인 결과 DashboardSetting 화면은 Dashboard URL을 구성해 연결 시험을 수행한다.
- Dashboard DB Sync는 RDA 정보 업데이트와 Dashboard DB 동기화 서비스를 호출한다.
- Dashboard 페이지 접속 정보는 고객사/운영사 관리 정책에 따라 달라질 수 있다.

## Related Keywords
- Dashboard
- Use Dashboard
- Dashboard Set
- BASE Url
- Sub Path
- Remote Task
- Create RDA ID
- Connection Test
- DB Sync

## Example Answer
Dashboard를 쓰려면 Setting에서 `Use Dashboard`를 체크한 뒤 `Dashboard Set`에서 BASE Url, Sub Path, Remote Task 여부와 RDA 정보를 입력합니다. `Connection Test`로 연결을 확인하고 저장한 후, Dashboard에 스케줄 기반 태스크 정보를 반영하려면 `DB Sync`를 실행합니다.
