using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InterviewManager;

public class QuestionPanel : MonoBehaviour
{

    public TextMeshProUGUI questionText;
    public Button[] ansButtons;
    private Question question;

    private List<int> prevID = new();

    // public Canvas entryPanel;

    // Rimettere question come argomento funzione
    public void Setup(Question q)
    {
        // Domanda di prova
        // Question q = GetQuestion("Sviluppatore", 0);

        if (q == null)
        {
            questionText.text = "Domande finite";
            foreach (Button button in ansButtons)
            {
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = "---";
            }
            return;
        }


        question = q;
        questionText.text = q.question;

        for (int i = 0; i < ansButtons.Length; i++)
        {
            ansButtons[i].name = $"Ans{i}";
            // Test indice corretto: Trovare come rendere la corretta quella che dà punti
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
        int i = 0;



        Debug.Log($"Domande in {job}: " + GetJobFolderSize(job));

        while (i <= GetJobFolderSize(job))
        {
            Question rand = GetRandomQuestionInFolder(job);
            if (!prevID.Contains(rand.id))
            {
                prevID.Add(rand.id);

                //Debug.Log("Elenco già uscite: ");
                //foreach (int id in prevID) Debug.Log(id + "\t");

                i++;
                // Debug.Log("I = " + i);
                return rand;

            }
            else
            {
                //  Debug.Log($"Domanda {rand.id} già uscita");
                i++;
            }
        }

        return null;
    }
}
