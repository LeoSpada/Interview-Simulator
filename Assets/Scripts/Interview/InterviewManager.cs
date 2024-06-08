using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public static class InterviewManager
{
    public static TextMeshProUGUI text;

    public static int score = 0;

    private const string saveFolder = "Questions";

    public static string GetJobFolder(string job)
    {
        return Path.Combine(Application.persistentDataPath, saveFolder, job);
    }

    public static int GetJobFolderSize(string job)
    {
        return GetAllQuestionsInFolder(job).Count;
    }

    public static DirectoryInfo[] GetFoldersInfo()
    {
        DirectoryInfo mainDir = new(Path.Combine(Application.persistentDataPath, saveFolder));
        var dirs = mainDir.GetDirectories();
        return dirs;
    }

    public static int CountFolders()
    {
        var dirs = GetFoldersInfo();

        Debug.Log("Numero di cartelle: " + dirs.Length);

        return dirs.Length;
    }

    public static int CountQuestions(bool log)
    {
        var dirs = GetFoldersInfo();

        int sum = 0;

        foreach (DirectoryInfo dir in dirs)
        {
            sum += GetAllQuestionsInFolder(dir.Name).Count;
           if(log) Debug.Log(dir.Name + " ha " + GetAllQuestionsInFolder(dir.Name).Count + " domande.");
        }

        Debug.Log("Somma: " + sum);
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
        Debug.Log(json);
        File.WriteAllText(GetQuestionFilePath(job, question.id), json);
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

            q_id = GetLastID();

            id = ++q_id;

            // Debug.Log("Q_ID attuale:" + id);
            SaveLastID();
        }

        public static void SaveLastID()
        {
            PlayerPrefs.SetInt("q_id", q_id);
        }

        public static int GetLastID()
        {
            if (PlayerPrefs.HasKey("q_id"))
            {
                if (CountQuestions(false) == 0)
                {                    
                    Debug.Log("Nessuna domanda trovata. Azzero q_id in PlayerPrefs.");
                    return -1;
                }
                return PlayerPrefs.GetInt("q_id");
            }
            else return -1;
        }
    }
}
