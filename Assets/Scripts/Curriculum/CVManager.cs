using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class CVManager
{
    public static CVEntry currentCV = null;

    public static bool editCurrent = false;
    static string GetCVFilePath(string name, string surname)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{name}_{surname}_CV.json");
        return path;
    }

    // Restituisce la lista di tutti i CV nella cartella di salvataggio
    public static List<CVEntry> GetAllCV()
    {
        List<CVEntry> list = new();

        DirectoryInfo dir = new(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles("*.json");

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

    public static void AddCVEntry(CVEntry cvEntry)
    {
        //if (CheckEntry(cvEntry))
        //{
        //   // Debug.Log("Sovrascrittura di");
        //   // DebugCV(cvEntry);

        //    // AGGIUNGERE MESSAGGIO / CONFERMA DI SOVRASCRITTURA
        //}

        string json = JsonConvert.SerializeObject(cvEntry);
        File.WriteAllText((GetCVFilePath(cvEntry.name, cvEntry.surname)), json);
    }

    public static void RemoveCVEntry(CVEntry cvEntry)
    {
        if (CheckEntry(cvEntry))
            File.Delete(GetCVFilePath(cvEntry.name, cvEntry.surname));
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
    public string job;
    public Genere gender;
    // AGGIUNGERE QUI TUTTI I CAMPI e rigenerare costruttore
    // VEDERE DATI_CURRICULUM e altri


    public CVEntry()
    {

    }

    public CVEntry(string name, string surname, string job, Genere gender)
    {
        this.name = name;
        this.surname = surname;
        this.job = job;
        this.gender = gender;
    }

    public enum Lingua { Nessuno, Italiano, Inglese, Francese, Tedesco, Spagnolo, Portoghese }

    public enum Patente { A, A1, A2, B, C, D, E }

    public enum Genere { M, F, Altro }
}

