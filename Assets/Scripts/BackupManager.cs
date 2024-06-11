using System.IO;
using UnityEditor;
using UnityEngine;

public static class BackupManager
{
    public static string GetBackUpPath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "Backup");
    }


    public static void BackUpFolder(string folder, string backupFolder, bool restore = false)
    {
        string backUpPath = Path.Combine(GetBackUpPath(), backupFolder);

        string source, destination;

        if (!restore)
        {
            source = folder;
            destination = backUpPath;
        }
        else
        {
            source = backUpPath;
            destination = folder;
        }

        CopyDirectory(source, destination, true);

        Debug.Log("Backup terminato");
    }

    public static void BackUpAll(bool restore = false)
    {

        Debug.Log("Avvio backup di tutto");

        string persistent = Path.Combine(UnityEngine.Device.Application.persistentDataPath);
        string backUpPath = GetBackUpPath();

        //string backUpPath = Path.Combine(UnityEngine.Application.dataPath, "Backup");


        string source, destination;

        if (!restore)
        {
            source = persistent;
            destination = backUpPath;
        }
        else
        {
            source = backUpPath;
            destination = persistent;
        }             

        CopyDirectory(source, destination, true);

        Debug.Log("Backup terminato");
    }

    public static void DeleteBackupDirectory()
    {
        Directory.Delete(GetBackUpPath(), true);
        Directory.CreateDirectory(GetBackUpPath());
    }

    public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {

        // Get information about the source directory
        var dir = new DirectoryInfo(sourceDir);

        // Check if the source directory exists
        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        // Cache directories before we start copying
        DirectoryInfo[] dirs = dir.GetDirectories();

        // Create the destination directory
        Directory.CreateDirectory(destinationDir);

        // Get the files in the source directory and copy to the destination directory
        foreach (FileInfo file in dir.GetFiles())
        {

            string targetFilePath = Path.Combine(destinationDir, file.Name);
            try
            {
                file.CopyTo(targetFilePath, true);
            }
            catch (IOException)
            {
                // Debug.Log($"{file.Name} non sovrascritto.");
            }
        }

        // If recursive and copying subdirectories, recursively call this method
        if (recursive)
        {
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir, true);
            }
        }
    }
}