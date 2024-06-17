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
    public FaceManager faceManager;

    [Header("CV")]
    private CVEntry cv;
    public string currentJob;
    private int currentJobID;
    public string currentEducation;
    private int currentEducationID;
    private bool hasLowEducation = false;
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

        // Se è stato caricato correttamente un CV
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

            // bonusPointsQuestion = GetBonusPointsQuestion();

            // Debug.Log(currentEducation);

            if (cvInfo)
            {
                cvInfo.gameObject.SetActive(true);
                cvInfo.ReloadInfo();
            }

            SetupInterview();
        }

        else Debug.Log("Nessun CV attualmente caricato. Caricare domanda tramite pulsanti.");
    }

    public void Setup(Question q)
    {

        // Debug.Log(currentJob);

        // Se q è null, le domande sono finite.
        // Rimpiazzare questa parte con caricamento scena di calcolo punteggio??

        // INSERIRE INVOCAZIONE SCENA PUNTEGGIO

        if (q == null)
        {
            Debug.Log("Score = " + points);
            Debug.Log("Average = " + average);
            PlayerPrefs.SetFloat("score", points);
            PlayerPrefs.SetFloat("average", average);

            GameManager.instance.LoadScene("Scena_Punteggio");
            return;

            //question = null;
            //questionText.text = "Domande finite.\nPunteggio: " + points;
            //foreach (Button button in ansButtons)
            //{
            //    button.name = "EmptyAns";
            //    TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            //    buttonText.text = "-Reset-";
            //return;
            //}
        }

        question = q;
        questionText.text = q.question;

        // Se le voci di introQuestion non hanno più risposte disponibili al loro interno, non vengono visualizzate
        if (question.id == introQuestion.id)
        {
            if (CountAnswers(softSkillQuestion) == 0)
            {
                // Debug.Log("Finite risposte per SoftSkillQ");
                question.answers[0].text = InputPanel.disabledText;
            }

            if (CountAnswers(strengthQuestion) == 0)
            {
                //  Debug.Log("Finite risposte per StrengthQ");
                question.answers[1].text = InputPanel.disabledText;
            }

            if (CountAnswers(weaknessQuestion) == 0)
            {
                //  Debug.Log("Finite risposte per WeaknessQ");
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

    // Usata solo in scena di test, commentare in versione finale
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
            questionNumber = questions.Count - 2;
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

                //  Debug.Log("rimossa 1 di soft " + softSkillQuestion.id);

                softSkillQuestion.answers[i].text = InputPanel.disabledText;
                //  DebugQuestion(softSkillQuestion);
            }

            if (question.id == strengthQuestion.id)
            {
                int i = int.Parse(button.tag);

                // Debug.Log("rimossa 1 di strength " + strengthQuestion.id);

                strengthQuestion.answers[i].text = InputPanel.disabledText;
                //   DebugQuestion(strengthQuestion);
            }

            if (question.id == weaknessQuestion.id)
            {
                int i = int.Parse(button.tag);

                // Debug.Log("rimossa 1 di weakness " + weaknessQuestion.id);

                weaknessQuestion.answers[i].text = InputPanel.disabledText;
                //  DebugQuestion(weaknessQuestion);
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
                    introQuestion.question = "C'è altro che vuole aggiungere?";
                    questions.Insert(index + 2, introQuestion);
                    questionNumber++;
                }
                else if (answerPoints == pregiID)
                {
                    // Debug.Log("PREGI");
                    noPointAnswers++;

                    questions.Insert(index + 1, strengthQuestion);
                    introQuestion.question = "C'è altro che vuole aggiungere?";
                    questions.Insert(index + 2, introQuestion);
                    questionNumber++;

                }
                else if (answerPoints == difettiID)
                {
                    // Debug.Log("DIFETTI");
                    noPointAnswers++;

                    questions.Insert(index + 1, weaknessQuestion);
                    introQuestion.question = "C'è altro che vuole aggiungere?";
                    questions.Insert(index + 2, introQuestion);
                    questionNumber++;

                }
                else if (answerPoints == continueID)
                {
                    //  Debug.Log("CONTINUE");
                    noPointAnswers++;
                    questionNumber++;
                }

                // Se non si è nella introQuestion, il punteggio viene semplicemente sommato

                else points += answerPoints;
            }
            // Se il punteggio è 0, non deve incidere sulla media
            else noPointAnswers++;

            answered++;

            if (answered - noPointAnswers == 0) average = 0;
            else average = points / (answered - noPointAnswers);

            // Cambio espressione
            faceManager.ChangeExpression(average >= 0.75f);

            Invoke(nameof(NextQuestion), 1f);
        }

       // else Invoke(nameof(ResetInterview), 1f);
    }

    // Imposta il colloquio precaricando dialoghi e le domande da varie cartelle.
    // ATTENZIONE: attualmente carica da 3 cartelle, senza possibilità di cambiare. Si può cambiare però le 3 cartelle da cui caricare (VEDI VARIABILI SOPRA)

    public void SetupInterview()
    {

        string dialogue = "";
        string dialogueAnswer = "";
        string[] otherAnswers =
        {
            "",
            "",
            "",
        };

        Debug.Log("punteggio = " + points);

        if (questions == null)
        {
            Debug.Log("Lista nulla");
            return;
        }

        // FASE 0: Introduzione
        // Dialoghi senza punti

        questions.Add(GetDialogue($"Lei è {cv.name} {cv.surname}, giusto?", "Sì, sono io."));
        questionNumber++;

        questions.Add(GetDialogue($"Molto bene, {cv.surname}. Le va di cominciare?", "Certo, va bene."));
        questionNumber++;

        //questions.Add(bonusPointsQuestion);
        //questionNumber++;

        // FASE 1: Presentazioni
        // Parte un loop in cui il giocatore può parlare di sè (pregi, difetti, soft skill) finché vuole.
        // Incide sul punteggio.
        questions.Add(introQuestion);
        questionNumber++;

        // FASE 2 parte 1: Vecchie carriere lavorative
        // Il datore di lavoro nota la presenza o meno di altre esperienze sul CV.
        // Non influisce sul punteggio.

        dialogue = "Leggendo il suo CV, ho notato che Lei ";

        if (!cv.esperienza.ToString().Equals("Nessuna"))
        {
            dialogue += $"ha lavorato in precedenza come {cv.esperienza}.";
            dialogueAnswer = "Mi ha aiutato nel mio percorso lavorativo";
            otherAnswers[0] = "La rifarei se fosse possibile";
            otherAnswers[1] = $"L'esperienza da {cv.esperienza} è stata impegnativa";
        }
        else
        {
            dialogue += "non ha avuto alcuna esperienza lavorativa passata.";
            dialogueAnswer = "Non ho mai avuto l'occasione giusta";
            otherAnswers[0] = "";
            otherAnswers[1] = "";
        }

        dialogue += "\nMi può dire qualcosa a riguardo?";

        questions.Add(GetDialogue(dialogue, dialogueAnswer, otherAnswers[0], otherAnswers[1]));
        questionNumber++;

        // Fase 2 parte 2: Istruzione e formazione
        // Il datore di lavoro nota la presenza o meno di un'istruzione adeguata al lavoro.
        // Non influisce sul punteggio.

        dialogue = "Leggendo il suo CV, ho notato che Lei ";
        dialogue += GetEducationMessage();

        if (hasLowEducation)
        {
            dialogueAnswer = "La prego, mi dia comunque una possibilità";
            otherAnswers[0] = "Imparerò in fretta, ho tanta ambizione";
        }
        else
        {
            dialogueAnswer = "Con i miei studi, sono pronto ad affrontare la sfida";
            otherAnswers[0] = "Il mio percorso formativo mi ha preparato per questo lavoro";
        }

        dialogue += "\nMi può dire qualcosa a riguardo?";

        questions.Add(GetDialogue(dialogue, dialogueAnswer, otherAnswers[0]));
        questionNumber++;

        // Fase 2 parte 3: Dati aggiuntivi
        // Il datore osserva dati aggiuntivi quali patente e seconda lingua.
        // Non incide sul punteggio.

        if (!cv.secondaLingua.ToString().Equals("Nessuna"))
        {
            dialogue = $"Noto inoltre che Lei parla anche {cv.secondaLingua}.\nComplimenti, sapere più lingue è un valore aggiunto che apprezziamo.";
            dialogueAnswer = "La ringrazio";
            otherAnswers[0] = "Conoscere più lingue mi appassiona";
            questions.Add(GetDialogue(dialogue, dialogueAnswer, otherAnswers[0]));
            questionNumber++;
        }

        if (!cv.patente.ToString().Equals("Nessuna"))
        {
            dialogue = $"Perfetto, ha anche una patente.\nPatente di tipo {cv.patente}, giusto? Potrebbe tornare utile.";
            dialogueAnswer = "Giusto, confermo";
            otherAnswers[0] = "Sì, sono automunito e disposto a viaggiare per lavoro";
            questions.Add(GetDialogue(dialogue, dialogueAnswer, otherAnswers[0]));
            questionNumber++;
        }

        // Fase 3: Colloquio
        // Fase a punti, vengono caricate le tre cartelle prefissate (1 per lavoro caricato da curriculum).
        // Influisce su punteggio e media

        questions.Add(GetDialogue("Perfetto, direi che possiamo iniziare con domande vere e proprie, per testare la sua preparazione.", "Sono pronto", "Perfetto"));
        questionNumber++;

        startQuestions = QuestionLimit(startQuestions, startFolder);
        jobQuestions = QuestionLimit(startQuestions, currentJob);
        endQuestions = QuestionLimit(startQuestions, endFolder);


        int i, j, k;

        for (i = 0; i < startQuestions; i++) questions.Add(GetRandomQuestion(startFolder));

        questions.Add(GetDialogue($"Parliamo adesso del lavoro che cerca.\nIn quanto aspirante {cv.occupazione}, dobbiamo testare la sua preparazione.", "Sono pronto", "Perfetto"));
        questionNumber++;

        for (j = i; j < i + jobQuestions; j++) questions.Add(GetRandomQuestion(currentJob));

        questions.Add(GetDialogue($"Abbiamo quasi finito.\nAltre {endQuestions} domande per decidere l'esito del colloquio.", "Sono pronto", "Perfetto"));
        questionNumber++;

        for (k = j; k < j + endQuestions; k++) questions.Add(GetRandomQuestion(endFolder));

        questions.Add(GetDialogue($"La ringraziamo per il Suo tempo, {cv.surname}.\nSaprà al più presto la nostra decisione.\nLe auguriamo buona giornata.", "Grazie a Lei, buona giornata."));
        questionNumber++;

        // Aggiunge una domanda nulla per segnalare a setup la fine del colloquio
        questions.Add(null);

        SetupInterviewInfo();
        Setup(questions[index]);
    }

   // Domanda non salvata, usata per transizioni e commenti del datore
    public Question GetDialogue(string dialogue, string answer0, string answer1 = "", string answer2 = "", string answer3 = "", float points = 0)
    {
        int id = 1099;

        Answer[] answers = new Answer[4];
        answers[0].text = answer0;
        answers[0].points = points;

        if (answer1.Equals("")) answer1 = InputPanel.disabledText;
        if (answer2.Equals("")) answer2 = InputPanel.disabledText;
        if (answer3.Equals("")) answer3 = InputPanel.disabledText;

        answers[1].text = answer1;
        answers[1].points = points;

        answers[2].text = answer2;
        answers[2].points = points;

        answers[3].text = answer3;
        answers[3].points = points;

        Question question = new(dialogue, answers, id);

        return question;

    }    

    // Restituisce un messaggio per la differenza tra educazione e posizione, e segnala una differenza troppo negativa
    public string GetEducationMessage()
    {
        // Solo terza media
        if (currentEducationID == 0)
        {
            if (currentJobID <= 1)
            {
                return "ha solo la terza media... ma va bene.";
                // points -= 0;
            }
            else if (currentJobID <= 3)
            {
                hasLowEducation = true;
                return "ha solo la terza media... pazienza.";
                // points -= 5;
            }
            else if (currentJobID >= 4)
            {
                hasLowEducation = true;
                return "ha solo la terza media.\nCiò non può essere sufficiente per la posizione richiesta.";
                // points -= 50;
            }
        }

        // Solo superiori
        if (currentEducationID == 1)
        {
            if (currentJobID <= 1)
            {
                return "ha un diploma di scuola superiore.\nPiù che sufficiente.";
                // points += 5;
            }
            else if (currentJobID <= 3)
            {
                return "ha un diploma di scuola superiore.\nPerfetto.";
                // points += 0;
            }
            else if (currentJobID >= 4)
            {
                hasLowEducation = true;
                return "non ha una laurea.\nVedremo di provvedere alla Sua formazione, per quanto possibile.";                
                // points -= 10;
            }
        }

        // Laurea
        if (currentEducationID == 2)
        {
            if (currentJobID <= 1)
            {
                return "ha una laurea.\nMolto qualificato.";
                // points += 10;
            }
            else if (currentJobID <= 3)
            {
                return "ha una laurea.\nPiù che sufficiente.";
                // points += 5;
            }
            else if (currentJobID >= 4)
            {
                return $"ha una laurea.\nBene, la formazione è fondamentale per essere {cv.occupazione}.";
                // points -= 0;
            }
        }

        return "non sa leggere e scrivere. Quindi non sta leggendo il mio dialogo.";
    }

    // Prepara la domanda introduttiva, in cui il candidato parla di sè.
    // Usando gli ID, questa domanda diventa un menù.
    // Ogni risposta porta ad una domanda diversa.

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
            answers[0].text = "Sono in grado di lavorare in team per raggiungere obiettivi comuni";
            answers[0].points = 1f;

            answers[1].text = "Sono in grado di guidare e ispirare il mio team";
            answers[1].points = 1f;

            answers[2].text = "Uso creatività e tenacia per trovare soluzioni efficaci alle sfide";
            answers[2].points = 1f;

            answers[3].text = "Rispetto le scadenze pianificando con cura le attività e le loro priorità";
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
            answers[0].text = "Dico quello che penso senza troppi riguardi";
            answers[0].points = 1f;

            answers[1].text = "Sono bravo nel riconoscere i miei limiti";
            answers[1].points = 1f;

            answers[2].text = "La gente si fida facilmente di me";
            answers[2].points = 1f;

            answers[3].text = "Faccio di tutto per ottenere i miei obiettivi";
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
            answers[0].text = "Se provocato, mi arrabbio molto facilmente";
            answers[0].points = 0.25f;

            answers[1].text = "Spesso mi capita di non voler fare nulla";
            answers[1].points = 0.25f;

            answers[2].text = "A volte, desidero lo stesso successo degli altri";
            answers[2].points = 0.25f;

            answers[3].text = "Molte volte ho la testa fra le nuvole";
            answers[3].points = 0.25f;

            question = new(questionText, answers, id);
            InterviewManager.AddQuestion(question, introFolder);
            return question;
        }
        return question;
    }

    // Se sono state richieste più domande di quante ve ne siano nella cartella scelta, viene restituita la cartella scelta
    public int QuestionLimit(int counter, string folder, bool log = true)
    {
        {
            int folderSize = GetJobFolderSize(folder);
            if (counter > folderSize)
            {
                if (log) Debug.Log($"Domande richieste ({counter}) > Domande trovate in cartella '{folder}' ({folderSize}). Caricamento di tutte le domande in '{folder}'.");
                return GetJobFolderSize(folder);
            }
            else
            {
                if (log) Debug.Log($"Domande richieste ({counter}) <= Domande trovate in cartella '{folder}' ({folderSize}).");
                return counter;
            }
        }
    }

    // RIMUOVERE / COMMENTARE DA VERSIONE FINALE

    public void ResetInterview()
    {
        // CVManager.UnloadCurrentCV();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}