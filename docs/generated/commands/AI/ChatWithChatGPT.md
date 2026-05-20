# Activity: ChatWithChatGPT

## Summary
API를 이용하여 ChatGPT에게 질문을 하고 답변을 받는 액티비티

## Metadata
- group: `AI`
- script: `AI.chat_with_chatgpt()`
- pattern: `AI\.chat_with_chatgpt\(`
- dependencies: `AI`
- theme: `Accent2_4`
- prefix: `response`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `apiKey` | `string` | `-` | - | API키를 지정합니다.<br/>ex) "sk-..." |
| `assistant` | `string` | `-` | - | 이전 메시지에 대한 모델의 응답을 입력합니다.<br/>(이전 답변과 연결을 원할 경우 사용한다.)<br/>ex) 2입니다. |
| `continue` | `string` | `False` | `True`, `False` | 답변 데이터를 이어서 받음 여부를 지정합니다.<br/>(사용할 경우 이전에 수신 받은 content를 assistant에 넣어줘야 한다.)<br/>ex) False |
| `endPoint` | `string` | `"https://api.openai.com/v1/chat/completions"` | - | Chat GPT API 엔드포인트를 지정합니다.<br/>ex) "https://api.openai.com/v1/chat/completions" |
| `images` | `string` | `-` | - | 입력에 사용할 이미지 URL 또는 base64 이미지를 리스트 안에 입력합니다.<br/>(gpt-4-vision 모델의 경우 사용이 가능하며 리스트의 형태로 전달한다.)<br/>ex) ['https://www.batem.com/image1.jpg'] |
| `maxTokens` | `string` | `-` | - | 요청과 응답을 모두 포함한 최대 토큰 수를 지정합니다.<br/>ex) 500 |
| `model` | `string` | `"gpt-3.5-turbo"` | `"gpt-3.5-turbo"`, `"gpt-3.5"`, `"gpt-4"`, `"gpt-4-vision-preview"` | 사용할 ChatGPT 엔진 모델을 지정합니다.<br/>ex) "gpt-3.5-turbo" |
| `n` | `string` | `1` | `1`, `2`, `3`, `4`, `5` | 답변의 개수를 지정합니다.<br/>ex) 1 |
| `returnType` | `string` | `"text"` | `"json"`, `"text"` | 리턴 타입을 지정합니다.<br/>ex) json<br/>"json": json 형태로 리턴합니다.<br/>"text": text 형태로 답변만 리턴합니다.(답변이 여러개일 경우 줄바꿈으로 구분됩니다.) |
| `system` | `string` | `-` | - | 동작 제어 지시용 메시지를 입력합니다.<br/>ex) "한국말로 말해줘" |
| `temperature` | `string` | `1` | `0`, `0.1`, `0.2`, `0.3`, `0.4`, `0.5`, `0.6`, `0.7`, `0.8`, `0.9`, `1` | 답변의 다양성 정도를 지정합니다.<br/>(지정된 값에 따라 답변이 달라집니다.0~1 사이의 값만 가능하다.)<br/>ex) 1 |
| `user` | `string` | `-` | - | 사용자의 기본 요청 메시지를 입력합니다.<br/>ex) "1+1이 뭐야?" |

## Property Notes
### `apiKey`
API키를 지정합니다.
ex) "sk-..."

### `assistant`
이전 메시지에 대한 모델의 응답을 입력합니다.
(이전 답변과 연결을 원할 경우 사용한다.)
ex) 2입니다.

### `continue`
답변 데이터를 이어서 받음 여부를 지정합니다.
(사용할 경우 이전에 수신 받은 content를 assistant에 넣어줘야 한다.)
ex) False

### `endPoint`
Chat GPT API 엔드포인트를 지정합니다.
ex) "https://api.openai.com/v1/chat/completions"

### `images`
입력에 사용할 이미지 URL 또는 base64 이미지를 리스트 안에 입력합니다.
(gpt-4-vision 모델의 경우 사용이 가능하며 리스트의 형태로 전달한다.)
ex) ['https://www.batem.com/image1.jpg']

### `maxTokens`
요청과 응답을 모두 포함한 최대 토큰 수를 지정합니다.
ex) 500

### `model`
사용할 ChatGPT 엔진 모델을 지정합니다.
ex) "gpt-3.5-turbo"

### `n`
답변의 개수를 지정합니다.
ex) 1

### `returnType`
리턴 타입을 지정합니다.
ex) json
"json": json 형태로 리턴합니다.
"text": text 형태로 답변만 리턴합니다.(답변이 여러개일 경우 줄바꿈으로 구분됩니다.)

### `system`
동작 제어 지시용 메시지를 입력합니다.
ex) "한국말로 말해줘"

### `temperature`
답변의 다양성 정도를 지정합니다.
(지정된 값에 따라 답변이 달라집니다.0~1 사이의 값만 가능하다.)
ex) 1

### `user`
사용자의 기본 요청 메시지를 입력합니다.
ex) "1+1이 뭐야?"

