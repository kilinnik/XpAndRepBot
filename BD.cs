using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace XpAndRepBot
{
    public class Users
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Lvl { get; set; }
        public int CurXp { get; set; }
        public Users (long id, string name, int lvl, int curXp)
        {
            Id = id; Name = name;  Lvl = lvl; CurXp = curXp;
        }
    }
    public class InfoContext : DbContext
    {
        public DbSet<Users> TableUsers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=HOME-PC;Initial Catalog=BD_Users;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
