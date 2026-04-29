# FT.MAXMessenger

Клиент для работы с HTTP API MAX на .NET.

Библиотека упрощает интеграцию с ботами MAX и закрывает основные сценарии:
- получение информации о боте;
- отправка и чтение сообщений;
- работа с чатами и участниками;
- WebHook-подписки и long polling;
- получение ссылки для загрузки файлов, загрузка медиа и получение информации о видео;
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

### Загрузка файла и отправка сообщения с вложением

```csharp
using System.IO;

var upload = await client.CreateUpload(new MaxCreateUploadRequest
{
    Type = MaxUploadTypes.Video
});

using var stream = File.OpenRead("movie.mp4");
var payload = await client.UploadFile(upload.Url, stream, "movie.mp4", "video/mp4");

var messageWithAttachment = await client.SendMessage(new MaxSendMessageRequest
{
    ChatId = "<chat-id>",
    Text = "Видео из .NET",
    Attachments = new[]
    {
        new MaxAttachment
        {
            Type = MaxUploadTypes.Video,
            Payload = payload
        }
    }
});
```

Поддерживаемые типы загрузки:
- `MaxUploadTypes.Image`
- `MaxUploadTypes.Video`
- `MaxUploadTypes.Audio`
- `MaxUploadTypes.File`

## Что реализовано

В библиотеке уже реализованы разделы API:
- `Bots`
- `Messages`
- `Answers`
- `Subscriptions and updates`
- `Files and media`
- `Chats`

Полный список методов см. в файле [`src/FT.MAXMessenger/API_METHODS.md`](src/FT.MAXMessenger/API_METHODS.md).

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
- `UploadFile(...)`
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

Интеграционные тесты находятся в проекте `FT.MAXMessenger.Tests`.

Для запуска нужно настроить секреты:

```powershell
dotnet user-secrets set "MAX:AccessToken" "<access-token>" --project .\tests\FT.MAXMessenger.Tests\FT.MAXMessenger.Tests.csproj
dotnet user-secrets set "MAX:TestChatId" "<chat-id>" --project .\tests\FT.MAXMessenger.Tests\FT.MAXMessenger.Tests.csproj
```

Дополнительно можно использовать переменные окружения:
- `MAX_API_TOKEN`
- `MAX_TEST_CHAT_ID`

## Документация API

Официальная документация MAX API:
- `https://dev.max.ru/docs-api`