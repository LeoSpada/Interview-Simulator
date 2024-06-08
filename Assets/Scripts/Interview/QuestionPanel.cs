using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static InterviewManager;

public class QuestionPanel : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button[] ansButtons;
    private Question question;

    public InterviewInfo interviewInfo;
    public CVInfo cvInfo;

    public TMP_InputField folderInputField;

    public float points = 0;

    public int answered = 0;

    public float average = 0f;

    public int questionNumber = 0;

    public string currentJob;

    private bool cvLoaded = false;

    private readonly List<int> prevID = new();

    void Start()
    {
        if (interviewInfo)
            interviewInfo.gameObject.SetActive(false);

        if (cvInfo)
            cvInfo.gameObject.SetActive(false);

        // Se è stato caricato correttamente un CV
        if (CVManager.currentCV != null)
        {
            cvLoaded = true;
            currentJob = CVManager.currentCV.job.ToString();

            if (cvInfo)
            {
                cvInfo.gameObject.SetActive(true);
                cvInfo.reloadInfo();
            }
            LoadNewQuestion();
        }

        else Debug.Log("Nessun CV attualmente caricato. Caricare domanda tramite pulsanti.");
    }

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
                buttonText.text = "-Reset-";
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

    public void LoadNewQuestion()
    {
        Setup(GetRandomQuestion(currentJob));
        SetupInterviewInfo(currentJob);
    }

    public void LoadNewQuestion(string job)
    {
        Setup(GetRandomQuestion(job));
        SetupInterviewInfo(job);

    }

    public void SubmitFolderInput()
    {
        if (folderInputField != null)
            InputPanel.AcceptInputField(folderInputField);
        else
        {
            Debug.Log("Nessun input field caricato.");
            return;
        }

        if (InputPanel.fieldsClear)
        {
            currentJob = folderInputField.text;
            LoadNewQuestion();
        }

        InputPanel.ClearAll();
    }


    public void SetupInterviewInfo(string job)
    {
        questionNumber = GetJobFolderSize(job);
        if (interviewInfo)
        {
            interviewInfo.gameObject.SetActive(true);
            interviewInfo.ReloadInfo();
        }
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

    public void OnButtonClick(Button button)
    {
        if (question != null)
        {
            points += float.Parse(button.name);
            answered++;

            average = points / answered;

            Invoke(nameof(LoadNewQuestion), 1f);
        }
        else
        {
            Invoke(nameof(ResetInterview), 1f);
        }
    }

    public void ResetInterview()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}