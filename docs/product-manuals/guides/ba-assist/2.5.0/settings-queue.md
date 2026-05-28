# BA-Assist Queue 설정 방법

- Product: BA-Assist
- Version: 2.5.0
- Topic: settings-queue

## User Intent
사용자가 BA-Assist Queue 기능, Automatic/Manual 모드, Queue Task List, Sequential과의 제약을 알고 싶어 한다.

## Related Questions
- BA-Assist Queue 기능은 뭐야?
- Use Queue는 어떻게 설정해?
- Task Watcher는 어떤 기능이야?
- Queue Task List에서 뭘 할 수 있어?
- Queue와 Sequential Task를 같이 쓸 수 있어?

## Short Answer
Queue 기능은 동시에 실행하기 어려운 태스크를 대기열에 넣고 순차적으로 실행하기 위한 설정입니다. Automatic 모드는 Task Watcher가 이전 Task 종료를 감시해 Queue 태스크를 실행하고, Manual 모드는 Queue Task List에서 사용자가 직접 실행/검색/삭제합니다. Sequential Task와 Queue는 함께 사용할 수 없습니다.

## Steps
1. Main 탭에서 `Setting`을 연다.
2. Queue Configuration 탭에서 `Use Queue`를 확인한다.
3. 자동 실행이 필요하면 `Task Watcher + Run Queue Task (Automatic)` 방식을 선택한다.
4. Watch Interval과 Watcher run time을 설정한다.
5. 수동 실행이 필요하면 `Run Queue Task (Manual)` 방식을 사용한다.
6. Queue Task List에서 Run Task, Search, Delete executed tasks, Delete 기능을 사용한다.
7. Sequential Task를 쓰는 경우 Queue와 동시에 사용하지 않도록 한다.

## Notes
- 매뉴얼과 Appendix 모두 Queue 기능은 Sequential Task와 함께 사용할 수 없다고 설명한다.
- 소스 확인 결과 Queue 옵션이 비어 있으면 기본값은 `Both`로 저장된다.
- Queue Task List는 큐에 쌓인 Task 목록을 확인하고 실행/검색/삭제하는 창이다.

## Related Keywords
- Queue
- Use Queue
- Task Watcher
- Queue Task List
- Automatic
- Manual
- Watch Interval
- Watcher run time
- Sequential

## Example Answer
Queue는 Task를 대기열에 넣어 실행 순서를 제어하는 기능입니다. 자동 모드에서는 Task Watcher가 이전 Task 종료를 감시해 다음 Queue Task를 실행하고, 수동 모드에서는 Queue Task List에서 사용자가 직접 실행하거나 삭제합니다. 단, Sequential Task와 Queue는 함께 사용할 수 없습니다.
