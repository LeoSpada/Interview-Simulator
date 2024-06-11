using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.WSA;

public static class BackupManager
{

    // FARE CON INVERSE
    // Mettere indirizzo A e B (sorgente / destinazione) e in base a restore assegni folder e backup folder

    public static void BackUpFolder(string folder, string backupFolder, bool restore = false)
    {
        string backUpPath = Path.Combine(UnityEngine.Application.dataPath, "Backup", backupFolder);

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
            
        CopyDir(source, destination);
    }

    public static void BackUpAll(bool restore = false)
    {
        string persistent = Path.Combine(UnityEngine.Device.Application.persistentDataPath);
        string backUpPath = Path.Combine(UnityEngine.Application.dataPath, "Backup");


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

        CopyDir(source,destination);
    }

    private static void CopyDir(string source, string destination)
    {
        if (!Directory.Exists(destination))
        {
            Directory.CreateDirectory(destination);
            FileUtil.ReplaceDirectory(source, destination);
        }

        else
        {
            Debug.Log("Cancello vecchio Backup");
            FileUtil.DeleteFileOrDirectory(destination);
            FileUtil.CopyFileOrDirectory(source, destination);
        }
    }
}
