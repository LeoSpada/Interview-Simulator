using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InterviewManager;

public class QuestionPanel : MonoBehaviour
{

    public TextMeshProUGUI questionText;
    public Button[] ansButtons;
    private Question question;

    // public Canvas entryPanel;

    // Rimettere question come argomento funzione
    public void Setup(Question q)
    {
        // Domanda di prova
        // Question q = GetQuestion("Sviluppatore", 0);

        question = q;
        questionText.text = q.question;

        for (int i = 0; i < ansButtons.Length; i++)
        {
            ansButtons[i].name = $"Ans{i}";
            // Test indice corretto: Trovare come rendere la corretta quella che d� punti
            if (i == q.correctIndex) ansButtons[i].name += " [CORRETTA]";


            TextMeshProUGUI buttonText = ansButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = q.answers[i];
        }
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    Setup(RandomQuestion("Sviluppatore"));
    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

    public void RandomMedicoQuestion()
    {
        Setup(RandomQuestion("Medico"));
    }

    public void RandomSviluppatoreQuestion()
    {
        Setup(RandomQuestion("Sviluppatore"));
    }

    public Question RandomQuestion(string job)
    {
        return GetRandomQuestionInFolder(job);
    }
    // Fare LoadRandom qui o in InterviewManager per restituire una domanda a caso del lavoro scelto
    // Parti dalla lista ottenuta da GetAll
}
