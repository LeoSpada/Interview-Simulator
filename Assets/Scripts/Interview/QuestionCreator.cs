using System;
using TMPro;
using UnityEngine;
using static InterviewManager.Question;

public class QuestionCreator : MonoBehaviour
{
    private InterviewManager.Question question;

    public TMP_InputField[] inputFields;
    public TMP_InputField[] points;

    public TMP_InputField customFolder;

    private bool customActive = false;

    public TMP_Dropdown folderDropdown;

    private string folder = "";

    public void Start()
    {
        float value = 0.25f;
        foreach (var point in points)
        {
            point.text = value.ToString();
            value += 0.25f;
        }
    }

    public void Update()
    {
        if (folderDropdown.captionText.text.Equals("Custom"))
        {
            customFolder.gameObject.SetActive(true);
            customActive = true;
        }
        else
        {
            customFolder.gameObject.SetActive(false);
            customActive = false;
        }
    }

    // Crea un CV a partire dai campi inseriti. Controlli su file esistenti e campi vuoti prima del salvataggio.
    public void Submit()
    {
        InputPanel.AcceptInputFields(inputFields);
        InputPanel.AcceptInputFields(points);

        // Se il dropdown è impostato su custom, controlla che il nome cartella inserito da utente sia valido
        if (customActive) InputPanel.AcceptInputField(customFolder);

        else
        {
            CVEntry.Occupazione occupazione = InputPanel.AcceptDropdown<CVEntry.Occupazione>(folderDropdown, false, true);

            // Se il dropdown non corrisponde ad una occupazione, usa il valore del dropdown come nome cartella
            if (!InputPanel.dropdownsClear)
            {
                folder = folderDropdown.captionText.text;
                Debug.Log("Non è custom, ma " + folder);
            }
            else folder = occupazione.ToString();
        }

        if (InputPanel.fieldsClear)
        {
            // Separa la domanda dalle risposte
            Answer[] answers = CreateAnswers(inputFields);



            question = new(inputFields[0].text, answers);

            // Se il dropdown è impostato su custom, usa il valore dell'input field come nome cartella
            if (customActive)
            {
                folder = customFolder.text;
                Debug.Log("Custom concesso: salvataggio andrà in " + folder);
            }

            SaveQuestion(folder);
        }

        // Reimposta allClear a true per il prossimo submit
        InputPanel.ClearAll();
    }

    // Formatta il contenuto degli input field per diventare delle Answer compatibili con Question, con testo e punti

    // ATTENZIONE: Attualmente prende tutti gli input field, anche quello della domanda (valore 0). Modificare se si decide di separare il singolo inputField della question dalle altre
    public Answer[] CreateAnswers(TMP_InputField[] inputs)
    {
        Answer[] answers = new Answer[inputs.Length - 1];

        for (int i = 1; i < inputs.Length; i++)
        {
            answers[i - 1].text = inputs[i].text;

            // Assegnazione punti corretti
            answers[i - 1].points = float.Parse(points[i - 1].text);
        }

        return answers;
    }

    // Conferma il Submit
    public void SaveQuestion(string folder)
    {
        InterviewManager.AddQuestion(question, folder);
        Debug.Log("SALVATO CON SUCCESSO IN " + folder);
    }
}
