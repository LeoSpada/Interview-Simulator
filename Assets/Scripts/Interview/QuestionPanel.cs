using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;
using static InterviewManager;
using static InterviewManager.Question;

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
    public int endQuestions = 2;
    public int index = 0;
    public string startFolder = "Start";
    // CAMBIARE NOME CARTELLA / CREARE CARTELLA END e END nel dropdown di question creator
    // Rimuovere soft skill da dropdown question Creator
    public string endFolder = "End";

    [Header("CV")]
    public string currentJob;
    private bool cvLoaded = false;

    [Header("Punteggi")]
    public float points = 0;
    public int answered = 0;
    public int noPointAnswers = 0;
    public float average = 0f;
    public int questionNumber = 0;

    // Sezione usata in IntroQuestion

    private readonly float softSkillID = 11.01f;
    private readonly float pregiID = 11.02f;
    private readonly float difettiID = 11.03f;
    private readonly float continueID = 11.04f;
    // private bool continueIntro = true;
    private readonly string introFolder = "intro";

    private Question introQuestion;
    private Question softSkillQuestion;
    private Question strengthQuestion;
    private Question weaknessQuestion;

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
        introQuestion = GetIntroQuestion();
        softSkillQuestion = GetSoftSkillQuestion();
        strengthQuestion = GetStrengthQuestion();
        weaknessQuestion = GetWeaknessQuestion();


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

        // INSERIRE INVOCAZIONE SCENA PUNTEGGIO

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

        foreach (Button button in ansButtons)
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText.text.Equals(InputPanel.disabledText))
            {
                // Debug.Log($"Risposta {button.name} rimossa");
                button.gameObject.SetActive(false);
            }
            else
            {
                button.gameObject.SetActive(true);
            }
        }
    }

    //public void LoadNewQuestion()
    //{
    //    Setup(GetRandomQuestion(currentJob));
    //    SetupInterviewInfo();
    //}

    public void NextQuestion()
    {
        // Rimosso -1 per test
        if (index <= questionNumber)
        {
            Setup(questions[++index]);
            // Debug.Log("Index = " + index);
            SetupInterviewInfo();
        }

        else Debug.Log("FINITE");
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
            SetupInterview();
        }

        InputPanel.ClearAll();
    }


    public void SetupInterviewInfo()
    {

        if (interviewInfo)
        {
            questionNumber = questions.Count - 1;
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
        DebugQuestion(question);

        if (question != null)
        {

            Debug.Log("Question: " + question.question);
            Debug.Log("SSQ: " + softSkillQuestion.question);

            if (question.question.Equals(softSkillQuestion.question))
            {
                Debug.Log("TROVATA");
                softSkillQuestion.answers[0].text = InputPanel.disabledText;
                DebugQuestion(softSkillQuestion);
            }

            float answerPoints = float.Parse(button.name);

            if (answerPoints != 0)
            {
                if (answerPoints == softSkillID)
                {
                    Debug.Log("SOFT");
                    noPointAnswers++;

                    questions.Insert(index + 1, softSkillQuestion);
                    questions.Insert(index + 2, introQuestion);
                    questionNumber++;
                }
                else if (answerPoints == pregiID)
                {
                    Debug.Log("PREGI");
                    noPointAnswers++;

                    questions.Insert(index + 1, strengthQuestion);
                    questions.Insert(index + 2, introQuestion);
                    questionNumber++;

                }
                else if (answerPoints == difettiID)
                {
                    Debug.Log("DIFETTI");
                    noPointAnswers++;

                    questions.Insert(index + 1, weaknessQuestion);
                    questions.Insert(index + 2, introQuestion);
                    questionNumber++;

                }
                else if (answerPoints == continueID)
                {
                    Debug.Log("CONTINUE");
                    noPointAnswers++;
                    questionNumber++;
                }
                else points += answerPoints;


            }
            else noPointAnswers++;

            answered++;

            if (answered - noPointAnswers == 0) average = 0;
            else average = points / (answered - noPointAnswers);

            Invoke(nameof(NextQuestion), 1f);
        }

        // Parte non funzionante di test per rimuovere risposte già date
        // TESTATO SOLO CON SOFTSKILLQUESTION

        else Invoke(nameof(ResetInterview), 1f);
    }

    // Imposta il colloquio precaricando le domande da varie cartelle.
    // ATTENZIONE: attualmente carica da 3 cartelle, senza possibilità di cambiare. Si può cambiare però le 3 cartelle da cui caricare (VEDI VARIABILI SOPRA)


    // ALCUNE DOMANDE ANDREBBERO FATTE IN UN ORDINE PREFISSATO
    // Funzione in InterviewManager che restituisce la cartella intera, in ordine? (Forse già c'è - Vedi GetAllQuestionsInFolder()

    public void SetupInterview()
    {


        questions.Add(introQuestion);
        questionNumber++;

        startQuestions = QuestionLimit(startQuestions, startFolder);
        jobQuestions = QuestionLimit(startQuestions, currentJob);
        endQuestions = QuestionLimit(startQuestions, endFolder);

        //questionNumber = startQuestions + jobQuestions + endQuestions;

        if (questions == null)
        {
            Debug.Log("Lista nulla");
            return;
        }

        int i, j, k;

        for (i = 0; i < startQuestions; i++) questions.Add(GetRandomQuestion(startFolder));

        for (j = i; j < i + jobQuestions; j++) questions.Add(GetRandomQuestion(currentJob));

        for (k = j; k < j + endQuestions; k++) questions.Add(GetRandomQuestion(endFolder));

        // Aggiunge una domanda nulla per segnalare a setup la fine del colloquio
        questions.Add(null);

        SetupInterviewInfo();
        DebugQuestion(questions[index]);

        Debug.Log("Ci sono domande n = " + questions.Count);

        questionNumber = questions.Count - 1;
        Setup(questions[index]);
    }

    // SNELLIRE CODICE??


    public Question GetIntroQuestion(bool save = true)
    {
        string questionText = "Mi parli un po' di lei...";

        Answer[] answers = new Answer[4];
        answers[0].text = "Elenca Soft Skill";
        answers[0].points = softSkillID;

        answers[1].text = "Elenca Pregi";
        answers[1].points = pregiID;

        answers[2].text = "Elenca Difetti";
        answers[2].points = difettiID;

        answers[3].text = "Vai avanti";
        answers[3].points = continueID;

        Question question = new(questionText, answers);

        if (save) InterviewManager.AddQuestion(question, introFolder);

        return question;
    }

    public Question GetSoftSkillQuestion(bool save = false)
    {
        string questionText = "Quali Soft Skill possiede?";

        Answer[] answers = new Answer[4];
        answers[0].text = "Team Working";
        answers[0].points = 1f;

        answers[1].text = "Leadership";
        answers[1].points = 1f;

        answers[2].text = "Problem Solving";
        answers[2].points = 1f;

        answers[3].text = "Time Management";
        answers[3].points = 1f;

        Question question = new(questionText, answers);

        if (save) InterviewManager.AddQuestion(question, introFolder);

        return question;
    }

    // Domande sui pregi
    public Question GetStrengthQuestion(bool save = false)
    {
        string questionText = "Quali pregi possiede?";

        Answer[] answers = new Answer[4];
        answers[0].text = "Onestà";
        answers[0].points = 1f;

        answers[1].text = "Umiltà";
        answers[1].points = 1f;

        answers[2].text = "Affidabilità";
        answers[2].points = 1f;

        answers[3].text = "Determinazione";
        answers[3].points = 1f;

        Question question = new(questionText, answers);

        if (save) InterviewManager.AddQuestion(question, introFolder);

        return question;
    }

    // Domande sui difetti
    public Question GetWeaknessQuestion(bool save = false)
    {
        string questionText = "Quali difetti possiede?";

        Answer[] answers = new Answer[4];
        answers[0].text = "Rabbia";
        answers[0].points = -1f;

        answers[1].text = "Pigrizia";
        answers[1].points = -1f;

        answers[2].text = "Invidia";
        answers[2].points = -1f;

        answers[3].text = "Distrazione";
        answers[3].points = -1f;

        Question question = new(questionText, answers);

        if (save) InterviewManager.AddQuestion(question, introFolder);

        return question;
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

    // RIMUOVERE / COMMENTARE DA VERSIONE FINALE

    public void ResetInterview()
    {
        CVManager.UnloadCurrentCV();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}