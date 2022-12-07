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
using System.Collections.Generic;

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
                        db.Add(new Users(idUser, update.Message.From.FirstName + " " + update.Message.From.LastName, 0, 0, 0));
                    }
                    db.SaveChanges();
                    var user = db.TableUsers.First(x => x.Id == idUser);
                    //if (update.Message.Caption != null) Console.WriteLine(update.Message.Caption.GetType());
                    //Console.WriteLine(update?.Message?.Text != null);
                    if (update?.Message?.Text != null || update?.Message?.Caption?.GetType() == typeof(string))
                    {
                        if (update.Message.Caption == null) user.CurXp += update.Message.Text.ToString().Length;
                        else user.CurXp += update.Message.Caption.Length;
                        while (user.CurXp >= Сalculation.Genlvl(user.Lvl + 1))
                        {
                            user.Lvl++;
                            user.CurXp -= Сalculation.Genlvl(user.Lvl);
                            //await botClient.SendTextMessageAsync(update.Message.Chat, $"{user.Name} получает {user.Lvl} lvl");
                            await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, replyToMessageId: update.Message.MessageId, text: $"{user.Name} получает {user.Lvl} lvl");
                        }
                        db.SaveChanges();
                    }
                    if (update.Message.ReplyToMessage != null && !update.Message.ReplyToMessage.From.IsBot) RepUp(botClient, update, cancellationToken, db);
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
                        case
                        {
                            Type: UpdateType.Message,
                            Message: { Text: { } text, Chat: { } chat },
                        } when text.Equals("/toprep@XpAndRepBot"):
                            {
                                //await botClient.SendTextMessageAsync(chat!, Commands.TopLvl(db), cancellationToken: cancellationToken);
                                await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, replyToMessageId: update.Message.MessageId, text: Commands.TopRep(db));
                                break;
                            }
                        case
                        {
                            Type: UpdateType.Message,
                            Message: { Text: { } text, Chat: { } chat },
                        } when text.Equals("/rules@XpAndRepBot"):
                            {
                                //await botClient.SendTextMessageAsync(chat!, Commands.TopLvl(db), cancellationToken: cancellationToken);
                                await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, replyToMessageId: update.Message.MessageId, text: "Правила: бан за: 1. Флуд 2. Спам 3. Порно 4. Оскорбление админа 5. Попрошайничество (из коммов один раз простительно) 6. Поддержку РФ и против Украины.\r\nВ случае отсутствия админа или модера начать голосование за бан по причинам выше можно с помощью команды /voteban. Человек, начавший голосование без причины, а также все, кто за в данном голосовании, получат бан.\r\n7. За 3 варна (1 варн снимается за 30 дней без варнов, кидаете модеру сообщение с ссылкой на последний варн) \r\nПричины для варна: любое искажение правил, бессмысленные сообщения, личные оскорбления, громкие ГС, небольшой флуд/флуд негативными эмоциями, старт голосования не по правилам и если это голосование было сразу удалено модером (список будет пополняться).\r\nВ остальных случаях голосование за бан создаёт модератор для нарушающих общественное спокойствие чата (требующий голосование должен собрать 3 лайка (свой не считается) на своём сообщении). \r\nГолосование за роль создаёт модератор или чел сам себе, или другому по его согласию (роль не должна быть связана с администраторской деятельностью), за нарушение бан.");
                                break;
                            }
                        case
                        {
                            Type: UpdateType.Message,
                            Message: { Text: { } text, Chat: { } chat },
                        } when text.Equals("/help@XpAndRepBot"):
                            {
                                //await botClient.SendTextMessageAsync(chat!, Commands.TopLvl(db), cancellationToken: cancellationToken);
                                await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, replyToMessageId: update.Message.MessageId, text: "/me@XpAndRepBot - информация о себе\r\n/toplvl@XpAndRepBot - топ по уровню\r\n/toprep@XpAndRepBot - топ по репутации\r\n/rules@XpAndRepBot - правила чата\r\n/mesrep@XpAndRepBot - список сообщений, повышающих репутацию\r\n/games@XpAndRepBot - ссылки на игры в чате");
                                break;
                            }
                        case
                        {
                            Type: UpdateType.Message,
                            Message: { Text: { } text, Chat: { } chat },
                        } when text.Equals("/mesrep@XpAndRepBot"):
                            {
                                //await botClient.SendTextMessageAsync(chat!, Commands.TopLvl(db), cancellationToken: cancellationToken);
                                await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, replyToMessageId: update.Message.MessageId, text: "Собщения, увеличивающие репутацию: +, спс, спасибо, пасиб, сяб, класс, молодец, жиза, 👍, 👍🏼, 👍🏽, 👍🏾, 👍🏿");
                                break;
                            }
                        case
                        {
                            Type: UpdateType.Message,
                            Message: { Text: { } text, Chat: { } chat },
                        } when text.Equals("/games@XpAndRepBot"):
                            {
                                //await botClient.SendTextMessageAsync(chat!, Commands.TopLvl(db), cancellationToken: cancellationToken);
                                await botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, replyToMessageId: update.Message.MessageId, text: "#games Ссылки на все игры, можете предлагать ещё игры \r\nhttps://t.me/Igruha_chat/821741 math battle \r\nhttps://t.me/Igruha_chat/821792 Corsairs\r\nhttps://t.me/Igruha_chat/821793 LumberJack\r\nhttps://t.me/Igruha_chat/822251 keep it up\r\nhttps://t.me/Igruha_chat/822191 atomic drop\r\nhttps://t.me/Igruha_chat/822162 motofx 2\r\nhttps://t.me/Igruha_chat/826931 tube runner");
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
        public void RepUp(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, InfoContext db)
        {
            List<string> repWords = new List<string>() { "+", "спс", "спасибо", "пасиб", "сяб", "класс", "молодец", "жиза", "👍", "👍🏼", "👍🏽", "👍🏾", "👍🏿" };
            if (update.Message.ReplyToMessage.From.Id != update.Message.From.Id)
                if (update?.Message?.Text != null)
                {
                    if (repWords.Any(x => update.Message.Text.Contains(x)))
                    {
                        var idUser = update.Message.ReplyToMessage.From.Id;
                        var user = db.TableUsers.First(x => x.Id == idUser);
                        user.Rep++;
                        db.SaveChanges();
                        var user1 = db.TableUsers.First(x => x.Id == update.Message.From.Id);
                        botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, replyToMessageId: update.Message.ReplyToMessage.MessageId, text: $"{user1.Name}({user1.Rep}) увеличил вашу репутацию на 1({user.Rep})");
                    }
                }
                else if (update?.Message?.Caption?.GetType() == typeof(string))
                    if (repWords.Any(x => update.Message.Caption.Contains(x)))
                    {
                        var idUser = update.Message.ReplyToMessage.From.Id;
                        var user = db.TableUsers.First(x => x.Id == idUser);
                        user.Rep++;
                        db.SaveChanges();
                        var user1 = db.TableUsers.First(x => x.Id == update.Message.From.Id);
                        botClient.SendTextMessageAsync(chatId: update.Message.Chat.Id, replyToMessageId: update.Message.ReplyToMessage.MessageId, text: $"{user1.Name}({user1.Rep}) увеличил вашу репутацию на 1({user.Rep})");
                    }
        }
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.Error.WriteLine(exception);
            return Task.CompletedTask;
        }
    }
}
