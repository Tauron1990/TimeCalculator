using System;
using System.IO;

namespace TimeCalculator.Data
{
    public static class FilesHelper
    {
        public static readonly string BasePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Tauron\\TimeCalculator");

        public static readonly string SeassionFilePath = Path.Combine(BasePath, "Seassion.db");

    }
}