# BA Chatbot UI Improvement Implementation Plan

## Goal

BA Chatbot을 설치 패키지로 배포하기 전에, 현재 기능 중심 WPF UI를 실제 제품처럼 보이고 사용할 수 있는 챗봇 UI로 개선한다.

핵심 방향은 다음과 같다.

- 첫 실행부터 제품명, 연결 상태, 사용 가능한 질문 범위가 명확하게 보인다.
- 대화 영역은 최신 AI 챗봇처럼 읽기 쉽고, 입력과 응답 흐름이 부드럽다.
- Online, Offline, API Add-on 배포 환경의 차이를 사용자가 불안하지 않게 이해할 수 있다.
- 기능 추가보다 시각 완성도, 정보 위계, 상태 피드백, 설치 환경 안정성을 우선한다.

## Current UI Diagnosis

현재 UI는 기본 기능이 잘 연결되어 있으나 제품 UI로 보기에는 다음 한계가 있다.

- 전체 레이아웃이 기본 WPF 업무 도구 느낌에 가깝고, 브랜드 경험이 약하다.
- 메시지 카드가 전체 폭으로 펼쳐져 실제 대화 앱의 리듬이 부족하다.
- User, Assistant 역할 표시가 기술적이며 사용자 친화적이지 않다.
- 상태 정보가 상단 텍스트와 하단 로그로 분산되어 있어 현재 무엇이 일어나는지 한눈에 보기 어렵다.
- 질문 유형, 문맥 유지, 웹 검색 옵션이 입력부 위에 노출되어 있어 초보 사용자에게 복잡하게 느껴질 수 있다.
- 서버 버전, 연결 상태, 로컬 API 감지 상태가 설치 패키지 UX와 충분히 연결되어 있지 않다.
- 설정 창이 단순 폼 구조라 Online/Offline 환경 선택, API 토큰, health check 결과를 제품스럽게 안내하지 못한다.

## Design Direction

### Product Tone

- 제품명: `BA Chatbot`
- 보조 설명: `BA-Studio와 BA-Assist 사용을 돕는 AI Assistant`
- 톤: 전문적, 조용한, 신뢰감 있는 업무용 AI 도구
- 피해야 할 방향: 과한 그라데이션, 마케팅 페이지 같은 장식, 장난스러운 말투, 지나치게 어두운 테마

### Visual System

- 기본 배경: `#F4F6F8` 계열의 밝은 neutral
- 주요 텍스트: `#111827`
- 보조 텍스트: `#667085`
- 브랜드 포인트: 딥 블루 또는 청록 계열 1개
- 성공 상태: muted green
- 경고 상태: amber
- 오류 상태: red
- 카드 radius: 6-8px
- 메시지 bubble radius: 10-14px
- 그림자: 아주 약한 elevation만 사용
- 폰트: Windows 기본 환경을 고려해 `Segoe UI` 유지, 한글 가독성을 위해 line-height와 spacing 개선

## Target Layout

### 1. App Shell

현재 2열 구조는 유지하되, 정보 위계를 재정렬한다.

- 좌측: 대화 목록과 새 대화 버튼
- 중앙: 대화 본문
- 하단: 입력 composer
- 상단: 제품명, 모드 배지, 연결 상태, 설정 버튼

구현 포인트:

- `MainWindow.xaml`의 최상단 header를 56px 고정 바에서 64px 내외의 product header로 개선한다.
- `BA-Studio AI Chatbot` 표기는 `BA Chatbot`으로 변경한다.
- `AnswerModeText`는 텍스트 대신 `Online` / `Offline` 상태 배지로 표현한다.
- `StatusText`는 우측 작은 텍스트가 아니라 상태 dot + label로 표현한다.
- 설정 버튼은 현재 텍스트 아이콘 유지 가능하나 hover/pressed 상태를 버튼 스타일로 통일한다.

### 2. Sidebar

대화 목록은 단순 리스트가 아니라 최근 작업을 빠르게 찾는 영역으로 개선한다.

구현 포인트:

