# BA-Assist 개요와 파일 형식

- Product: BA-Assist
- Version: 2.5.0
- Topic: overview-and-file-types

## User Intent
사용자가 BA-Assist가 무엇인지, BA-Studio/BA-Server와 어떤 관계인지, `fp`, `fpp`, `fpk`, `fpx` 파일이 무엇인지 알고 싶어 한다.

## Related Questions
- BA-Assist는 어떤 프로그램이야?
- BA-Assist와 BA-Studio 관계를 설명해줘.
- fp, fpp, fpk, fpx 차이가 뭐야?
- BA-Assist에서 실행할 수 있는 파일 형식은?
- 프로젝트 파일과 패키지 파일 차이를 알려줘.

## Short Answer
BA-Assist는 BA-Studio에서 만든 자동화 태스크를 사용자의 PC에서 스케줄에 따라 실행하는 RDA 제품입니다. BA-Studio의 태스크 파일은 `fp`, 프로젝트 파일은 `fpp`이고, BA-Assist에서 실행하려면 리소스와 함께 묶은 패키지 파일인 `fpk` 또는 `fpx` 형태로 변환해 로딩합니다.

## Steps
1. BA-Studio에서 단일 태스크를 만들면 `fp` 파일이 생성된다.
2. BA-Studio에서 프로젝트를 만들면 `fpp` 파일과 리소스 폴더가 포함된 프로젝트 구조가 생성된다.
3. 단일 태스크를 BA-Assist에서 실행하려면 `fp`와 리소스를 `fpk` 패키지로 만든다.
4. 프로젝트를 BA-Assist에서 실행하려면 `fpp`와 리소스를 `fpx` 패키지로 만든다.
5. 만들어진 `fpk` 또는 `fpx`를 BA-Assist에 로딩해 실행하거나 스케줄에 등록한다.

## Notes
- `fpk`는 태스크 패키지, `fpx`는 프로젝트 패키지로 이해하면 된다.
- 소스 확인 결과 BA-Assist는 Repository Path에서 `*.fpk`와 `*.fpx`를 모두 읽고 패키지 정보를 만든다.
- `fpx`는 압축을 풀어 내부 `fpp` 파일에서 시작 태스크와 패키지 정보를 읽는다.
- `fpk`는 `config.json`에서 main, packageName, packageDesc 정보를 읽는다.

## Related Keywords
- BA-Assist
- RDA
- fp
- fpp
- fpk
- fpx
- 패키지 파일
- 프로젝트 패키지
- 태스크 패키지

## Example Answer
BA-Assist는 BA-Studio에서 만든 자동화 태스크를 사용자의 PC에서 실행하고 스케줄링하는 도구입니다. BA-Studio의 단일 태스크는 `fp`, 프로젝트는 `fpp`로 저장되며, BA-Assist에서 실행하려면 각각 `fpk` 또는 `fpx` 패키지로 변환해 로딩합니다.
