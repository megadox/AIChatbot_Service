# BA-Assist 스케줄 활성화와 실행 후 PC 종료

- Product: BA-Assist
- Version: 2.5.0
- Topic: schedule-enable-shutdown

## User Intent
사용자가 BA-Assist 스케줄이 동작하지 않을 때 Enabled 설정을 확인하거나, 태스크 실행 후 PC를 종료하는 옵션을 알고 싶어 한다.

## Related Questions
- BA-Assist에서 스케줄은 어떻게 활성화해?
- Enabled 체크박스는 뭐야?
- 스케줄이 작동하지 않을 때 확인할 것은?
- 태스크 실행 후 PC를 종료하려면?
- Shutdown PC after execution은 언제 동작해?

## Short Answer
스케줄을 실제로 동작시키려면 Schedule 창에서 `Enabled` 체크 박스를 선택해야 합니다. 태스크 실행 후 PC를 종료하려면 `Shutdown PC after execution`을 체크합니다. 반복 실행 옵션이 있는 경우 PC 종료는 첫 번째 스케줄 이벤트 수행 후 이루어진다는 점에 주의해야 합니다.

## Steps
1. 스케줄을 설정할 태스크의 Schedule 창을 연다.
2. Trigger 시간과 반복 옵션을 확인한다.
3. 스케줄을 동작시키려면 `Enabled` 체크 박스를 선택한다.
4. 실행 후 PC 종료가 필요하면 `Shutdown PC after execution`을 체크한다.
5. 반복 실행 옵션이 켜져 있다면 종료 시점이 첫 실행 후인지 확인한다.
6. 저장 후 Schedule List에서 활성 스케줄로 표시되는지 확인한다.

## Notes
- Enabled가 선택 해제되어 있으면 예약된 스케줄은 작동하지 않는다.
- 소스 확인 결과 스케줄 활성 상태는 Windows 작업 스케줄러 작업 정의에도 반영된다.
- 실행 후 PC 종료 옵션이 켜져 있으면 작업 정의에 shutdown action이 추가될 수 있다.
- 트리거가 여러 개이거나 반복 옵션이 있는 경우 종료 시점 제약을 주의해야 한다.

## Related Keywords
- Enabled
- 스케줄 활성화
- 스케줄 비활성화
- Shutdown PC after execution
- PC 종료
- shutdown action

## Example Answer
BA-Assist 스케줄을 동작시키려면 Schedule 창에서 `Enabled`를 체크해야 합니다. 실행 후 PC를 종료하려면 `Shutdown PC after execution`을 선택합니다. 반복 스케줄에서는 첫 번째 실행 후 종료될 수 있으므로 반복 옵션과 함께 사용할 때는 주의하세요.
