# Activity: ChatWithGemini

## Summary
API를 이용하여 Gemini에게 질문을 하고 답변을 받는 액티비티

## Metadata
- group: `AI`
- script: `AI.chat_with_gemini()`
- pattern: `AI\.chat_with_gemini\(`
- dependencies: `AI`
- theme: `Accent2_4`
- prefix: `response`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `apiKey` | `string` | `-` | - | API키를 지정합니다.<br/>ex) "ABCDEF-EF..." |
| `endPoint` | `string` | `"https://generativelanguage.googleapis.com/v1/"` | - | Gemini API 엔드포인트를 지정합니다.<br/>ex) "https://generativelanguage.googleapis.com/v1/" |
| `image` | `string` | `-` | - | 첨부할 이미지의 주소를 작성합니다.<br/>ex) "C:\imgs\image\image1.png" |
| `maxTokens` | `string` | `-` | - | 응답 최대 토큰 수를 지정합니다.<br/>ex) 500 |
| `model` | `string` | `"gemini-1.5-flash"` | `"gemini-1.5-pro"`, `"gemini-1.5-flash"`, `"gemini-1.0-pro"` | 사용할 Gemini 엔진 모델을 지정합니다.<br/>ex) "gemini-1.5-flash" |
| `returnType` | `string` | `"text"` | `"json"`, `"text"` | 리턴 타입을 지정합니다.<br/>ex) json<br/>"json": json 형태로 리턴합니다.<br/>"text": text 형태로 답변만 리턴합니다. |
| `temperature` | `string` | `1` | `0`, `0.2`, `0.4`, `0.6`, `0.8`, `1`, `1.2`, `1.4`, `1.6`, `1.8`, `2` | 답변의 다양성 정도를 지정합니다.<br/>(지정된 값에 따라 답변이 달라집니다.0~2 사이의 값만 가능하다.)<br/>ex) 1 |
| `user` | `string` | `-` | - | 사용자의 기본 요청 메시지를 입력합니다.<br/>ex) "1+1이 뭐야?" |

## Property Notes
### `apiKey`
API키를 지정합니다.
ex) "ABCDEF-EF..."

### `endPoint`
Gemini API 엔드포인트를 지정합니다.
ex) "https://generativelanguage.googleapis.com/v1/"

### `image`
첨부할 이미지의 주소를 작성합니다.
ex) "C:\imgs\image\image1.png"

### `maxTokens`
응답 최대 토큰 수를 지정합니다.
ex) 500

### `model`
사용할 Gemini 엔진 모델을 지정합니다.
ex) "gemini-1.5-flash"

### `returnType`
리턴 타입을 지정합니다.
ex) json
"json": json 형태로 리턴합니다.
"text": text 형태로 답변만 리턴합니다.

### `temperature`
답변의 다양성 정도를 지정합니다.
(지정된 값에 따라 답변이 달라집니다.0~2 사이의 값만 가능하다.)
ex) 1

### `user`
사용자의 기본 요청 메시지를 입력합니다.
ex) "1+1이 뭐야?"

