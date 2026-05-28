# BA-Assist 스케줄 생성 방법

- Product: BA-Assist
- Version: 2.5.0
- Topic: schedule-create

## User Intent
사용자가 BA-Assist에서 로딩된 태스크에 스케줄을 등록하는 기본 절차를 알고 싶어 한다.

## Related Questions
- BA-Assist에서 스케줄을 등록하려면?
- 태스크에 스케줄을 설정하는 방법은?
- Schedule 창은 어떻게 열어?
- 스케줄 시작 시간은 어디서 설정해?
- BA-Assist 예약 실행 방법을 알려줘.

## Short Answer
BA-Assist에서 스케줄을 만들려면 먼저 실행할 태스크를 로딩한 뒤 태스크를 선택하고 `Schedule` 버튼 또는 `+` 버튼을 클릭합니다. Schedule 창에서 Trigger 날짜/시간, 반복 옵션, PC 종료 여부, Enabled 상태를 설정해 저장하면 예약 실행이 등록됩니다.

## Steps
1. `fpk` 또는 `fpx` 패키지를 BA-Assist에 로딩한다.
2. 스케줄을 설정할 태스크를 선택한다.
3. `Schedule` 버튼 또는 `+` 버튼을 클릭해 Schedule 창을 연다.
4. Trigger 날짜와 시간을 입력하거나 달력/이동 버튼으로 선택한다.
5. One Time, Daily, Weekly, Monthly 중 반복 주기를 선택한다.
6. 필요하면 반복 간격, 반복 유지 시간, PC 종료 옵션을 설정한다.
7. `Enabled`를 체크해 스케줄을 활성화한다.
8. 저장 후 Schedule List에서 등록 상태를 확인한다.

## Notes
- 소스 확인 결과 BA-Assist 스케줄은 Windows 작업 스케줄러의 `BA-Assist` 폴더 아래에 등록된다.
- 스케줄 실행 Action은 BA-Assist 플레이어 실행 파일과 패키지 파일 경로를 인자로 사용한다.
- 관리자 권한 실행 여부에 따라 작업 스케줄러의 LogonType과 RunLevel 설정이 달라질 수 있다.

## Related Keywords
- 스케줄 등록
- Schedule
- Trigger
- Enabled
- 예약 실행
- Windows 작업 스케줄러
- BA-Assist 폴더

## Example Answer
태스크를 로딩한 뒤 해당 태스크를 선택하고 `Schedule` 또는 `+` 버튼을 누르면 Schedule 창이 열립니다. 여기서 Trigger 시간, 반복 옵션, PC 종료 여부를 설정하고 `Enabled`를 체크해 저장하면 BA-Assist 스케줄이 등록됩니다.
