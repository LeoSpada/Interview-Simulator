using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public static class CVManager
{
    public static CVEntry currentCV = null;

    public static bool editCurrent = false;

    private const string saveFolder = "Saves";

    public static string GetCVFolder()
    {
        string folder = Path.Combine(Application.persistentDataPath, saveFolder);
        try
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

        }
        catch (IOException ex)
        {
            Console.WriteLine(ex.Message);
        }

        return Path.Combine(Application.persistentDataPath, saveFolder);
    }

    public static int GetCVFolderSize()
    {
        return GetAllCV().Count;
    }

    public static FileInfo[] GetFilesInfo()
    {
        DirectoryInfo mainDir = new(GetCVFolder());
        var files = mainDir.GetFiles("*.json");
        return files;
    }

    static string GetCVFilePath(string name, string surname)
    {
        string folder = GetCVFolder();
        string path = Path.Combine(folder, $"{name}_{surname}_CV.json");
        return path;
    }

    // Restituisce la lista di tutti i CV nella cartella di salvataggio
    public static List<CVEntry> GetAllCV()
    {
        List<CVEntry> list = new();

        // DirectoryInfo dir = new(Path.Combine(Application.persistentDataPath, saveFolder));
        FileInfo[] info = GetFilesInfo();

        foreach (FileInfo f in info)
        {
            string json = File.ReadAllText(f.ToString());
            var loadedCV = JsonConvert.DeserializeObject<CVEntry>(json);
            list.Add(loadedCV);
        }

        return list;
    }

    public static CVEntry GetCVEntry(string name, string surname)
    {
        if (!File.Exists(GetCVFilePath(name, surname)))
        {
            return null;
        }

        string json = File.ReadAllText(GetCVFilePath(name, surname));

        var loadedCV = JsonConvert.DeserializeObject<CVEntry>(json);

        return loadedCV;
    }

    public static CVEntry GetRandomCVEntry()
    {
        List<CVEntry> list = GetAllCV();
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static void AddCVEntry(CVEntry cvEntry)
    {
        string json = JsonConvert.SerializeObject(cvEntry);
        File.WriteAllText((GetCVFilePath(cvEntry.name, cvEntry.surname)), json);

        // COPIA IN CARTELLA BACKUP
        BackUpFolder();
    }


    public static void BackUpFolder()
    {
        string backUpPath = Path.Combine(Application.dataPath, "Backup", saveFolder);

        if (!Directory.Exists(backUpPath))
        {
            Directory.CreateDirectory(backUpPath);
            // FileUtil.CopyFileOrDirectory(GetCVFolder(), backUpPath);
            FileUtil.ReplaceDirectory(GetCVFolder(), backUpPath);
        }

        else
        {
            Debug.Log("Cancello vecchio Backup");
            FileUtil.DeleteFileOrDirectory(backUpPath);
            FileUtil.CopyFileOrDirectory(GetCVFolder(), backUpPath);
        }
    }


    public static void RemoveCVEntry(CVEntry cvEntry)
    {
        if (CheckEntry(cvEntry))
            File.Delete(GetCVFilePath(cvEntry.name, cvEntry.surname));

        BackUpFolder();
    }

    public static bool CheckEntry(CVEntry cvEntry)
    {
        if (!File.Exists(GetCVFilePath(cvEntry.name, cvEntry.surname)))
            return false;
        return true;
    }

    public static bool IsCurrentCV(string name, string surname)
    {
        if (currentCV.name != name || currentCV.surname != surname)
        {
            Debug.Log(name + "non corrisponde");
            return false;
        }

        else return true;
    }

    public static void UnloadCurrentCV()
    {
        currentCV = null;
    }


    // Usata per debug. Stampa a schermo il cv formattato.
    public static void DebugCV(CVEntry cv)
    {
        Debug.Log("Nome: " + cv.name + " " + cv.surname + " , " + cv.gender + "\nOccupazione desiderata: " + cv.job);
    }
}


[System.Serializable]
public class CVEntry
{
    public string name;
    public string surname;
    public Occupazione job;
    public Genere gender;
    //public Lingua linguaMadre;
    //public Lingua linguaSecondaria;
    // public Patente patente;
    // AGGIUNGERE QUI TUTTI I CAMPI e rigenerare costruttore
    // VEDERE DATI_CURRICULUM e altri


    public CVEntry()
    {

    }

    public CVEntry(string name, string surname, Occupazione job, Genere gender)
    {
        this.name = name;
        this.surname = surname;
        this.job = job;
        this.gender = gender;
    }

    public enum Lingua { Nessuno, Italiano, Inglese, Francese, Tedesco, Spagnolo, Portoghese }

    public enum Patente { Nessuna, A, A1, A2, B, C, D, E }

    public enum Genere { M, F, Altro }

    public enum Occupazione { Sviluppatore, Medico };
}

