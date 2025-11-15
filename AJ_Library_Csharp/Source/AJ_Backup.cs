
using System;
using System.IO;

namespace AJ_Backup
{
    delegate void BackupLog(string message);


    class BackupSystem
    {
        public BackupLog? _backupLog;


        public string BackupDirectory(string pathToBackup, string backupDestination)
        {
            // Create a backup directory with a timestamp
            string backupDirectory = Path.Combine(backupDestination!, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            Directory.CreateDirectory(backupDirectory);

            // Get all files in the specified path
            string[] files = Directory.GetFiles(pathToBackup!, "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                // Get the relative path of the file from the backup source directory
                string relativePath = Path.GetRelativePath(pathToBackup!, file);
                string destinationPath = Path.Combine(backupDirectory, relativePath);

                // Create the destination directory if it doesn't exist
                string? destinationDirectory = Path.GetDirectoryName(destinationPath);
                Directory.CreateDirectory(destinationDirectory!);

                // Copy the file to the destination directory
                File.Copy(file, destinationPath, true);

                if (_backupLog != null)
                    _backupLog($"Copied: {file}");
            }

            if (_backupLog != null)
                _backupLog("Backup completed successfully.");

            return backupDirectory;
        }




    }
}