- 상단에 full-width `새 대화` primary 버튼 배치.
- 삭제 버튼은 항상 노출하지 않고 선택된 대화의 context action 또는 작은 icon button으로 이동.
- 세션 item은 제목, 마지막 수정 시간, 질문 유형 또는 메시지 개수 표시.
- 선택된 세션은 배경색과 좌측 accent bar로 명확히 구분.
- 대화가 없거나 새 세션만 있을 때 empty state 문구 제공.

필요 ViewModel 확장:

- `ChatSessionViewModel`에 `MessageCountText` 또는 `Subtitle` 추가 검토.
- 삭제 confirmation dialog 추가 검토.

### 3. Chat Canvas

메시지는 전체 폭 카드가 아니라 대화형 bubble 구조로 바꾼다.

구현 포인트:

- 사용자 메시지는 우측 정렬, 최대 폭 70%.
- 챗봇 메시지는 좌측 정렬, 최대 폭 78%.
- 챗봇 첫 메시지는 welcome panel로 별도 표현한다.
- 역할명 `User`, `Assistant`를 `나`, `BA Chatbot` 또는 아이콘/아바타로 변경한다.
- 메시지 timestamp를 작은 보조 텍스트로 표시한다.
- 복사 버튼은 메시지 hover 시 드러나게 하거나 우측 상단에 작게 유지한다.
- 답변 streaming 중에는 skeleton 또는 typing indicator를 표시한다.
- 오류 메시지는 일반 답변 bubble과 다른 warning/error style로 표시한다.

필요 ViewModel 확장:

- `ChatMessageViewModel.DisplayRole`
- `ChatMessageViewModel.DisplayCreatedAt`
- `ChatMessageViewModel.MessageKind` 또는 `IsError`
- streaming 중 빈 Assistant 메시지를 위한 `IsPending`

### 4. Welcome State

새 대화 첫 화면은 단순 안내 문장 대신 제품 첫인상을 담당하는 화면으로 만든다.

구성:

- 짧은 인사: `무엇을 도와드릴까요?`
- 3-4개 추천 질문 chip
- 현재 모드와 데이터 출처 요약

추천 질문 예:

- `Excel 파일을 여는 액티비티 사용법 알려줘`
- `BA-Studio에서 Task를 실행하는 방법은?`
- `BA-Assist 스케줄을 등록하는 절차 알려줘`
- `WIN32 ControlExists 속성 설명해줘`

필요 구현:

- 추천 질문 chip 클릭 시 `SelectedSession.InputText`에 입력하거나 바로 전송.
- 추천 질문 목록은 ViewModel 상수로 시작하고 추후 설정 가능하게 확장.

### 5. Input Composer

입력부는 챗봇의 핵심이므로 제품형 composer로 개선한다.

구현 포인트:

- 입력 박스를 rounded container 안에 배치하고, 전송/취소 버튼을 우측 icon button으로 정리.
- Enter 전송, Shift+Enter 줄바꿈 동작은 유지한다.
- 질문 유형은 composer 위 고정 폼이 아니라 compact segmented control 또는 dropdown chip으로 정리한다.
- `문맥 유지`, `웹 검색`은 toggle chip으로 표현한다.
- `답변` 버튼은 `답변 수정` 또는 `피드백`으로 명확히 변경한다.
- 전송 중에는 send 버튼 대신 stop 버튼을 보여준다.
- 입력 placeholder 추가: `BA-Studio 기능, 액티비티, BA-Assist 사용법을 질문하세요.`

필요 ViewModel 확장:

- `InputPlaceholder`
- `CanStop`은 기존 `IsBusy`로 대체 가능
- 추천 질문 실행 command

### 6. Process Log

현재 하단 로그는 디버그 정보로 유용하지만 일반 사용자에게는 부담스럽다.

개선 방향:

- 기본은 접힌 상태의 `처리 과정` 패널로 제공한다.
- 답변 bubble 하단의 `자세히`와 역할을 분리한다.
- 상태 흐름은 짧은 stepper로 표현한다.

예:

- 질문 분석
- 관련 문서 검색
- 답변 생성
- 완료

구현 포인트:

- 하단 고정 로그 영역을 없애거나 collapsed expander로 이동한다.
- `SelectedSession.ProcessLogText`는 유지하되 UI 노출 방식을 바꾼다.
- 고급 사용자용으로 `처리 로그 보기` 버튼 제공.

### 7. Settings UX

