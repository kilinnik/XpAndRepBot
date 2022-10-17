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
            string result = $"Имя: {user.Name}\r\nLvl: {user.Lvl}({user.CurXp}/{Math.Genlvl(user.Lvl + 1)})\r\nМесто в рейтинге по уровню: {Math.PlaceLvl(user.Id, db.TableUsers)}";           
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
                if(i == 50) result += $"{i}. {user.Name} lvl {user.Lvl}({user.CurXp}/{Math.Genlvl(user.Lvl + 1)})";
                else result += $"{i}. {user.Name} lvl {user.Lvl}({user.CurXp}/{Math.Genlvl(user.Lvl + 1)})\r\n";
                i++;
            }
            return result;
        }
    }
    public static class Math
    {
        public static int PlaceLvl(long idUser, DbSet<Users> TableUsers)
        {
            var users = TableUsers
            .OrderByDescending(b => b.Lvl).ThenByDescending(n => n.CurXp);
            int result = 1;
            foreach (var user in users)
            {
                if (idUser == user.Id) break;
                result++;
            }
            return result;
        }
        public static int Genlvl(int x)
        {
            if (x == 0) return x;
            else if (x == 1) return 100;
            if (x % 2 == 0) x = 2 * Genlvl(x - 1) - Genlvl(x - 2) + 35;
            else x = 2 * Genlvl(x - 1) - Genlvl(x - 2) + 35 + 100;
            return x;
        }
    }
}
