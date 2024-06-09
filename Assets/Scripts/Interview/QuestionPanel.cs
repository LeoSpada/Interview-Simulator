using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static InterviewManager;

public class QuestionPanel : MonoBehaviour
{
    [Header("Domanda")]
    public TextMeshProUGUI questionText;
    public Button[] ansButtons;
    private Question question;
    private List<Question> questions = new();
    private readonly List<int> prevID = new();

    [Header("Colloquio")]
    public int startQuestions = 2;
    public int jobQuestions = 2;
    public int softSkillQuestions = 2;
    public int index = 0;
    public string startFolder = "Start";
    public string softSkillFolder = "SoftSkill";

    [Header("CV")]
    public string currentJob;
    private bool cvLoaded = false;

    [Header("Punteggi")]
    public float points = 0;
    public int answered = 0;
    public float average = 0f;
    public int questionNumber = 0;

    // Sezione usata in Quiz Test
    // CVInfo e InterviewInfo potrebbero essere usati anche in fase finale
    // folderInput e metodi associati dovrebbero essere usati SOLO nella fase di testing

    [Header("Debug e altro")]
    public InterviewInfo interviewInfo;
    public CVInfo cvInfo;
    public TMP_InputField folderInputField;
    public GameObject folderInputGroup;

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
            folderInputGroup.SetActive(false);
            currentJob = CVManager.currentCV.job.ToString();

            if (cvInfo)
            {
                cvInfo.gameObject.SetActive(true);
                cvInfo.reloadInfo();
            }

            SetupInterview();
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
        SetupInterviewInfo();
    }

    public void NextQuestion()
    {
        if (index <= questionNumber - 1)
        {
            Setup(questions[++index]);
            // Debug.Log("Index = " + index);
            SetupInterviewInfo();
        }

        else Debug.Log("FINITE");
    }

    //public void LoadNewQuestion(string job)
    //{
    //    Setup(GetRandomQuestion(job));
    //    SetupInterviewInfo();

    //}

    public void SubmitFolderInput()
    {
        if (cvLoaded) return;

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
            folderInputGroup.SetActive(false);
            SetupInterview();
        }

        InputPanel.ClearAll();
    }


    public void SetupInterviewInfo()
    {

        if (interviewInfo)
        {
            interviewInfo.gameObject.SetActive(true);
            interviewInfo.ReloadInfo();
        }
    }

    public Question GetRandomQuestion(string job)
    {

        int i = 0;

        while (i <= GetJobFolderSize(job))
        {
            Question rand = GetRandomQuestionInFolder(job);

            // DebugQuestion(rand);

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

            Invoke(nameof(NextQuestion), 1f);
        }
        else
        {
            Invoke(nameof(ResetInterview), 1f);
        }
    }

    // Imposta il colloquio precaricando le domande da varie cartelle.
    // ATTENZIONE: attualmente carica da 3 cartelle, senza possibilità di cambiare. Si può cambiare però le 3 cartelle da cui caricare (VEDI VARIABILI SOPRA)
    public void SetupInterview()
    {
        startQuestions = QuestionLimit(startQuestions, startFolder);
        jobQuestions = QuestionLimit(startQuestions, currentJob);
        softSkillQuestions = QuestionLimit(startQuestions, softSkillFolder);

        questionNumber = startQuestions + jobQuestions + softSkillQuestions;

        if (questions == null)
        {
            Debug.Log("Lista nulla");
            return;
        }

        int i, j, k;

        for (i = 0; i < startQuestions; i++) questions.Add(GetRandomQuestion("Start"));

        for (j = i; j < i + jobQuestions; j++) questions.Add(GetRandomQuestion(currentJob));

        for (k = j; k < questionNumber; k++) questions.Add(GetRandomQuestion("SoftSkill"));

        // Aggiunge una domanda nulla per segnalare a setup la fine del colloquio
        questions.Add(null);

        SetupInterviewInfo();
        Setup(questions[index]);
    }

    public int QuestionLimit(int counter, string folder, bool log = false)
    {
        {
            int folderSize = GetJobFolderSize(folder);
            if (counter > folderSize)
            {
                if (log) Debug.Log($"Domande richieste ({counter}) > Domande trovate in cartella '{folder}' ({folderSize}). Caricamento di tutte le domande in '{folder}'.");
                return GetJobFolderSize(folder);
            }
            else return counter;
        }
    }

    public void ResetInterview()
    {
        CVManager.UnloadCurrentCV();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}