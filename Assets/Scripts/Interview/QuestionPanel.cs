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

    float points = 0;

    private string currentJob;

    private readonly List<int> prevID = new();

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
            ansButtons[i].name = question.answers[i].points.ToString();
            TextMeshProUGUI buttonText = ansButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = q.answers[i].text;
        }
    }


    void Start()
    {
        CountFolder();
        CountQuestions(true);

        if (CVManager.currentCV != null)
        {
            currentJob = CVManager.currentCV.job.ToString();
            LoadNewQuestion();
        }
            
        else Debug.Log("Nessun CV attualmente caricato. Caricare domanda tramite pulsanti.");
    }

    public void  LoadNewQuestion()
    {
        Setup(GetRandomQuestion(currentJob));
    }

    public void LoadNewQuestion(string job)
    {
        Setup(GetRandomQuestion(job));
    }
    
    

    public Question GetRandomQuestion(string job)
    {
        int i = 0;

       // Debug.Log($"Domande in {job}: " + GetJobFolderSize(job));

        while (i <= GetJobFolderSize(job))
        {
            Question rand = GetRandomQuestionInFolder(job);
            if (!prevID.Contains(rand.id))
            {
                prevID.Add(rand.id);
                return rand;
            }
            else
            {
                i++;
            }
        }
        return null;
    }

    //public void NextQuestion()
    //{
    //    // Messo medico per test: inserire in seguito variabile a questionPanel forse per memorizzare il lavoro
    //    // OPPURE (MEGLIO) leggere da CurrentCV

    //    Setup(GetRandomQuestion("Medico"));
    //}

    public void OnButtonClick(Button button)
    {
       // Debug.Log("Click di " + button.name);
        if (question != null)
        {
           // Debug.Log("Punteggio pre-risposta: " + points);
            points += float.Parse(button.name);
         //   Debug.Log("Aggiungo punti " + float.Parse(button.name));
         //   Debug.Log("Punteggio post-risposta: " + points);
        }
        Invoke(nameof(LoadNewQuestion), 1f);
    }
}