using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static InterviewManager.Question;

public static class InterviewManager
{
    public static TextMeshProUGUI text;

    public static int score = 0;

    private const string saveFolder = "Questions";

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

    public static string GetJobFolder(string job)
    {
        return Path.Combine(GetQuestionsFolder(), job);
    }

    public static int GetJobFolderSize(string job)
    {
        return GetAllQuestionsInFolder(job).Count;
    }

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

    public static int CountFolders(bool log = false)
    {
        var dirs = GetFoldersInfo();
        if (dirs == null) return 0;

        if (log) Debug.Log("Numero di cartelle: " + dirs.Length);

        return dirs.Length;
    }

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

    public static Question GetRandomQuestionInFolder(string job)
    {
        List<Question> list = GetAllQuestionsInFolder(job);

        return list[UnityEngine.Random.Range(0, list.Count)];
    }

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

    public static void AddQuestion(Question question, string job)
    {
        string json = JsonConvert.SerializeObject(question);
        // Debug.Log(json);
        File.WriteAllText(GetQuestionFilePath(job, question.id), json);

        //  BackupManager.BackUpFolder(GetQuestionsFolder(), saveFolder);

        // BackupManager.BackUpAll();
    }

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

    // Usata per debug. Stampa ans schermo la domanda formattata.
    public static void DebugQuestion(Question question)
    {
        Debug.Log($"Domanda: {question.question}\nRisposte: 1) {question.answers[0].text} [{question.answers[0].points} pt.]\t 2) {question.answers[1].text} [{question.answers[1].points} pt.]\t 3) {question.answers[2].text} [{question.answers[2].points} pt.]\t 3) {question.answers[3].text} [{question.answers[3].points} pt.]");
    }

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

        public static int q_id;

        public Question()
        {

        }

        public Question(string question, Answer[] answers)
        {
            this.question = question;
            this.answers = answers;

            // q_id = GetLastID();

            q_id = CountQuestions();
            id = ++q_id;
            //  Debug.Log("domanda " + id + " creata");
        }

        public Question(string question, Answer[] answers, int id)
        {
            this.question = question;
            this.answers = answers;
            this.id = id;
            // Debug.Log("Creata domanda con custom id " + id);
        }
    }
}
