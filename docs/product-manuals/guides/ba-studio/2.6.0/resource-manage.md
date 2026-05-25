# BA-Studio Resource 관리 방법

Product: BA-Studio
Version: 2.6.0
GuideType: how-to
Topic: resource-manage
SourceManual: docs/product-manuals/normalized/ba-studio/2.6.0.md
SourceSections:
- 사용자 매뉴얼 > 목차 > 2-5. Properties / Library / Images / Resource
- Source Code Supplement > Source-Confirmed Project Bundle Structure

## User Intent

사용자가 프로젝트에 종속된 리소스 파일, 이미지, 라이브러리 파일을 어디서 관리하는지 알고 싶어 한다.

대표 질문:
- Resource 창 사용법을 알려줘.
- 프로젝트 리소스는 어디서 관리해?
- 이미지나 파일 리소스를 확인하려면?
- Resource 폴더에는 뭐가 들어가?

## Short Answer

프로젝트 리소스는 Project 파일이 열린 상태에서 `Resource` 창에서 관리한다. 일반 파일, 이미지 리소스, 사용자 라이브러리 등 프로젝트 실행과 패키징에 필요한 보조 파일을 확인하고 관리하는 용도이다.

## Steps

1. BA-Studio에서 프로젝트를 연다.
2. Properties/Library/Images/Resource 영역을 확인한다.
3. `Resource` 탭 또는 Resource 관리창을 연다.
4. 프로젝트에 포함된 보조 파일을 확인한다.
5. 이미지 리소스는 Images 창에서도 확인한다.
6. 필요한 파일이 프로젝트 리소스에 포함되어 있는지 확인한다.
7. 패키징 또는 실행 전 누락된 리소스가 없는지 점검한다.

## Notes

- Resource 창은 Project 파일이 열린 경우 프로젝트에 종속된 파일 구조를 보여준다.
- 이미지 캡처 파일은 이미지 리소스 영역에 등록된다.
- 리소스, 이미지, 셀렉터, 사용자 라이브러리는 실행이나 패키징 시 함께 참조될 수 있다.

## Answer Style

사용자에게 내부 폴더 구조를 장황하게 설명하기보다 Resource 창의 용도와 확인 절차를 중심으로 답변한다.

## Related Keywords

- Resource
- Resource 창
- Images
- 이미지 리소스
- 프로젝트 리소스
- 패키징 리소스

## Example Answer

프로젝트 리소스는 프로젝트를 연 상태에서 `Resource` 창에서 확인합니다. 이 창은 프로젝트에 종속된 파일을 관리하는 영역이며, 이미지 리소스는 Images 창에서도 확인할 수 있습니다. 실행이나 패키징 전에 필요한 리소스가 포함되어 있는지 점검하세요.
