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
    public static class Сalculation
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
        public static int PlaceRep(long idUser, DbSet<Users> TableUsers)
        {
            var users = TableUsers.OrderByDescending(b => b.Rep);
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
            int[] xplvl = new int[20] { 100, 235, 505, 810, 1250, 1725, 2335, 2980, 3760, 4575, 5525, 6510, 7630, 8785, 10075, 11400, 12860, 14355, 15985, 17650 };
            //if (x == 0) return x;
            //else if (x == 1) return 100;
            //if (x % 2 == 0) x = 2 * Genlvl(x - 1) - Genlvl(x - 2) + 35;
            //else x = 2 * Genlvl(x - 1) - Genlvl(x - 2) + 35 + 100;
            return xplvl[x - 1];
        }
    }
}