설정 창은 설치 패키지의 Online/Offline 차이를 이해시키는 핵심 화면이다.

구현 포인트:

- `답변 서비스 모드` ComboBox를 card 형태의 mode selector로 변경한다.
- Online card: API 주소, API 토큰, 연결 테스트 버튼, 서버 버전 표시.
- Offline card: 로컬 API 감지 상태, 실행 경로, health check 결과 표시.
- API Add-on이 감지되면 `로컬 API 사용 가능` 배지 표시.
- 토큰 입력은 저장 여부와 masking을 명확히 한다.
- 저장 전 validation: URL 형식, token empty warning, health check 실패 경고.

필요 서비스 확장:

- `ChatbotSettingsStore`는 유지.
- `ApiHealthCheckService` 또는 `RemoteChatOrchestratorClient` 재사용 health check 추가.
- installer 작업의 `api/BAStudio.Chatbot.Api.exe` 감지 로직과 연계.

## Implementation Phases

### Phase 1: Visual Refresh Without Architecture Change

목표: 리스크 낮게 제품 인상을 개선한다.

- `MainWindow.xaml`에 color/style resource 정리.
- header, sidebar, message bubble, composer 스타일 개선.
- 버튼, TextBox, ComboBox 기본 style 통일.
- User/Assistant 표시명을 사용자 친화적으로 변경.
- 입력 placeholder와 welcome message 문구 개선.
- process log를 접힘 패널로 변경.
- 설정 버튼, 복사 버튼, 전송 버튼의 hover/disabled 상태 정리.

예상 파일:

- `src/BAStudio.Wpf/MainWindow.xaml`
- `src/BAStudio.Wpf/ViewModels/ChatMessageViewModel.cs`
- `src/BAStudio.Wpf/ViewModels/ChatSessionViewModel.cs`

완료 기준:

- 기존 질의/응답 기능이 그대로 동작한다.
- Online/Offline 모드 전환이 기존과 동일하게 동작한다.
- 780px 최소 폭에서도 버튼과 텍스트가 겹치지 않는다.

### Phase 2: Product UX Enhancements

목표: 사용자 흐름과 상태 피드백을 제품 수준으로 끌어올린다.

- 추천 질문 chip과 quick prompt command 추가.
- 메시지 timestamp 표시.
- streaming pending indicator 추가.
- 처리 과정을 stepper 또는 compact timeline으로 표시.
- 세션 list subtitle 추가.
- 삭제 확인 dialog 추가.
- 답변 수정 UX 명칭과 위치 정리.

예상 파일:

- `src/BAStudio.Wpf/MainWindow.xaml`
- `src/BAStudio.Wpf/MainWindow.xaml.cs`
- `src/BAStudio.Wpf/ViewModels/ChatViewModel.cs`
- `src/BAStudio.Wpf/ViewModels/ChatMessageViewModel.cs`
- `src/BAStudio.Wpf/ViewModels/ChatSessionViewModel.cs`

완료 기준:

- 새 사용자도 추천 질문만으로 첫 질의를 시작할 수 있다.
- 답변 생성 중 상태가 명확히 보인다.
- 처리 로그가 필요할 때만 드러난다.

### Phase 3: Installer-Aware Settings UX

목표: 설치 패키지 유형별 사용 환경을 UI에서 안정적으로 안내한다.

- 설정 창 mode selector 리디자인.
- Online API 연결 테스트 추가.
- Offline local API 감지 및 health check 표시.
- API Add-on 감지 결과를 설정 화면과 header 상태에 반영.
- `/health`의 `version`, `imageTag`, `gitSha`, `kb` 표시 영역 추가.

예상 파일:

- `src/BAStudio.Wpf/ChatbotSettingsWindow.xaml`
- `src/BAStudio.Wpf/ViewModels/ChatbotSettingsViewModel.cs`
- `src/BAStudio.Wpf/Services/ChatbotSettingsStore.cs`
- 신규 `src/BAStudio.Wpf/Services/ChatbotHealthService.cs`

완료 기준:

- Online 패키지에서 원격 API 연결 상태가 명확히 보인다.
- Offline 패키지에서 로컬 API 실패 시 사용자가 다음 행동을 알 수 있다.
- API Add-on 설치 여부가 UI에 반영된다.

