using System;
using TMPro;
using UnityEngine;
using static InterviewManager.Question;

public class QuestionCreator : MonoBehaviour
{
    private InterviewManager.Question question;

    // public TextMeshProUGUI titleText;

    public TMP_InputField[] inputFields;
    public TMP_InputField[] points;

    // public TMP_Dropdown correctIndex;

    public TMP_Dropdown jobDropdown;

    private bool allClear = true;
    // Start is called before the first frame update
    //void Start()
    //{
    //    if (CVManager.currentCV != null)
    //        titleText.text = CVManager.currentCV.job.ToString();
    //    else titleText.text = "CARICARE CV!!!";
    //}

    public void Start()
    {
        float value = 0.25f;
        foreach (var point in points)
        {
            point.text = value.ToString();
            value += 0.25f;
        }
    }



    // Crea un CV a partire dai campi inseriti. Controlli su file esistenti e campi vuoti prima del salvataggio.
    public void Submit()
    {
        foreach (TMP_InputField inputField in inputFields)
        {
            if (!AcceptInput(inputField))
            {
                // Debug.Log("Campo inesistente");

                inputField.image.color = Color.red;

                allClear = false;

                // Rimettere break riduce i controlli ma non fa colorare di rosso tutti i campi (solo il primo non valido)
                // break;
            }

            else inputField.image.color = Color.white;
        }

        foreach (TMP_InputField point in points)
        {
            if (!AcceptInput(point))
            {
                // Debug.Log("Campo inesistente");

                point.image.color = Color.red;

                allClear = false;

                // Rimettere break riduce i controlli ma non fa colorare di rosso tutti i campi (solo il primo non valido)
                // break;
            }

            else point.image.color = Color.white;
        }
        // AGGIUNGERE CONTROLLO SU Dropdown di correctIndex

        CVEntry.Occupazione occupazione = AcceptDropdown<CVEntry.Occupazione>(jobDropdown);


        if (allClear)
        {
            Answer[] answers = FilterAnswers(inputFields);


            // Assegnazione punti corretti
            for (int i = 0; i < answers.Length; i++)
            {

                Debug.Log("Assegnazione punti");
                answers[i].points = float.Parse(points[i].text);

            }

            question = new(inputFields[0].text, answers);


            // Debug.Log("All clear: " + question);

            //// Se un file con lo stesso nome è già presente, compare una finestra di conferma
            //if (CVManager.CheckEntry(CV))
            //{
            //    // Debug.Log("CV già presente.");
            //    confirmPanel.SetActive(true);
            //}
            //else
            //{
            string job = occupazione.ToString();

            SaveQuestion(job);
            //}

            // Reimposta allClear a true per il prossimo submit
            allClear = true;
        }
    }

    // Controlla che l'input field contenga dati utilizzabili
    public bool AcceptInput(TMP_InputField inputField)
    {
        if (string.IsNullOrWhiteSpace(inputField.text)) return false;
        else return true;

        // nameCompletionSource.SetResult(inputField.text);
    }

    // FUNZIONE CHE RIMUOVE IL PRIMO CAMPO?? ( La domanda vera e propria)
    public Answer[] FilterAnswers(TMP_InputField[] inputs)
    {
        Answer[] answers = new Answer[inputs.Length - 1];

        for (int i = 1; i < inputs.Length; i++)
        {
            answers[i - 1].text = inputs[i].text;
            // answers[i - 1].points = UnityEngine.Random.Range(0, 1f);
        }

        //foreach (string s in answers)
        //{
        //    Debug.Log(s);
        //}

        return answers;
    }

    // Controlla che il valore del dropdown corrisponda ad un valore contenuto nell'enum
    public T AcceptDropdown<T>(TMP_Dropdown dropdown)
    {
        // Ottiene il valore del testo del dropdown
        string dropdownText = dropdown.options[dropdown.value].text;
        // Debug.Log(dropdownText);

        T enumObj;

        // Prova ad ottenere il valore enum corretto corrispondente alla stringa
        try
        {
            enumObj = (T)Enum.Parse(typeof(T), dropdownText);
        }
        catch (ArgumentException)
        {
            allClear = false;

            dropdown.image.color = Color.red;
            Debug.Log("Il valore del dropdown non è consentito nell'Enum.\nRicontrollare codice.");

            return default;
        }

        dropdown.image.color = Color.white;

        // Debug.Log("Enumobj: " + enumObj);
        return enumObj;
    }

    // Conferma il Submit
    public void SaveQuestion(string job)
    {
        InterviewManager.AddQuestion(question, job);
        Debug.Log("SALVATO CON SUCCESSO da SaveQuestion");
    }
}
