using System.IO;
using TMPro;
using UnityEngine;
using static CVEntry;

public static class InterviewManager
{
    private static CVEntry loadedCV;

    public static TextMeshProUGUI text;

    public static int score = 0;

    static string GetJobQuestionFilePath(CVEntry cv)
    {
        string path = Path.Combine(Application.persistentDataPath, $"job_{cv.job}_questions.json");
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
            if(index == correctIndex)
            {
                return true;
            }
            else return false;
        }


    }
}
