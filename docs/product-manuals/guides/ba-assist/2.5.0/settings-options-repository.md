# BA-Assist 옵션, Repository, 입력 차단 설정

- Product: BA-Assist
- Version: 2.5.0
- Topic: settings-options-repository

## User Intent
사용자가 BA-Assist Setting의 화면 보호기 방지, 키보드/마우스 입력 차단, 스크린 캡처/녹화, 로그 레벨, Repository 설정을 알고 싶어 한다.

## Related Questions
- BA-Assist Setting 옵션을 설명해줘.
- 태스크 실행 중 키보드와 마우스를 막으려면?
- 화면 보호기 진입을 방지하려면?
- 오류 화면 캡처 설정은 어디서 해?
- Repository Path는 어디에 저장돼?

## Short Answer
BA-Assist Setting에서는 화면 보호기 방지, Dashboard, 키보드/마우스 입력 차단, 스크린 녹화/캡처, 멀티 태스크, 로그 레벨, 저장 폴더 관련 설정을 관리합니다. 소스 확인 결과 Local Repository Root/RootName, Queue 설정, 로그 보관 기간, 입력 차단, 오류 화면 캡처 설정은 config에 저장됩니다.

## Steps
1. Main 탭에서 `Setting`을 연다.
2. 화면 보호기 방지가 필요하면 Screen Saver 관련 옵션을 체크한다.
3. 태스크 실행 중 사용자 입력을 막으려면 키보드/마우스 제어 옵션을 사용한다.
4. 입력 차단을 해제해야 하면 Esc를 두 번 눌러 방지 모드를 해제한다.
5. 오류 분석이 필요하면 스크린 캡처/녹화 옵션을 확인한다.
6. 로그 출력 수준이 필요하면 Full, Normal, Simple, OneLine, None 중에서 설정한다.
7. Repository 관련 설정을 바꾸는 경우 기존 Task/Log/Setting DB 이전 여부를 함께 확인한다.

## Notes
- 키보드/마우스 입력 차단을 사용하지 않으면 사용자의 입력으로 태스크 실행 중 오동작이 발생할 수 있다.
- 소스의 `Settings.cs`는 Local Root, RootName, 서버 접속 정보, Queue, 로그 보관 기간을 config에서 읽고 쓴다.
- 서버 프로토콜 기본값은 `http`, 서버 포트 기본값은 `8080`이다.
- 화면 설정 일부는 WinForms `Properties.Settings`에도 저장된다.

## Related Keywords
- Setting
- Screen Saver
- 키보드 입력 차단
- 마우스 입력 차단
- Screen Capture
- Screen Recording
- Log Level
- Repository Path
- Config

## Example Answer
BA-Assist Setting에서는 실행 중 사용자 입력 차단, 화면 보호기 방지, 캡처/녹화, 로그 레벨, Repository 같은 실행 환경 옵션을 관리합니다. 입력 차단을 켜면 태스크 수행 중 키보드/마우스 입력을 막을 수 있고, 해제는 Esc 두 번으로 처리합니다.
