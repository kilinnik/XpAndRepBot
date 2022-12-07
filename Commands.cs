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
using Microsoft.EntityFrameworkCore;

namespace XpAndRepBot
{
    public static class Commands
    {
        public static string Me(Update update, InfoContext db)
        {
            var idUser = update.Message.From.Id;
            var user = db.TableUsers.First(x => x.Id == idUser);
            string result = $"Имя: {user.Name}\r\nLvl: {user.Lvl}({user.CurXp}/{Сalculation.Genlvl(user.Lvl + 1)})\r\nМесто в топе по уровню: {Сalculation.PlaceLvl(user.Id, db.TableUsers)}\r\nRep: {user.Rep}\r\nМесто в топе по репутации: {Сalculation.PlaceRep(user.Id, db.TableUsers)}";           
            return result;
        }
        public static string TopLvl(InfoContext db)
        {
            var users = db.TableUsers
            .OrderByDescending(b => b.Lvl).ThenByDescending(n => n.CurXp);
            string result = "";
            int i = 1;
            foreach (var user in users)
            {
                if (i == 51) break;
                if(i == 50) result += $"{i}. {user.Name} lvl {user.Lvl}({user.CurXp}/{Сalculation.Genlvl(user.Lvl + 1)})";
                else result += $"{i}. {user.Name} lvl {user.Lvl}({user.CurXp}/{Сalculation.Genlvl(user.Lvl + 1)})\r\n";
                i++;
            }
            return result;
        }
        public static string TopRep(InfoContext db)
        {
            var users = db.TableUsers
            .OrderByDescending(b => b.Rep);
            string result = "";
            int i = 1;
            foreach (var user in users)
            {
                if (i == 51) break;
                if (i == 50) result += $"{i}. {user.Name} rep {user.Rep}";
                else result += $"{i}. {user.Name} rep {user.Rep}\r\n";
                i++;
            }
            return result;
        }
    }
}
