# BA-Assist, BA-Server, BA-Worker 차이

- Product: BA-Assist
- Version: 2.5.0
- Topic: server-worker-difference

## User Intent
사용자가 BA-Assist가 BA-Server나 BA-Worker와 어떻게 다른지, BA-Worker 설치가 필요한지 알고 싶어 한다.

## Related Questions
- BA-Assist와 BA-Server 차이는?
- BA-Assist는 BA-Worker가 필요해?
- BA-Assist는 여러 Bot PC를 제어할 수 있어?
- BA-Server와 BA-Assist 역할을 비교해줘.
- BA-Assist는 어디에서 태스크를 실행해?

## Short Answer
BA-Assist는 설치된 한 대의 PC에서 스케줄 기반 태스크를 실행하는 RDA 제품입니다. BA-Server는 BA-Worker가 설치된 여러 Bot PC에 스케줄을 설정해 태스크를 분산 실행할 수 있습니다. BA-Assist는 독자적인 플레이어로 태스크 파일을 실행하므로 BA-Worker를 설치할 필요가 없습니다.

## Steps
1. 단일 사용자 PC에서 로컬 스케줄 실행이 필요하면 BA-Assist를 사용한다.
2. 여러 Bot PC에 태스크를 분산 실행하려면 BA-Server와 BA-Worker 구성을 사용한다.
3. BA-Assist만 사용하는 환경에서는 BA-Worker 설치가 필수인지 확인할 필요가 없다.
4. BA-Assist는 자체 플레이어로 로딩된 패키지 파일을 실행한다.

## Notes
- Appendix Q1은 BA-Assist가 독자적인 플레이어를 통해 태스크 파일을 실행하므로 BA-Worker 설치가 필요 없다고 설명한다.
- Appendix Q2는 BA-Assist는 설치된 PC에서만 동작하고, BA-Server는 다수 Bot PC에 스케줄을 설정할 수 있다고 설명한다.
- BA-Assist는 BA-Studio에서 만든 `fpk`/`fpx` 패키지를 로딩해 실행한다.

## Related Keywords
- BA-Assist
- BA-Server
- BA-Worker
- Bot PC
- RDA
- 플레이어
- 분산 실행
- 스케줄 기반

## Example Answer
BA-Assist는 설치된 한 대의 PC에서 스케줄 기반 태스크를 실행하는 도구입니다. 반면 BA-Server는 BA-Worker가 설치된 여러 Bot PC에 태스크를 분산 실행할 수 있습니다. BA-Assist는 자체 플레이어로 태스크를 실행하므로 BA-Worker 설치가 필요하지 않습니다.
