
**********************************************************************************************************************
**********************************************************************************************************************

Phi-3-mini 모델을 기반으로 한 BA-Studio 내장형 로컬 챗봇의 상세 설계.

이 설계의 핵심은 C# WPF와의 매끄러운 통합과 오프라인 환경에서의 안정적인 추론입니다.

1. 소프트웨어 컴포넌트 구조

BA-Studio 프로세스 내에서 챗봇 엔진이 라이브러리 형태로 구동되는 구조입니다.

    컴포넌트               |                역할                         |           비고
Chat UI (WPF)               사용자 채팅 인터페이스 (대화창, 입력창)"             
Orchestrator                사용자의 질문을 받아 RAG 로직 제어                  C# Class Library
Vector Searcher             SQLite 기반 벡터 DB에서 관련 매뉴얼 검색            SQLite-net + Vector Extension
Inference Engine            GGUF 모델을 로드하여 답변 생성                     LLamaSharp 라이브러리


2. 데이터 흐름 (Data Flow) 설계

사용자가 질문을 입력했을 때 답변이 생성되는 내부 메커니즘입니다.

1) 질문 수신: 사용자가 "Click 액티비티 사용법 알려줘" 입력.

2) 임베딩: 질문 텍스트를 로컬 임베딩 모델을 통해 벡터(숫자 배열)로 변환.

3) 검색(Retrieval): 벡터 DB에서 질문과 가장 유사한 상위 3~5개의 매뉴얼 조각(Chunk)을 추출.

4) 프롬프트 구성: 추출된 매뉴얼 내용 + 질문을 결합하여 LLM용 프롬프트 생성.

5) 추론(Inference): Phi-3-mini 모델에 프롬프트를 입력하고 답변을 스트리밍 방식으로 출력.


3. 상세 파일 구성 및 배포 전략

설치 폴더 내에 다음과 같은 파일 구조를 유지해야 합니다.

 - /ChatBot

    * Phi-3-mini-4k-instruct-q4.gguf (2.3GB) : 메인 LLM 엔진

    * embedding_model.onnx (약 50MB) : 텍스트를 벡터로 바꾸는 경량 모델

    * ba_manual_vector.db (약 50MB) : SQLite 기반 지식 베이스

    * LLamaSharp.dll, libllama.dll : C# 연동을 위한 네이티브 라이브러리


4. C# 주요 구현 코드 가이드 (LLamaSharp 활용)

WPF 프로젝트에서 모델을 초기화하고 답변을 받는 핵심 로직 예시입니다.

C# 
// 1. 모델 설정 로드
var parameters = new ModelParams("ChatBot/Phi-3-mini-4k-instruct-q4.gguf")
{
    ContextSize = 4096,
    GpuLayerCount = 0 // CPU 전용 모드 (GPU가 있으면 숫자 조정 가능)
};

// 2. 모델 로드
using var weights = LLamaWeights.LoadFromFile(parameters);
using var context = weights.CreateContext(parameters);
var executor = new InteractiveExecutor(context);

// 3. 채팅 세션 시작
var session = new ChatSession(executor);

// 4. RAG 기반 답변 생성 (수도코드)
string userQuestion = txtInput.Text;
string contextInfo = vectorDb.Search(userQuestion); // DB에서 관련 내용 검색
string fullPrompt = $"Context: {contextInfo}\nQuestion: {userQuestion}\nAnswer:";

await foreach (var text in session.ChatAsync(fullPrompt, new InferenceParams() { Temperature = 0.7f }))
{
    rtbChatHistory.AppendText(text); // 대화창에 실시간 출력
}


5. 사용자 경험(UX) 최적화 포인트

* 초기 로딩 지연 처리: 2GB가 넘는 모델을 메모리에 올릴 때 약 5~10초가 소요됩니다. 챗봇 창을 처음 열 때 **"AI 엔진을 준비 중입니다..."**라는 프로그레스 바를 보여주는 것이 좋습니다.

* 리소스 관리: 챗봇을 사용하지 않을 때는 모델을 메모리에서 해제하거나 Working Set을 최소화하여 BA-Studio 본체의 성능에 영향을 주지 않도록 설계합니다.

* 스트리밍 출력: 답변이 한꺼번에 나오지 않고 한 글자씩 출력되도록 하여 사용자가 대기 시간을 짧게 느끼게 합니다.


