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

        // Guid guid = Guid.NewGuid();



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
        //if (CheckEntry(cvEntry))
        //{
        //   // Debug.Log("Sovrascrittura di");
        //   // DebugCV(cvEntry);

        //    // AGGIUNGERE MESSAGGIO / CONFERMA DI SOVRASCRITTURA
        //}

        string json = JsonConvert.SerializeObject(question);
        Debug.Log(json);
        File.WriteAllText(GetQuestionFilePath(job, question.id), json);


    }

    [System.Serializable]
    public class Question
    {
        public string question;
        public string[] answers;
        public int correctIndex;

        public int id;

        // Viene resettato ad ogni esecuzione del codice
        // Trovare soluzione alternativa: data / ora?
        public static int q_id = 0;

        public Question(string question, string[] answers, int correctIndex)
        {
            this.question = question;
            this.answers = answers;
            this.correctIndex = correctIndex;
            id = q_id++;
        }

        public bool CheckAnswer(int index)
        {
            if (index == correctIndex)
            {
                return true;
            }
            else return false;
        }

        public int GetLastID()
        {
            return id;
        }
    }
}
