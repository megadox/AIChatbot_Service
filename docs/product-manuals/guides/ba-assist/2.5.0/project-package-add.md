# BA-Assist 프로젝트 패키지 추가 방법

- Product: BA-Assist
- Version: 2.5.0
- Topic: project-package-add

## User Intent
사용자가 BA-Assist에서 프로젝트를 추가하거나 BA-Studio 프로젝트 패키지(`fpx`)를 로딩하는 방법을 알고 싶어 한다.

## Related Questions
- 프로젝트를 추가하는 방법을 알려줘.
- BA-Assist에서 프로젝트를 추가하려면?
- BA-Assist에 fpx 파일을 등록하려면?
- 프로젝트 패키지를 BA-Assist에 로딩하는 방법은?
- Add Task로 프로젝트를 불러오는 절차를 알려줘.

## Short Answer
BA-Assist에서 프로젝트를 추가한다는 것은 BA-Studio에서 내보낸 프로젝트 패키지(`fpx`)를 BA-Assist에 로딩하는 절차입니다. BA Assist의 스케줄 에디터 화면에서 `Add Task`를 선택한 뒤, 생성해 둔 `fpx` 파일을 선택하면 프로젝트 패키지가 BA-Assist에 로딩됩니다.

## Steps
1. BA-Studio에서 프로젝트를 `fpx` 패키지 파일로 변환한다.
2. BA-Assist의 스케줄 에디터 화면을 연다.
3. 왼쪽 상단의 `Add Task`를 선택한다.
4. 파일 검색창에서 Step 1에서 만든 `fpx` 파일을 선택한다.
5. 확인 버튼을 눌러 파일을 로딩한다.
6. 로딩 후 BA-Assist 화면에 선택한 `fpx` 파일이 표시되는지 확인한다.
7. 자동 실행이 필요하면 로딩된 항목에 대해 스케줄을 설정한다.

## Notes
- BA-Assist는 `fpk` 태스크 패키지와 `fpx` 프로젝트 패키지를 모두 로딩할 수 있다.
- `fpx`는 BA-Studio의 Project 만들기로 생성된 Project 폴더를 패키지로 변환한 파일이다.
- `fpk`는 단일 태스크 패키지이고, `fpx`는 프로젝트 패키지이므로 질문이 프로젝트 추가라면 `fpx` 로딩 절차를 우선 안내한다.
- 매뉴얼의 관련 위치는 `3-1. 태스크(=fpk 또는 fpx 파일) 로딩하기`와 `3-1-1. fpx 파일 변환 후, 로딩하기`이다.

## Related Keywords
- 프로젝트 추가
- 프로젝트 등록
- 프로젝트 로딩
- fpx
- fpp
- Add Task
- 패키지 추가
- 패키지 등록
- 태스크 로딩
- 스케줄 에디터

## Example Answer
BA-Assist에서 프로젝트를 추가하려면 먼저 BA-Studio 프로젝트를 `fpx` 패키지로 내보낸 뒤 BA-Assist에 로딩합니다. BA-Assist의 스케줄 에디터 화면에서 `Add Task`를 선택하고, 파일 검색창에서 만들어 둔 `fpx` 파일을 선택하면 프로젝트 패키지가 추가됩니다. 이후 자동 실행이 필요하면 해당 항목에 스케줄을 설정하세요.
