using System.Collections.Generic;
using TMPro;
using Unity.Jobs;
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


        // LoadNewQuestion("start");

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

            //LoadNewQuestion();
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
        SetupInterviewInfo(currentJob);
    }

    public void NextQuestion()
    {
        Debug.Log("caricamento nuova domanda");
       

        if (index <= questionNumber - 1)
        {            
            Setup(questions[++index]);
            Debug.Log("Index = " + index);
            SetupInterviewInfo(currentJob);
        }

        else Debug.Log("FINITE?");
    }

    public void LoadNewQuestion(string job)
    {
        Setup(GetRandomQuestion(job));
        SetupInterviewInfo(job);

    }

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
            // LoadNewQuestion();
            Debug.Log("Il lavoro scelto è " + currentJob);
            SetupInterview();
        }

        InputPanel.ClearAll();
    }


    public void SetupInterviewInfo(string job)
    {
        // questionNumber = GetJobFolderSize(job);

        if (interviewInfo)
        {
            interviewInfo.gameObject.SetActive(true);
            interviewInfo.ReloadInfo();
        }
    }

    public Question GetRandomQuestion(string job)
    {
        Debug.Log("Cerco per " + job);

        int i = 0;
        Debug.Log("Dimensione jobFolder = " + GetJobFolderSize(job));

        while (i <= GetJobFolderSize(job))
        {
            Question rand = GetRandomQuestionInFolder(job);

            // DebugQuestion(rand);

            if (!prevID.Contains(rand.id))
            {
                prevID.Add(rand.id);

                Debug.Log("CARICATA!");
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

    public void SetupInterview()
    {
        questionNumber = startQuestions + jobQuestions + softSkillQuestions;

        // Serve un controllo che, se le domande scelte per il lavoro / cartella sono MAGGIORI di quante ce ne sono attualmente in cartella, carica IL MASSIMO
        // oppure ferma il setup

        Debug.Log("Caricando domande Start");

        if (questions == null)
        {
            Debug.Log("Lista nulla");
            return;
        }


        for (int i = 0; i < startQuestions; i++)
        {
            //questions[i] = GetRandomQuestion("Start");
            Debug.Log("i = " + i);
            Question q = GetRandomQuestion("Start");

            //if (q == null)
            //{
            //    Debug.Log("Errore?");
            //}
            //else
            //{
            //    Debug.Log("Tutto ok in setup interview. La domanda è:");
            //    DebugQuestion(q);
            //}

            questions.Add(q);

            //    Debug.Log(questions[i].question);

            //   DebugQuestion(questions[i]);
        }

        for (int i = startQuestions; i < startQuestions + jobQuestions; i++)
        {
            Debug.Log("i = " + i);
            questions.Add(GetRandomQuestion(currentJob));
        }

        for (int i = startQuestions + jobQuestions; i < questionNumber; i++)
        {
            Debug.Log("i = " + i);
            questions.Add(GetRandomQuestion("SoftSkill"));
        }

        // Aggiunge una domanda nulla per segnalare a setup la fine del colloquio
        questions.Add(null);


        SetupInterviewInfo(currentJob);
        Setup(questions[index]);
    }

    public void ResetInterview()
    {
        CVManager.UnloadCurrentCV();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}