6. 향후 확장성

* Self-Healing 연동: RPA 실행 중 에러가 발생하면 에러 코드를 챗봇에 자동으로 전달하여 해결 방법을 제시하는 기능을 추가할 수 있습니다.

* 업데이트 시스템: 매뉴얼이 수정될 경우 전체 재설치가 아닌 ba_manual_vector.db 파일만 교체하여 지식을 최신화할 수 있습니다.


**********************************************************************************************************************
**********************************************************************************************************************
5단계 상세 실행 가이드

BA-Studio 내장형 챗봇 개발을 위한 5단계 상세 실행 가이드입니다. 각 단계는 실제 개발 순서에 맞춰 구성되었습니다.


1단계: 지식 베이스 구축 (Data Engineering)
가장 먼저 LLM이 참고할 '교과서'를 디지털화해야 합니다.

    1. 데이터 수집 및 정제:

        * BA-Studio 매뉴얼, 액티비티 명세서(Input/Output/Property), FAQ를 Markdown(.md) 형식으로 정리합니다.

        * 구조화 예시: ### [Activity] Click, #### 설명, #### 속성값 등

    2. 문서 분할 (Chunking):

        * 한 번에 읽기 적당한 크기(500~1,000자 내외)로 텍스트를 자릅니다.

    3. 임베딩 및 벡터 DB 저장:

        * Python의 sentence-transformers 등을 사용해 텍스트를 벡터로 변환합니다.

________________________________________________________________________________________________________________________

2단계: C# 프로젝트 환경 설정 (Setup)
WinForm 프로젝트에서 LLM을 구동할 라이브러리를 준비합니다.

    1. NuGet 패키지 설치:

        * LLamaSharp: LLM 구동 핵심 라이브러리

        * LLamaSharp.Backend.Cpu: CPU 환경용 백엔드 (사용자 PC에 GPU가 없을 경우 대비)

        * Microsoft.ML.OnnxRuntime: 임베딩 모델 실행용

    2. 네이티브 라이브러리 확인:

        * libllama.dll 등 실행에 필요한 파일들이 출력 디렉토리(bin)에 복사되도록 설정합니다.

________________________________________________________________________________________________________________________

3단계: 로컬 RAG 로직 구현 (Backend Logic)
사용자의 질문에 맞는 답을 찾는 '검색기'를 코딩합니다.

    1. Semantic Search 구현:

        * 사용자 질문을 1단계에서 쓴 것과 동일한 임베딩 모델로 수치화합니다.

        * SQLite DB에서 질문 벡터와 가장 유사한(Cosine Similarity) 텍스트 조각을 찾아냅니다.

    2. 프롬프트 템플릿 설계:

        * Phi-3의 형식에 맞게 프롬프트를 조립합니다.

        <|system|>
        너는 BA-Studio 도우미야. 아래 제공된 [Manual] 내용만 참고해서 답변해줘.
        [Manual]: {DB에서 검색된 내용}
        <|user|>
        {사용자 질문}
        <|assistant|>

________________________________________________________________________________________________________________________

4단계: WPF UI 통합 (UI/UX)
사용자가 편리하게 사용할 수 있는 채팅창을 만듭니다.

    1. UI 구성:

        

    2. 비동기 처리 (Async):

        * LLM 추론은 무거운 작업이므로 반드시 Task.Run 또는 async/await를 사용하여 UI가 멈추지 않게 합니다.

        * 답변이 생성되는 대로 AppendText를 호출해 실시간으로 글자가 써지는 효과를 줍니다.

________________________________________________________________________________________________________________________

5단계: 배포 및 최적화 (Deployment)
사용자 PC에서 가볍고 빠르게 돌아가도록 마감합니다.

    1. 모델 양자화 확인:

        * Phi-3-mini-4k-instruct-q4_K_M.gguf 파일이 정상적으로 로드되는지 확인합니다.

    2. 리소스 관리:

        * 챗봇 사용이 끝나면 메모리를 반환(Dispose)하는 로직을 추가합니다.

        * 설치 패키지(MSI 등) 제작 시 모델 파일을 포함하거나, 설치 후 최초 실행 시 다운로드받는 기능을 구현합니다.