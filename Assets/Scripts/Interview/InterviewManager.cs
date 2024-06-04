using Newtonsoft.Json;
using System.IO;
using TMPro;
using UnityEngine;
using static CVEntry;

public static class InterviewManager
{
   // private static CVEntry loadedCV;

    public static TextMeshProUGUI text;

    public static int score = 0;

    static string GetJobQuestionFilePath()
    {
        var cv = CVManager.currentCV;
               

        Debug.Log(cv);

        Debug.Log("Cerco file per " + cv.job);
        string path = Path.Combine(Application.persistentDataPath, $"job_{cv.job}_questions.json");
        Debug.Log("Path: " + path);
        return path;
    }
    // Start is called before the first frame update
    //void Start()
    //{

    //    loadedCV = CVManager.currentCV;

    //    if (loadedCV == null) Debug.Log("Nessun CV caricato. ERRORE");

    //   else if (loadedCV.job == 0) Debug.Log("Sviluppatore");
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    text.text = loadedCV.name;
    //}

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



        Debug.Log("TEST AGGIUNTA " + question);

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
