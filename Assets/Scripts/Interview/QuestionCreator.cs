using TMPro;
using UnityEngine;

public class QuestionCreator : MonoBehaviour
{
    private InterviewManager.Question question;

    public TextMeshProUGUI titleText;

    public TMP_InputField[] inputFields;

    public TMP_Dropdown correctIndex;

    private bool allClear = true;
    // Start is called before the first frame update
    void Start()
    {
        if (CVManager.currentCV != null)
            titleText.text = CVManager.currentCV.job.ToString();
        else titleText.text = "CARICARE CV!!!";
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

        // AGGIUNGERE CONTROLLO SU Dropdown di correctIndex


        if (allClear)
        {
            question = new(inputFields[0].text, FilterAnswers(inputFields), correctIndex.value);


            Debug.Log("All clear: " + question);

            //// Se un file con lo stesso nome è già presente, compare una finestra di conferma
            //if (CVManager.CheckEntry(CV))
            //{
            //    // Debug.Log("CV già presente.");
            //    confirmPanel.SetActive(true);
            //}
            //else
            //{
            SaveQuestion();
            //}
        }
        // Reimposta allClear a true per il prossimo submit
        allClear = true;
    }

    // Controlla che l'input field contenga dati utilizzabili
    public bool AcceptInput(TMP_InputField inputField)
    {
        if (string.IsNullOrWhiteSpace(inputField.text)) return false;
        else return true;

        // nameCompletionSource.SetResult(inputField.text);
    }

    // FUNZIONE CHE RIMUOVE IL PRIMO CAMPO?? ( La domanda vera e propria)
    public string[] FilterAnswers(TMP_InputField[] inputs)
    {
        string[] answers = new string[inputs.Length - 1];

        for (int i = 1; i < inputs.Length; i++)
        {
            answers[i - 1] = inputs[i].text;
        }

        foreach (string s in answers)
        {
            Debug.Log(s);
        }

        return answers;
    }

    // Controlla che il valore del dropdown corrisponda ad un valore contenuto nell'enum
    //public T AcceptDropdown<T>(TMP_Dropdown dropdown)
    //{
    //    // Ottiene il valore del testo del dropdown
    //    string dropdownText = dropdown.options[dropdown.value].text;
    //    // Debug.Log(dropdownText);

    //    T enumObj;

    //    // Prova ad ottenere il valore enum corretto corrispondente alla stringa
    //    try
    //    {
    //        enumObj = (T)Enum.Parse(typeof(T), dropdownText);
    //    }
    //    catch (ArgumentException)
    //    {
    //        allClear = false;

    //        dropdown.image.color = Color.red;
    //        Debug.Log("Il valore del dropdown non è consentito nell'Enum.\nRicontrollare codice.");

    //        return default;
    //    }

    //    dropdown.image.color = Color.white;

    //    // Debug.Log("Enumobj: " + enumObj);
    //    return enumObj;
    //}

    // Conferma il Submit
    public void SaveQuestion()
    {
        InterviewManager.AddQuestion(question);
        Debug.Log("SALVATO CON SUCCESSO");        
    }
}
