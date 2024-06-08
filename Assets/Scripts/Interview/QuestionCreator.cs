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

    // private bool allClear = true;

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
        foreach (TMP_InputField inputField in inputFields)
        {
            if (!InputPanel.AcceptInput(inputField))
            {
                // Debug.Log("Campo inesistente");

                inputField.image.color = Color.red;

                InputPanel.fieldsClear = false;
            }

            else inputField.image.color = Color.white;
        }

        foreach (TMP_InputField point in points)
        {
            if (!InputPanel.AcceptInput(point))
            {
                // Debug.Log("Campo inesistente");

                point.image.color = Color.red;

                InputPanel.fieldsClear = false;

                // Rimettere break riduce i controlli ma non fa colorare di rosso tutti i campi (solo il primo non valido)
                // break;
            }

            else point.image.color = Color.white;
        }



        if (customActive)
        {
            folder = customFolder.text;
            Debug.Log("Custom concesso: salvataggio andrà in " + folder);
        }
        else
        {


            CVEntry.Occupazione occupazione = InputPanel.AcceptDropdown<CVEntry.Occupazione>(folderDropdown, false, true);

            if (!InputPanel.dropdownsClear)
            {

                folder = folderDropdown.captionText.text;
                Debug.Log("Non è custom, ma " + folder);
                // Debug.Log(folder);
                //if (folder.Equals("Custom"))
                //{
                //    customFolder.gameObject.SetActive(true);
                //}
                // InputPanel.allClear = true;
            }
            else folder = occupazione.ToString();
        }

        //  if (occupazione == default) allClear = false;

        if (InputPanel.fieldsClear)
        {
            Answer[] answers = FilterAnswers(inputFields);

            // Assegnazione punti corretti
            for (int i = 0; i < answers.Length; i++)
            {
                // Debug.Log("Assegnazione punti");
                answers[i].points = float.Parse(points[i].text);
            }

            question = new(inputFields[0].text, answers);

            // string folder = occupazione.ToString();

            SaveQuestion(folder);
        }
        // Reimposta allClear a true per il prossimo submit
        InputPanel.fieldsClear = true;
        InputPanel.dropdownsClear = true;
    }

    // FUNZIONE CHE RIMUOVE IL PRIMO CAMPO?? ( La domanda vera e propria)
    public Answer[] FilterAnswers(TMP_InputField[] inputs)
    {
        Answer[] answers = new Answer[inputs.Length - 1];

        for (int i = 1; i < inputs.Length; i++)
        {
            answers[i - 1].text = inputs[i].text;
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
