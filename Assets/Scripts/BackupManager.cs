using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

public static class BackupManager
{

    public static void BackUpFolder(string folder, string backupFolder)
    {
        string backUpPath = Path.Combine(UnityEngine.Application.dataPath, "Backup", backupFolder);

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

    public static void BackUpAll()
    {
        string persistent = Path.Combine(UnityEngine.Device.Application.persistentDataPath);
        string backUpPath = Path.Combine(UnityEngine.Application.dataPath, "Backup");

        if (!Directory.Exists(backUpPath))
        {
            Directory.CreateDirectory(backUpPath);
            // FileUtil.CopyFileOrDirectory(GetCVFolder(), backUpPath);
            FileUtil.ReplaceDirectory(persistent, backUpPath);
        }

        else
        {
            Debug.Log("Cancello vecchio Backup");
            FileUtil.DeleteFileOrDirectory(backUpPath);
            FileUtil.CopyFileOrDirectory(persistent, backUpPath);
        }
    }
}
