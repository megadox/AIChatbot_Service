# Activity: PostgreSQLUpdate

## Summary
PostgreSQL 데이터베이스에서 테이블의 내용을 업데이트하는 액티비티

## Metadata
- group: `APP`
- script: `APP.postgre_update()`
- pattern: `APP\.postgre_update\(`
- dependencies: `APP`

## Properties
| Name | Type | Default | Options | Description |
|---|---|---|---|---|
| `dbId` | `string` | `-` | - | 데이터베이스 사용자 아이디를 지정합니다.<br/>ex) "batem" |
| `dbName` | `string` | `-` | - | 연결할 DB 이름을 지정합니다.<br/>ex) "batem_db" |
| `dbPw` | `string` | `-` | - | 데이터베이스 사용자 비밀번호를 지정합니다.<br/>ex) "batem12345" |
| `host` | `string` | `-` | - | 데이터베이스 서버의 주소를 지정합니다.<br/>ex)"123.456.789.0" |
| `port` | `string` | `-` | - | 데이터베이스 서버의 포트 번호를 지정합니다.<br/>ex) 4321 |
| `sql` | `string` | `-` | - | 데이터베이스 UPDATE 명령어를 지정합니다.<br/>ex) "UPDATE employees SET addr='Bundang, Kyunggi' WHERE id=3;" |

## Property Notes
### `dbId`
데이터베이스 사용자 아이디를 지정합니다.
ex) "batem"

### `dbName`
연결할 DB 이름을 지정합니다.
ex) "batem_db"

### `dbPw`
데이터베이스 사용자 비밀번호를 지정합니다.
ex) "batem12345"

### `host`
데이터베이스 서버의 주소를 지정합니다.
ex)"123.456.789.0"

### `port`
데이터베이스 서버의 포트 번호를 지정합니다.
ex) 4321

### `sql`
데이터베이스 UPDATE 명령어를 지정합니다.
ex) "UPDATE employees SET addr='Bundang, Kyunggi' WHERE id=3;"

