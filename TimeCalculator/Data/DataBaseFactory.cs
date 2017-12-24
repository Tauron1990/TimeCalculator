using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace TimeCalculator.Data
{
    public static class DataBaseFactory
    {
        private static readonly HashSet<string> AppliedMigrations = new HashSet<string>();

        private static string UserName => Properties.Settings.Default.CurrentUser;

        public static string UserNameOverride { get; set; }

        public static string DatabasePath
        {
            get
            {
                string name = string.IsNullOrWhiteSpace(UserNameOverride) ? UserName : UserNameOverride;
                if (string.IsNullOrEmpty(name))
                    name = "Common";

                name = name + ".db";

                return Path.Combine(FilesHelper.BasePath, name);
            }
        }

        public static JobDatabase CreateDatabase()
        {
            JobDatabase database = new JobDatabase();

            if (AppliedMigrations.Add(UserName))
                database.Database.Migrate();

            return database;
        }
    }
}