using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TimeCalculator.Data
{
    public sealed class JobDatabase : DbContext
    {
        public DbSet<JobEntity> JopEntities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string pathFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Tauron", "TimeCalculator");
            if (!Directory.Exists(pathFolder))
                Directory.CreateDirectory(pathFolder);


            optionsBuilder.UseSqlite(new SqliteConnectionStringBuilder {DataSource = Path.Combine(pathFolder, "data.db")}.ToString());
            base.OnConfiguring(optionsBuilder);
        }
    }
}