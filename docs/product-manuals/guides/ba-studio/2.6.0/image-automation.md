# BA-Studio 이미지 기반 자동화 작성 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: image-automation
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 3-2-1. 태스크 구현의 주요방법 > Image 인식기반 구현방법

## User Intent

사용자가 화면 이미지를 캡처해서 Wait for Image, Click on Image 같은 이미지 기반 자동화를 작성하는 방법을 알고 싶어 한다.

대표 질문:
- 이미지 기반 자동화 방법을 알려줘.
- Image Capture는 어떻게 써?
- 화면 이미지를 기다렸다가 클릭하려면?
- Wait for Image와 Click on Image 사용 순서는?

## Short Answer

이미지 기반 자동화는 대상 화면 이미지를 `Image Capture`로 캡처해 리소스에 저장한 뒤, `Wait for Image`나 `Click on Image` Activity에서 해당 이미지 이름을 사용해 화면 대기나 클릭을 수행한다.

## Steps

1. 자동화할 화면을 준비한다.
2. Task Editor에 필요한 Activity를 배치한다.
3. 이미지가 나타날 때까지 기다릴 경우 `WIN32/Wait for Image`를 사용한다.
4. 이미지를 클릭할 경우 `WIN32/Click on Image`를 사용한다.
5. 대상 Activity를 선택한 상태에서 `Image Capture` 버튼을 클릭한다.
6. 캡처할 영역을 드래그해 선택한다.
7. 캡처를 완료하고 이미지 이름을 지정한다.
8. 저장된 이미지 이름을 Activity 속성에 입력한다.
9. 필요한 경우 클릭 기준 위치를 조정한다.
10. Task를 실행해 이미지 탐색과 클릭이 정상 동작하는지 확인한다.

## Notes

- 캡처된 이미지는 프로젝트 리소스에 등록된다.
- 화면 해상도, 배율, UI 테마가 바뀌면 이미지 인식 결과가 달라질 수 있다.
- `Click on Image`는 이미지를 찾은 뒤 클릭하므로 단순 대기 단계가 불필요한 경우도 있다.

## Answer Style

캡처, 이미지 이름 지정, Activity 속성 연결 순서로 안내한다.

## Related Keywords

- Image Capture
- Image View
- Wait for Image
- Click on Image
- 이미지 자동화
- 이미지 리소스

## Example Answer

이미지 기반 자동화는 먼저 대상 화면을 `Image Capture`로 캡처해 이미지 리소스로 저장한 뒤, `Wait for Image`나 `Click on Image` Activity의 이미지 속성에 해당 이미지 이름을 입력해 사용합니다.
