using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static InterviewManager.Question;

public class QuestionCreator : MonoBehaviour
{
    private InterviewManager.Question question;

    public TMP_InputField questionField;
    public TMP_InputField[] answerFields;
    public TMP_InputField[] pointFields;

    public TMP_InputField customFolder;

    private bool customActive = false;

    public TMP_Dropdown folderDropdown;

    public TMP_Dropdown answersSizeDropdown;

    public GameObject confirmPanel;

    private string folder = "";

    public void Start()
    {
        // Imposta casualmente i campi dei punti
        RandomizePointPosition();
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

    public void RandomizePointPosition()
    {
        List<float> values = new() { 0.25f, 0.5f, 0.75f, 1f };
        int i;
        foreach (var point in pointFields)
        {
            if (!point.text.Equals(InputPanel.disabledText))
            {
                // value = UnityEngine.Random.Range(0.25f, 1f);
                i = UnityEngine.Random.Range(0, values.Count);

                point.text = values[i].ToString();
                Debug.Log("Rimuovo " + values[i]);
                values.Remove(values[i]);
            }
            else Debug.Log("Disabilitata risposta "+point.name);
        }
    }

    // Usata da OnValueChanged di answersSizeDropdown
    public void ChangeAnswersSize()
    {

        int size = int.Parse(answersSizeDropdown.captionText.text);

        Debug.Log("Dim inserita = " + size);


        foreach (var ans in answerFields)
        {

            ans.DeactivateInputField(true);
            ans.enabled = false;

        }

        int i;
        for (i = 0; i < size; i++)
        {
            InputPanel.EnableInputField(answerFields[i]);
            InputPanel.EnableInputField(pointFields[i]);
        }

        for (int j = i; j < answerFields.Length; j++)
        {
            InputPanel.DisableInputField(answerFields[j]);
            InputPanel.DisableInputField(pointFields[j]);
        }
    }

    public void CheckAllInputs()
    {
        InputPanel.AcceptInputField(questionField);
        InputPanel.AcceptInputFields(answerFields);
        InputPanel.AcceptInputFields(pointFields);
    }

    // Crea un CV a partire dai campi inseriti. Controlli su file esistenti e campi vuoti prima del salvataggio.
    public void Submit()
    {

        CheckAllInputs();

        // Se il dropdown è impostato su custom, controlla che il nome cartella inserito da utente sia valido
        if (customActive) InputPanel.AcceptInputField(customFolder);
        else
        {
            CVEntry.Occupazione occupazione = InputPanel.AcceptDropdown<CVEntry.Occupazione>(folderDropdown, false, true);

            // Se il dropdown non corrisponde ad una occupazione, usa il valore del dropdown come nome cartella
            if (!InputPanel.dropdownsClear)
            {
                folder = folderDropdown.captionText.text;
                // Debug.Log("Non è custom, ma " + folder);
            }
            else folder = occupazione.ToString();
        }

        if (InputPanel.fieldsClear)
        {
            // Formatta correttamente le risposte e assegna i punti
            Answer[] answers = CreateAnswers(answerFields);

            question = new(questionField.text, answers);

            // Se il dropdown è impostato su custom, usa il valore dell'input field come nome cartella
            if (customActive)
            {
                folder = customFolder.text;
                // Debug.Log("Custom concesso: salvataggio andrà in " + folder);
            }

            SaveQuestion(folder);
        }

        // Reimposta allClear a true per il prossimo submit
        InputPanel.ClearAll();
    }

    // Formatta il contenuto degli input field per diventare delle Answer compatibili con Question, con testo e punti
    public Answer[] CreateAnswers(TMP_InputField[] inputs)
    {
        Answer[] answers = new Answer[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            answers[i].text = inputs[i].text;

            // Assegnazione punti corretti
            answers[i].points = float.Parse(pointFields[i].text);
        }

        return answers;
    }

    // Conferma il Submit
    public void SaveQuestion(string folder)
    {
        InterviewManager.AddQuestion(question, folder);
        Debug.Log("SALVATO CON SUCCESSO IN " + folder);

        if (confirmPanel) confirmPanel.SetActive(true);

        // Randomizza nuovamente la posizione dei punti per incentivare il creatore delle domande a cambiare posizioni delle risposte giuste
        RandomizePointPosition();
    }
}