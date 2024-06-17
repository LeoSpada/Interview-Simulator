using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

// Gestisce i curriculum con salvataggi, caricamenti, rimozioni e altre operazioni varie.
public static class CVManager
{
    // L'attuale curriculum caricato
    public static CVEntry currentCV = null;

    // Indica se il curriculum attualmente caricato può essere completamente modificato e sovrascritto
    public static bool editCurrent = false;

    // Cartella in cui vengono salvati i curriculum
    private const string saveFolder = "Saves";

    // Restituisce la cartella contenente i curriculum
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

    // Restituisce il conteggio dei curriculum
    public static int GetCVFolderSize()
    {
        return GetAllCV().Count;
    }

    // Restituisce le informazioni dei file nella cartella
    public static FileInfo[] GetFilesInfo()
    {
        DirectoryInfo mainDir = new(GetCVFolder());
        var files = mainDir.GetFiles("*.json");
        return files;
    }

    // Restituisce il percorso di un determinato curriculum
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

        FileInfo[] info = GetFilesInfo();

        foreach (FileInfo f in info)
        {
            string json = File.ReadAllText(f.ToString());
            var loadedCV = JsonConvert.DeserializeObject<CVEntry>(json);
            list.Add(loadedCV);
        }

        return list;
    }

    // Restituisce un determinato curriculum
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

    // Restituisce un curriculum casuale
    public static CVEntry GetRandomCVEntry()
    {
        List<CVEntry> list = GetAllCV();
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    // Salva il curriculum su file
    public static void AddCVEntry(CVEntry cvEntry)
    {
        string json = JsonConvert.SerializeObject(cvEntry);
        File.WriteAllText((GetCVFilePath(cvEntry.name, cvEntry.surname)), json);
    }

    // Elimina il file corrisponde al curriculum scelto
    public static void RemoveCVEntry(CVEntry cvEntry)
    {
        if (CheckEntry(cvEntry))
            File.Delete(GetCVFilePath(cvEntry.name, cvEntry.surname));
    }

    // Controlla se esiste un file corrispondente al curriculum scelto
    public static bool CheckEntry(CVEntry cvEntry)
    {
        if (!File.Exists(GetCVFilePath(cvEntry.name, cvEntry.surname)))
            return false;
        return true;
    }

    // Controlla che il curriculum scelto sia quello attualmente caricato
    public static bool IsCurrentCV(string name, string surname)
    {
        if (currentCV.name != name || currentCV.surname != surname)
        {
            Debug.Log(name + "non corrisponde");
            return false;
        }

        else return true;
    }

    // Rimuove da currentCV il curriculum
    public static void UnloadCurrentCV()
    {
        currentCV = null;
    }


    // Usata per debug. Stampa a schermo il cv formattato.
    public static void DebugCV(CVEntry cv)
    {
        Debug.Log("Nome: " + cv.name + " " + cv.surname + " , " + cv.genere + "\nOccupazione desiderata: " + cv.occupazione);
    }
}


[System.Serializable]

// Gestisce l'istruzione del candidato
public class Istruzione
{
    public Qualifica qualifica;
    public Titolo titolo;

    public Istruzione()
    {

    }

    public Istruzione(Qualifica qualifica)
    {
        this.qualifica = qualifica;
        // FilterTitolo();
    }

    public Istruzione(Qualifica qualifica, Titolo titolo) : this(qualifica)
    {
        this.titolo = titolo;
    }

    public enum Qualifica { Medie, Superiori, Laurea };
    public enum Titolo { Nessuno, ITIS, IPSIA, Liceo, Scientifica, Umanistica, Economica, Sociale, Tecnologica };
}

[System.Serializable]

// Gestisce il curriculum
public class CVEntry
{
    public string name;
    public string surname;
    public Occupazione occupazione;
    public Genere genere;
    public Istruzione istruzione;
    public Esperienza esperienza;
    public Lingua secondaLingua;
    public Patente patente;

    public CVEntry()
    {

    }

    public CVEntry(string name, string surname, Occupazione occupazione, Genere genere, Istruzione istruzione, Esperienza esperienza, Lingua secondaLingua, Patente patente)
    {
        this.name = name;
        this.surname = surname;
        this.occupazione = occupazione;
        this.genere = genere;
        this.istruzione = istruzione;
        this.esperienza = esperienza;
        this.secondaLingua = secondaLingua;
        this.patente = patente;
    }

    // L'ordine in cui sono disposte le occupazioni possibili incide parzialmente sullo studio richiesto.

    // Prime due (0-1): Medie
    // Terza e quarta (2-3): Superiori
    // Quinta in poi (>4): Laurea

    public enum Occupazione { Sarto, Estetista, Meccanico, Segretario, Sviluppatore, Insegnante, Avvocato, Medico }

    public enum Genere { M, F, Altro }

    // Sezioni bonus: se presenti, danno punti bonus
    public enum Esperienza { Nessuna, Cameriere, Barista, Pulizie, Babysitter, Arbitro, Bagnino, Animatore }
    public enum Lingua { Nessuna, Italiano, Inglese, Francese, Tedesco, Spagnolo, Portoghese }
    public enum Patente { Nessuna, A, A1, A2, B, C, D, E }
}