## Concrete UI Changes

### Header

변경 전:

- `BA-Studio AI Chatbot`
- 작은 상태 텍스트
- 설정 아이콘

변경 후:

- `BA Chatbot`
- `Online` 또는 `Offline` badge
- 연결 상태 dot: `준비됨`, `검색 중`, `답변 생성 중`, `오류`
- 서버/앱 버전 compact label
- 설정 icon button

### Message Bubble

변경 전:

- 전체 폭 border card
- `User`, `Assistant` 텍스트
- 본문 TextBox

변경 후:

- 좌우 정렬 bubble
- `나`, `BA Chatbot`
- timestamp
- copy action
- details expander
- error/warning visual state

### Composer

변경 전:

- 질문 유형 ComboBox
- 문맥 유지 CheckBox
- 웹 검색 CheckBox
- TextBox
- 전송/답변/취소 버튼

변경 후:

- 옵션 chip row
- multiline input container
- send/stop icon button
- feedback button은 마지막 답변 아래 또는 composer 보조 action으로 이동

## Technical Notes

### XAML Style Organization

초기에는 별도 design system 파일을 만들지 않고 `MainWindow.xaml`의 `Window.Resources`에 스타일을 정리한다. 스타일이 많아지면 다음 단계에서 `Styles/ChatbotTheme.xaml`로 분리한다.

권장 resource:

- `Brush.AppBackground`
- `Brush.Surface`
- `Brush.SurfaceMuted`
- `Brush.TextPrimary`
- `Brush.TextSecondary`
- `Brush.Brand`
- `Brush.Border`
- `Style.IconButton`
- `Style.PrimaryButton`
- `Style.ChatBubble`
- `Style.SidebarSessionItem`

### Message Alignment

WPF `ItemsControl.ItemTemplate`에서 `IsUser` 기반 alignment 변환이 필요하다.

선택지:

- `ChatMessageViewModel`에 `HorizontalAlignment BubbleAlignment` 제공
- `BooleanToAlignmentConverter` 추가

초기 구현은 ViewModel property 방식이 가장 단순하다.

### Placeholder

WPF 기본 `TextBox`에는 placeholder가 없으므로 다음 중 하나를 선택한다.

- overlay `TextBlock`을 TextBox 위에 배치하고 `InputText` empty 여부로 visibility 제어
- reusable attached property 구현

초기 구현은 overlay 방식으로 충분하다.

### Icons

외부 icon package를 추가하지 않고 `Segoe MDL2 Assets`를 계속 사용한다. NuGet 추가 없이 설치 패키지 리스크를 줄이기 위해서다.

주요 아이콘:

- Send
- Stop
- Settings
- Copy
- New chat
- Delete
- Search
- Link/Cloud
- Local device

## Acceptance Checklist

- [ ] 첫 화면에서 제품명과 사용 목적이 3초 안에 이해된다.
- [ ] 새 대화, 질문 입력, 전송, 취소, 답변 복사가 명확하다.
- [ ] 답변 생성 중 사용자가 앱이 멈춘 것으로 오해하지 않는다.
- [ ] Online/Offline 모드가 header와 settings에서 일관되게 표시된다.
- [ ] 최소 창 크기 `780x560`에서 레이아웃이 깨지지 않는다.
- [ ] 긴 답변, 긴 질문, 여러 줄 입력, 오류 메시지가 모두 읽기 좋다.
- [ ] high contrast에 가까운 충분한 텍스트 대비를 유지한다.
- [ ] 설치 패키지별 health check 정보가 설정 창에서 확인된다.

## Recommended First Sprint

1. `MainWindow.xaml` visual refresh
2. `ChatMessageViewModel` display role/timestamp/alignment property 추가
3. composer placeholder와 send/stop 상태 개선
4. process log 접힘 처리
5. `ChatbotSettingsWindow.xaml` mode selector 초안 적용
6. 최소 폭 smoke test와 Online 질의 1건, Offline 질의 1건 확인

이 순서가 가장 좋다. 설치 파일을 만들기 전에 사용자가 가장 많이 보는 화면의 신뢰감을 먼저 끌어올리고, 이후 installer-aware 설정 UX를 붙이면 패키지 분리 작업과 자연스럽게 이어진다.
