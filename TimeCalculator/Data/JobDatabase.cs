using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TimeCalculator.Data
{
    public sealed class JobDatabase : DbContext
    {
        public DbSet<JobEntity> JobEntities { get; set; }

        public DbSet<SetupEntity> SetupEntities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string fileName = DataBaseFactory.DatabasePath;
            string pathFolder = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(pathFolder))
                Directory.CreateDirectory(pathFolder ?? throw new InvalidOperationException());


            optionsBuilder.UseSqlite(new SqliteConnectionStringBuilder {DataSource = fileName}.ToString());
            base.OnConfiguring(optionsBuilder);
        }
    }
}