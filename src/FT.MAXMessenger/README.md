# FT.MAXMessenger

Клиент для работы с HTTP API MAX на .NET.

Библиотека упрощает интеграцию с ботами MAX и закрывает основные сценарии:
- получение информации о боте;
- отправка и чтение сообщений;
- работа с чатами и участниками;
- WebHook-подписки и long polling;
- загрузка медиа и получение информации о видео;
- ответы на callback.

## Поддерживаемые платформы

- `netstandard2.0`
- `netstandard2.1`

## Установка

После публикации пакета в NuGet:

```powershell
dotnet add package FT.MAXMessenger
```

Namespace библиотеки:

```csharp
using FT.MAXMessenger;
```

## Быстрый старт

### Создание клиента

```csharp
using FT.MAXMessenger;

var client = new MaxClient("<access-token>");
```

### Получение информации о боте

```csharp
var me = await client.GetMe();
Console.WriteLine($"Bot: {me.Name} (@{me.Username})");
```

### Отправка сообщения в чат

> `chatId` в этой библиотеке рассматривается как `string`.

```csharp
var message = await client.SendMessage(new MaxSendMessageRequest
{
    ChatId = "<chat-id>",
    Text = "Привет из .NET"
});
```

### Получение сообщений из чата

```csharp
var messages = await client.GetMessages(new MaxMessagesQuery
{
    ChatId = "<chat-id>",
    Count = 20
});
```

### Создание WebHook-подписки

```csharp
var result = await client.CreateSubscription(new MaxCreateSubscriptionRequest
{
    Url = "https://example.com/max/webhook"
});
```

### Получение обновлений через long polling

```csharp
var updates = await client.GetUpdates(new MaxUpdatesQuery
{
    Limit = 100,
    Timeout = 30
});
```

## Что реализовано

В библиотеке уже реализованы разделы API:
- `Bots`
- `Messages`
- `Answers`
- `Subscriptions and updates`
- `Files and media`
- `Chats`

Полный список методов см. в файле [`API_METHODS.md`](API_METHODS.md).

## Основные возможности `MaxClient`

### Bots
- `GetMe()`

### Messages
- `GetMessages(...)`
- `SendMessage(...)`
- `EditMessage(...)`
- `GetMessage(...)`

### Answers
- `AnswerCallback(...)`

### Subscriptions and updates
- `GetSubscriptions()`
- `CreateSubscription(...)`
- `DeleteSubscription(...)`
- `GetUpdates(...)`

### Files and media
- `CreateUpload(...)`
- `GetVideo(...)`

### Chats
- `GetChats(...)`
- `GetChat(...)`
- `UpdateChat(...)`
- `DeleteChat(...)`
- `SendChatAction(...)`
- `GetPinnedMessage(...)`
- `SetPinnedMessage(...)`
- `DeletePinnedMessage(...)`
- `GetMyChatMembership(...)`
- `LeaveChat(...)`
- `GetChatAdmins(...)`
- `AddChatAdmins(...)`
- `RemoveChatAdmin(...)`
- `GetChatMembers(...)`
- `AddChatMembers(...)`
- `RemoveChatMember(...)`

## Тесты

Интеграционные тесты находятся в проекте `MAXMessenger.Tests`.

Для запуска нужно настроить секреты:

```powershell
dotnet user-secrets set "MAX:AccessToken" "<access-token>" --project .\MAXMessenger.Tests\MAXMessenger.Tests.csproj
dotnet user-secrets set "MAX:TestChatId" "<chat-id>" --project .\MAXMessenger.Tests\MAXMessenger.Tests.csproj
```

Дополнительно можно использовать переменные окружения:
- `MAX_API_TOKEN`
- `MAX_TEST_CHAT_ID`

## Документация API

Официальная документация MAX API:
- `https://dev.max.ru/docs-api`
