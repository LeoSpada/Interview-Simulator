using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    private int index = 0;
    public string startFolder = "Start";
    public string endFolder = "End";

    [Header("CV")]
    private CVEntry cv;
    public string currentJob;
    private int currentJobID;
    public string currentEducation;
    private int currentEducationID;
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
    private readonly string introFolder = "Intro";

    private Question bonusPointsQuestion;
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

        // Se � stato caricato correttamente un CV
        if (CVManager.currentCV != null)
        {
            cvLoaded = true;
            if (folderInputGroup) folderInputGroup.SetActive(false);

            cv = CVManager.currentCV;

            currentJob = cv.occupazione.ToString();
            currentJobID = (int)cv.occupazione;
            // Debug.Log("occupazione di valore " + currentJobID);

            currentEducation = cv.istruzione.qualifica.ToString();
            currentEducationID = (int)cv.istruzione.qualifica;
            // Debug.Log("educazione di valore " + currentEducationID);

            bonusPointsQuestion = GetBonusPointsQuestion();

            // Debug.Log(currentEducation);

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

        // Debug.Log(currentJob);

        // Se q � null, le domande sono finite.
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

        // Se le voci di introQuestion non hanno pi� risposte disponibili al loro interno, non vengono visualizzate
        if (question.id == introQuestion.id)
        {
            if (CountAnswers(softSkillQuestion) == 0)
            {
                Debug.Log("Finite risposte per SoftSkillQ");
                question.answers[0].text = InputPanel.disabledText;
            }

            if (CountAnswers(strengthQuestion) == 0)
            {
                Debug.Log("Finite risposte per StrengthQ");
                question.answers[1].text = InputPanel.disabledText;
            }

            if (CountAnswers(weaknessQuestion) == 0)
            {
                Debug.Log("Finite risposte per WeaknessQ");
                question.answers[2].text = InputPanel.disabledText;
            }
        }

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

    public string Feedback()
    {
        string[] feedbacks =
        {
            "Risposta interessante.",
            "...",
            "Capisco...",
            "Bene. Andiamo avanti.",
            "Proseguiamo."
        };

        return feedbacks[Random.Range(0, feedbacks.Length)];
    }

    public void OnButtonClick(Button button)
    {

        questionText.text = Feedback();

        if (question != null)
        {

            // Se la domanda combacia con una delle tipologie di domande introduttive, disattiva la risposta scelta per quella domanda

            if (question.id == softSkillQuestion.id)
            {
                int i = int.Parse(button.tag);

                Debug.Log("rimossa 1 di soft (" + softSkillQuestion.id);

                softSkillQuestion.answers[i].text = InputPanel.disabledText;
                DebugQuestion(softSkillQuestion);
            }

            if (question.id == strengthQuestion.id)
            {
                int i = int.Parse(button.tag);

                Debug.Log("rimossa 1 di strength " + strengthQuestion.id);

                strengthQuestion.answers[i].text = InputPanel.disabledText;
                DebugQuestion(strengthQuestion);
            }

            if (question.id == weaknessQuestion.id)
            {
                int i = int.Parse(button.tag);

                Debug.Log("rimossa 1 di weakness " + weaknessQuestion.id);

                weaknessQuestion.answers[i].text = InputPanel.disabledText;
                DebugQuestion(weaknessQuestion);
            }

            float answerPoints = float.Parse(button.name);

            if (answerPoints != 0)
            {
                // Se la risposta combacia con l'id/punteggio associato alle sottocategorie, vengono aggiunte al colloquio le domande correlate 

                if (answerPoints == softSkillID)
                {
                    // Debug.Log("SOFT");
                    noPointAnswers++;

                    questions.Insert(index + 1, softSkillQuestion);
                    introQuestion.question = "C'� altro che vuole aggiungere?";
                    questions.Insert(index + 2, introQuestion);
                    questionNumber++;
                }
                else if (answerPoints == pregiID)
                {
                    // Debug.Log("PREGI");
                    noPointAnswers++;

                    questions.Insert(index + 1, strengthQuestion);
                    introQuestion.question = "C'� altro che vuole aggiungere?";
                    questions.Insert(index + 2, introQuestion);
                    questionNumber++;

                }
                else if (answerPoints == difettiID)
                {
                    // Debug.Log("DIFETTI");
                    noPointAnswers++;

                    questions.Insert(index + 1, weaknessQuestion);
                    introQuestion.question = "C'� altro che vuole aggiungere?";
                    questions.Insert(index + 2, introQuestion);
                    questionNumber++;

                }
                else if (answerPoints == continueID)
                {
                    //  Debug.Log("CONTINUE");
                    noPointAnswers++;
                    questionNumber++;
                }

                // Se non si � nella introQuestion, il punteggio viene semplicemente sommato

                else points += answerPoints;
            }
            // Se il punteggio � 0, non deve incidere sulla media
            else noPointAnswers++;

            answered++;

            if (answered - noPointAnswers == 0) average = 0;
            else average = points / (answered - noPointAnswers);

            Invoke(nameof(NextQuestion), 1f);
        }

        else Invoke(nameof(ResetInterview), 1f);
    }

    // Imposta il colloquio precaricando le domande da varie cartelle.
    // ATTENZIONE: attualmente carica da 3 cartelle, senza possibilit� di cambiare. Si pu� cambiare per� le 3 cartelle da cui caricare (VEDI VARIABILI SOPRA)

    public void SetupInterview()
    {
        questions.Add(bonusPointsQuestion);
        questionNumber++;

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

        //DebugQuestion(questions[index]);

        // Debug.Log("Ci sono domande n = " + questions.Count);

        questionNumber = questions.Count - 1;
        Setup(questions[index]);
    }

    // SNELLIRE CODICE?? (Tante ripetizioni in tutte le funzioni Get...Question)

    // Cambiare forse frasi e valori bonus e malus

    public Question GetBonusPointsQuestion()
    {
        int id = 1100;

        // Question question = null;
        //= InterviewManager.GetQuestion(introFolder, id);

        // Debug.Log("Valore Edu = " + currentEducationID + " - Valore Occp = " + currentJobID);

        //int gap = currentJobID - currentEducationID;

        //Debug.Log("differenza J-E = " + gap);

        string questionText = "";

        // Solo terza media
        if (currentEducationID == 0)
        {
            if (currentJobID <= 1)
            {
                questionText = "Lei ha solo la terza media... ma va bene.";
                // points -= 0;
            }
            else if (currentJobID <= 3)
            {
                questionText = "Lei ha solo la terza media... pazienza.";
                points -= 5;
            }
            else if (currentJobID >= 4)
            {
                questionText = "Lei ha solo la terza media... INAMMISSIBILE.";
                points -= 50;
            }
        }

        // Solo superiori
        if (currentEducationID == 1)
        {
            if (currentJobID <= 1)
            {
                questionText = "Lei ha un diploma di scuola superiore... Pi� che sufficiente.";
                points += 5;
            }
            else if (currentJobID <= 3)
            {
                questionText = "Lei ha un diploma di scuola superiore...  perfetto.";
                // points += 0;
            }
            else if (currentJobID >= 4)
            {
                questionText = "Lei non ha una laurea... Vedremo.";
                points -= 10;
            }
        }

        // Laurea
        if (currentEducationID == 2)
        {
            if (currentJobID <= 1)
            {
                questionText = "Lei ha una laurea... Molto qualificato.";
                points += 10;
            }
            else if (currentJobID <= 3)
            {
                questionText = "Lei ha una laurea...  pi� che sufficiente.";
                points += 5;
            }
            else if (currentJobID >= 4)
            {
                questionText = "Lei ha una laurea... Bene.";
                // points -= 0;
            }
        }

        // Sezione punti bonus
        // MIGLIORARE INTERFACCIA (prefab Question) per far entrare tutto il testo (POTREBBE ESSERE MOLTO LUNGO)

        // Se non funziona, fare pi� schede (una per "dato")

        if (!cv.esperienza.ToString().Equals("Nessuna"))
        {
            questionText += "\nVedo che ha lavorato in precedenza come " + cv.esperienza.ToString() + "...";
            points++;
        }

        if (!cv.secondaLingua.ToString().Equals("Nessuna"))
        {
            questionText += "\nVedo che parla anche " + cv.secondaLingua.ToString() + "...";
            points++;
        }

        if (!cv.patente.ToString().Equals("Nessuna"))
        {
            questionText += "\nVedo che ha una patente " + cv.patente.ToString() + "...";
            points++;
        }



        Answer[] answers = new Answer[4];
        answers[0].text = "Continua";
        answers[0].points = continueID;

        answers[1].text = InputPanel.disabledText;
        answers[1].points = 0;

        answers[2].text = InputPanel.disabledText;
        answers[2].points = 0;

        answers[3].text = InputPanel.disabledText;
        answers[3].points = 0;

        Question question = new(questionText, answers, id);
        InterviewManager.AddQuestion(question, introFolder);
        return question;
    }

    public Question GetIntroQuestion()
    {

        int id = 1101;

        Question question = InterviewManager.GetQuestion(introFolder, id);

        if (question == null)
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

            question = new(questionText, answers, id);
            InterviewManager.AddQuestion(question, introFolder);
            return question;
        }

        return question;
    }

    public Question GetSoftSkillQuestion()
    {
        int id = 1102;

        Question question = InterviewManager.GetQuestion(introFolder, id);

        if (question == null)
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

            question = new(questionText, answers, id);
            InterviewManager.AddQuestion(question, introFolder);
            return question;
        }
        return question;
    }

    // Domande sui pregi
    public Question GetStrengthQuestion()
    {
        int id = 1103;

        Question question = InterviewManager.GetQuestion(introFolder, id);

        if (question == null)
        {
            string questionText = "Quali pregi possiede?";

            Answer[] answers = new Answer[4];
            answers[0].text = "Onest�";
            answers[0].points = 1f;

            answers[1].text = "Umilt�";
            answers[1].points = 1f;

            answers[2].text = "Affidabilit�";
            answers[2].points = 1f;

            answers[3].text = "Determinazione";
            answers[3].points = 1f;

            question = new(questionText, answers, id);
            InterviewManager.AddQuestion(question, introFolder);
            return question;
        }
        return question;
    }

    // Domande sui difetti
    public Question GetWeaknessQuestion()
    {
        int id = 1104;

        Question question = InterviewManager.GetQuestion(introFolder, id);

        if (question == null)
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

            question = new(questionText, answers, id);
            InterviewManager.AddQuestion(question, introFolder);
            return question;
        }
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
        // CVManager.UnloadCurrentCV();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}