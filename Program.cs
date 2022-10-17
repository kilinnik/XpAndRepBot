using System;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using XpAndRepBot;
using System.Linq;

var botClient = new TelegramBotClient("5759112130:AAHQm9muuBF5YXcQ3XzZQTnEp8MY9QnXNZM");

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, _) => cts.Cancel(); // Чтобы отловить нажатие ctrl+C и всякие sigterm, sigkill, etc

var handler = new UpdateHandler();
var receiverOptions = new ReceiverOptions();
botClient.StartReceiving(handler, receiverOptions, cancellationToken: cts.Token);

//using var db = new InfoContext();
//db.Add(new Users (1,"0", 1, 1));
//db.SaveChanges();

//var users = db.TableUsers
//    .OrderBy(b => b.Id)
//    .First();

Console.WriteLine("Bot started. Press ^C to stop");
await Task.Delay(-1, cancellationToken: cts.Token); // Такой вариант советуют MS: https://github.com/dotnet/runtime/issues/28510#issuecomment-458139641
Console.WriteLine("Bot stopped");
