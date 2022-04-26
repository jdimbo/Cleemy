using Cleemy.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleemy.Infrastructure.Data
{
    public class CleemyDbContext : DbContext
    {
        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }


        public string DbPath { get; }

        public CleemyDbContext()
        {
            //todo change for production
            // C:\Users\[username]\AppData\Local
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "CleemyDatabase.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
