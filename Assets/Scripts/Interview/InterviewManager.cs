using Newtonsoft.Json;
using System.IO;
using TMPro;
using UnityEngine;
using static CVEntry;

public static class InterviewManager
{
    public static TextMeshProUGUI text;

    public static int score = 0;

    private const string saveFolder = "Questions";

    static string GetJobQuestionFilePath()
    {
        var cv = CVManager.currentCV;
        string path = Path.Combine(Application.persistentDataPath, saveFolder, $"job_{cv.job}_questions.json");
        return path;
    }

    public static void LoadQuestion()
    {

    }

    public static void AddQuestion(Question question)
    {
        //if (CheckEntry(cvEntry))
        //{
        //   // Debug.Log("Sovrascrittura di");
        //   // DebugCV(cvEntry);

        //    // AGGIUNGERE MESSAGGIO / CONFERMA DI SOVRASCRITTURA
        //}

        string json = JsonConvert.SerializeObject(question);
        Debug.Log(json);
        File.WriteAllText(GetJobQuestionFilePath(), json);
    }

    [System.Serializable]
    public class Question
    {
        public string question;
        public string[] answers;
        public int correctIndex;

        public Question(string question, string[] answers, int correctIndex)
        {
            this.question = question;
            this.answers = answers;
            this.correctIndex = correctIndex;
        }

        public bool CheckAnswer(int index)
        {
            if (index == correctIndex)
            {
                return true;
            }
            else return false;
        }
    }
}
