using System;
using System.IO;

namespace TimeCalculator.Data
{
    public static class FilesHelper
    {
        private static string _basePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Tauron\\TimeCalculator");

        public static string SeassionFilePath = Path.Combine(_basePath, "Seassion.db");

    }
}