using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class BackupManager
{

    public static void BackUpFolder(string folder, string backupFolder)
    {
        string backUpPath = Path.Combine(Application.dataPath, "Backup", backupFolder);

        if (!Directory.Exists(backUpPath))
        {
            Directory.CreateDirectory(backUpPath);
            // FileUtil.CopyFileOrDirectory(GetCVFolder(), backUpPath);
            FileUtil.ReplaceDirectory(folder, backUpPath);
        }

        else
        {
            Debug.Log("Cancello vecchio Backup");
            FileUtil.DeleteFileOrDirectory(backUpPath);
            FileUtil.CopyFileOrDirectory(folder, backUpPath);
        }
    }
}
