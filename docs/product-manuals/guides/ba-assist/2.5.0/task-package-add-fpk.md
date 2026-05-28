# BA-Assist 태스크 패키지(fpk) 생성 및 추가 방법

- Product: BA-Assist
- Version: 2.5.0
- Topic: task-package-add-fpk

## User Intent
사용자가 BA-Studio에서 만든 단일 태스크를 BA-Assist에서 실행할 수 있도록 `fpk` 패키지로 만들고 로딩하는 방법을 알고 싶어 한다.

## Related Questions
- BA-Assist에서 fpk 파일을 만드는 방법은?
- 태스크를 BA-Assist에 추가하려면?
- fp 파일을 fpk로 패키징하려면?
- fpk 파일을 BA-Assist에 로딩하는 절차를 알려줘.
- 단일 Task를 BA-Assist에서 실행하려면?

## Short Answer
단일 태스크를 BA-Assist에서 실행하려면 태스크 폴더를 선택해 Main Task가 되는 `fp` 파일을 지정하고 Package Name과 Description을 입력해 `fpk`로 저장합니다. 이후 BA-Assist 스케줄 에디터에서 `Add Task`를 클릭하고 생성한 `fpk` 파일을 선택하면 로딩됩니다.

## Steps
1. BA-Assist의 Packing 기능 또는 Option 탭에서 패키징 화면을 연다.
2. `fpk`로 변환할 태스크 폴더를 선택한다.
3. 폴더 안에서 Main Task에 해당하는 `fp` 파일을 선택한다.
4. Package Name과 Description을 입력한다.
5. 저장 경로와 파일 이름을 지정해 `fpk` 파일을 생성한다.
6. 스케줄 에디터 화면에서 `Add Task`를 클릭한다.
7. 파일 검색창에서 생성한 `fpk` 파일을 선택해 BA-Assist에 로딩한다.

## Notes
- `fpk`는 단일 태스크 파일(`fp`)과 관련 리소스를 묶은 패키지이다.
- 소스 확인 결과 `fpk`는 압축 해제 후 `config.json`의 `main`, `packageName`, `packageDesc`를 읽어 패키지 정보를 만든다.
- 이미 만들어진 `fpk`를 다시 불러와 패키지 정보를 수정하는 흐름도 업데이트 기능처럼 사용할 수 있다.

## Related Keywords
- fpk
- fp
- Package Name
- Description
- Main Task
- Add Task
- Packing
- 태스크 로딩

## Example Answer
`fp` 태스크를 BA-Assist에서 실행하려면 먼저 태스크 폴더를 선택하고 Main Task, Package Name, Description을 지정해 `fpk` 파일로 저장합니다. 그런 다음 BA-Assist 스케줄 에디터에서 `Add Task`를 눌러 생성한 `fpk` 파일을 선택하면 태스크가 로딩됩니다.
