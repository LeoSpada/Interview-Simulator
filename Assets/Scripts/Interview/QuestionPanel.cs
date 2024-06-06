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

    int points = 0;

    private List<int> prevID = new();

    // Potrebbe essere necessario un metodo (da collegare ai pulsanti) che carica la prossima domanda

    public void Setup(Question q)
    {
        // Se q è null, le domande sono finite.
        // Rimpiazzare questa parte con caricamento scena di calcolo punteggio??

        if (q == null)
        {
            question = null;
            questionText.text = "Domande finite.\nPunteggio: " + points;
            foreach (Button button in ansButtons)
            {
                button.name = "EmptyAns";
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


    void Start()
    {
        CountFolder();
        CountQuestions(true);
    }

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

    public void NextQuestion()
    {
        // Messo medico per test: inserire in seguito variabile a questionPanel forse per memorizzare il lavoro
        // OPPURE (MEGLIO) leggere da CurrentCV
        foreach (Button button in ansButtons)
        {
            button.image.color = Color.white;
        }

        Setup(RandomQuestion("Medico"));
    }

    public void OnButtonClick(Button button)
    {
        Debug.Log("Clicc di " + button.name);
        if (question != null)
            if (button.name.Contains(question.correctIndex.ToString()))
            {
                Debug.Log("Risposta corretta");
                points++;
                button.image.color = Color.green;
            }
            else
            {
                Debug.Log("Risposta errata");
                button.image.color = Color.red;

                ansButtons[question.correctIndex].image.color = Color.green;
            }
        Invoke(nameof(NextQuestion), 2.5f);
    }
}