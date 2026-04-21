# MAXMessenger.Tests

Интеграционные тесты для `FT.MAXMessenger`.

## Настройка User Secrets

Выполнить из корня решения:

```powershell
dotnet user-secrets set "MAX:AccessToken" "<your-token>" --project .\MAXMessenger.Tests\MAXMessenger.Tests.csproj
dotnet user-secrets set "MAX:TestChatId" "<chat-id>" --project .\MAXMessenger.Tests\MAXMessenger.Tests.csproj
```

## Альтернатива

Можно использовать переменные окружения:

```powershell
$env:MAX_API_TOKEN = "<your-token>"
$env:MAX_TEST_CHAT_ID = "<chat-id>"
```

## Что покрывают интеграционные тесты

- `GetMe_ReturnsCurrentBot`
- `GetSubscriptions_ReturnsResponse`
- `GetChats_ReturnsResponse`
- `GetChat_ReturnsConfiguredChat`
- `GetMessages_ByChat_ReturnsResponse`
- `GetMessage_WhenChatHasMessages_ReturnsMessage`
- `SendMessage_ToTestChat_ReturnsCreatedMessage`
- `CreateUpload_ForVideo_ReturnsUploadUrlAndToken`

## Поведение тестов

- если не настроен `MAX:AccessToken`, интеграционные тесты будут пропущены;
- если не настроен `MAX:TestChatId`, будут пропущены тесты, которым нужен тестовый чат.
