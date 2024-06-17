using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static InterviewManager.Question;

// Gestisce le domande e le loro cartelle con salvataggi, caricamenti, rimozioni e altre operazioni varie.
public static class InterviewManager
{
    // Cartella in cui vengono salvate le domande
    private const string saveFolder = "Questions";

    // Restituisce il percorso della cartella principale
    public static string GetQuestionsFolder()
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

        return folder;
    }

    // Restituisce il percorso della cartella corrispondente all'occupazione / sezione scelta
    public static string GetJobFolder(string job)
    {
        return Path.Combine(GetQuestionsFolder(), job);
    }

    // Restituisce la dimensione della cartella corrispondente all'occupazione / sezione scelta
    public static int GetJobFolderSize(string job)
    {
        return GetAllQuestionsInFolder(job).Count;
    }

    // Restituisce le informazioni delle sottocartelle della principale 
    public static DirectoryInfo[] GetFoldersInfo()
    {
        DirectoryInfo mainDir = new(Path.Combine(Application.persistentDataPath, saveFolder));
        if (mainDir == null) return null;

        DirectoryInfo[] dirs;
        try
        {
            dirs = mainDir.GetDirectories();
        }
        catch (DirectoryNotFoundException)
        {
            Debug.Log("Nessuna directory trovata in " + saveFolder);
            return null;
        }

        return dirs;
    }
    
    // Conta quante sottocartelle ci sono
    public static int CountFolders(bool log = false)
    {
        var dirs = GetFoldersInfo();
        if (dirs == null) return 0;

        if (log) Debug.Log("Numero di cartelle: " + dirs.Length);

        return dirs.Length;
    }

    // Conta le domande presenti su file
    public static int CountQuestions(bool log = false)
    {
        var dirs = GetFoldersInfo();
        if (dirs == null) return 0;

        int sum = 0;

        foreach (DirectoryInfo dir in dirs)
        {
            sum += GetAllQuestionsInFolder(dir.Name).Count;
            if (log) Debug.Log(dir.Name + " ha " + GetAllQuestionsInFolder(dir.Name).Count + " domande.");
        }

        if (log) Debug.Log("Somma: " + sum);
        return sum;
    }

    // Ottiene il percorso della domanda scelta
    public static string GetQuestionFilePath(string job, int id)
    {
        string folder = GetJobFolder(job);
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

        string path = Path.Combine(folder, $"question_{id}.json");
        return path;
    }

    // Ottiene tutte le domande contenute nella sottocartella scelta
    public static List<Question> GetAllQuestionsInFolder(string job)
    {
        List<Question> list = new();

        DirectoryInfo dir = new(GetJobFolder(job));
        if (!dir.Exists)
        {
            Debug.Log("La cartella " + dir + " non esiste");
            return list;
        }

        FileInfo[] info = dir.GetFiles("*.json");

        foreach (FileInfo f in info)
        {
            string json = File.ReadAllText(f.ToString());
            var question = JsonConvert.DeserializeObject<Question>(json);
            list.Add(question);
        }

        return list;
    }

    // Ottiene una domanda a caso della sottocartella scelta
    public static Question GetRandomQuestionInFolder(string job)
    {
        List<Question> list = GetAllQuestionsInFolder(job);

        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    // Restituisce la domanda scelta
    public static Question GetQuestion(string job, int id)
    {
        if (!File.Exists(GetQuestionFilePath(job, id)))
        {
            return null;
        }

        string json = File.ReadAllText(GetQuestionFilePath(job, id));

        var question = JsonConvert.DeserializeObject<Question>(json);

        return question;
    }

    // Salva su file (nella sottocartella indicata) la domanda scelta
    public static void AddQuestion(Question question, string job)
    {
        string json = JsonConvert.SerializeObject(question);        
        File.WriteAllText(GetQuestionFilePath(job, question.id), json);
    }

    // Conta quante risposte ha la domanda scelta
    public static int CountAnswers(Question question, bool log = false)
    {
        int counter = 0;
        foreach (Answer ans in question.answers)
        {
            if (!ans.text.Equals("000")) counter++;
        }

        if (log) Debug.Log($"La domanda {question} ha {counter} risposte");
        return counter;
    }

    // Usata per debug. Stampa a schermo la domanda formattata.
    public static void DebugQuestion(Question question)
    {
        Debug.Log($"Domanda: {question.question}\nRisposte: 1) {question.answers[0].text} [{question.answers[0].points} pt.]\t 2) {question.answers[1].text} [{question.answers[1].points} pt.]\t 3) {question.answers[2].text} [{question.answers[2].points} pt.]\t 3) {question.answers[3].text} [{question.answers[3].points} pt.]");
    }

    // Classe della domanda, con risposte e id.
    [System.Serializable]
    public class Question
    {
        public string question;
        public Answer[] answers;

        public int id;

        public struct Answer
        {
            public string text;
            public float points;
        }

        // Serve per rendere univoco l'id della domanda
        public static int q_id;

        public Question()
        {

        }

        public Question(string question, Answer[] answers)
        {
            this.question = question;
            this.answers = answers;           

            // q_id prende il valore del conteggio più recente per evitare di sovrascrivere una domanda già esistente
            q_id = CountQuestions();
            id = ++q_id;           
        }

        // Costruttore alternativo con id scelto da utente
        public Question(string question, Answer[] answers, int id)
        {
            this.question = question;
            this.answers = answers;
            this.id = id;          
        }
    }
}
