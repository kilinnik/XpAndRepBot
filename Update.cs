using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Linq;

namespace XpAndRepBot
{
    class UpdateHandler : IUpdateHandler
    {
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Debug.WriteLine(JsonSerializer.Serialize(update));
            using var db = new InfoContext();
            if (update.Message != null)
                if (!update.Message.From.IsBot)
                {
                    var idUser = update.Message.From.Id;
                    if (!db.TableUsers.Any(c => c.Id == idUser))
                    {
                        db.Add(new Users(idUser, update.Message.From.FirstName + " " + update.Message.From.LastName, 0, 0));
                    }
                    db.SaveChanges();
                    var user = db.TableUsers.First(x => x.Id == idUser);
                    //if (update.Message.Caption != null) Console.WriteLine(update.Message.Caption.GetType());
                    //Console.WriteLine(update?.Message?.Text != null);
                    if (update?.Message?.Text != null || update?.Message?.Caption?.GetType() == typeof(string))
                    {
                        if (update.Message.Caption == null) user.CurXp += update.Message.Text.ToString().Length;
                        else user.CurXp += update.Message.Caption.Length;
                        if (user.CurXp > Math.Genlvl(user.Lvl + 1))
                        {
                            user.Lvl++;
                            user.CurXp -= Math.Genlvl(user.Lvl);
                            //await botClient.SendTextMessageAsync(update.Message.Chat, $"{user.Name} получает {user.Lvl} lvl");
                            await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, replyToMessageId: update.Message.MessageId, text: $"{user.Name} получает {user.Lvl} lvl");
                        }
                        db.SaveChanges();
                    }

                    switch (update)
                    {
                        case
                        {
                            Type: UpdateType.Message,
                            Message: { Text: { } text, Chat: { } chat },
                        } when text.Equals("/start", StringComparison.OrdinalIgnoreCase):
                            {
                                await botClient.SendTextMessageAsync(chat!, "Всем привет", cancellationToken: cancellationToken);
                                break;
                            }
                        case
                        {
                            Type: UpdateType.Message,
                            Message: { Text: { } text, Chat: { } chat },
                        } when text.Equals("/me@XpAndRepBot"):
                            {
                                //await botClient.SendTextMessageAsync(chat!, Commands.Me(update, db), cancellationToken: cancellationToken);
                                await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, replyToMessageId: update.Message.MessageId, text: Commands.Me(update, db));
                                break;
                            }
                        case
                        {
                            Type: UpdateType.Message,
                            Message: { Text: { } text, Chat: { } chat },
                        } when text.Equals("/toplvl@XpAndRepBot"):
                            {
                                //await botClient.SendTextMessageAsync(chat!, Commands.TopLvl(db), cancellationToken: cancellationToken);
                                await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, replyToMessageId: update.Message.MessageId, text: Commands.TopLvl(db));
                                break;
                            }
                            //case
                            //{
                            //    Type: UpdateType.Message,
                            //    Message.Chat: { } chat
                            //}:
                            //    {
                            //        await botClient.SendTextMessageAsync(chat!, "", cancellationToken: cancellationToken);
                            //        break;
                            //    }
                    }
                }
        }
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.Error.WriteLine(exception);
            return Task.CompletedTask;
        }
    }
}
