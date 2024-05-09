using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class CVManager
{
    static string GetCVFilePath(string name, string surname)
    {
        string path = Path.Combine(Application.persistentDataPath, $"{name}_{surname}_CV.json");
        Debug.Log($"FilePath: {path}");
        return path;
    }

    public static List<CVEntry> GetCV(string name, string surname)
    {
        if (!File.Exists(GetCVFilePath(name, surname)))
        {
            return new List<CVEntry>();
        }

        string json = File.ReadAllText(GetCVFilePath(name, surname));

        var loadedCV = JsonConvert.DeserializeObject<CVList>(json);
        return loadedCV.cv;
    }

    public static void AddCVEntry(string name, string surname)
    {
        var currentCV = GetCV(name, surname);

        var newEntry = new CVEntry
        {
            name = name,
            surname = surname
        };

        currentCV.Add(newEntry);

        //Salvataggio
        var toSave = new CVList
        {
            cv = currentCV
        };

        string json = JsonConvert.SerializeObject(toSave);


        File.WriteAllText(
            (GetCVFilePath(name, surname)),
            json
            );

        Debug.Log(json);
    }
}

[System.Serializable]
public class CVList
{
    public List<CVEntry> cv;
}

[System.Serializable]
public class CVEntry
{
    public string name;
    public string surname;
    // AGGIUNGERE QUI TUTTI I CAMPI
    // VEDERE DATI_CURRICULUM e altri


    public CVEntry()
    {

    }

    // CANCELLARE E USARE
    // click destro su CVEntry / Azioni rapide e refactoring / Genera costruttore
    // CON CAMPI AGGIORNATI
    public CVEntry(string name, string surname)
    {
        this.name = name;
        this.surname = surname;
    }
}

