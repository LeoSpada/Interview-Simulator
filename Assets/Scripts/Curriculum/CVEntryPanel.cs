using System;
using TMPro;
using UnityEngine;

public class CVEntryPanel : MonoBehaviour
{
    // Contiene i vari inputField
    public TMP_InputField[] inputFields;

    
    // public TMP_Dropdown genderDropdown;
    
    public TMP_Dropdown[] dropdowns;

   // private bool allClear = true;

    private CVEntry CV;

    public GameObject confirmPanel;

    private void Start()
    {
        // Se in modalit� modifica, carica il CV scelto
        if (CVManager.editCurrent)       
            LoadCurrent();      
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

                // Rimettere break riduce i controlli ma non fa colorare di rosso tutti i campi (solo il primo non valido)
                // break;
            }

            else inputField.image.color = Color.white;
        }

        // Possibile usare foreach??
        // Cambia il tipo di variabile, trovare soluzione

        CVEntry.Genere genere = InputPanel.AcceptDropdown<CVEntry.Genere>(dropdowns[0]);

        // if (genere == default) allClear = false;

        CVEntry.Occupazione occupazione = InputPanel.AcceptDropdown<CVEntry.Occupazione>(dropdowns[1]);

       // if (occupazione == default) allClear = false;

        if (InputPanel.fieldsClear && InputPanel.dropdownsClear)
        {
            CV = new(inputFields[0].text, inputFields[1].text, occupazione, genere);

            // Se un file con lo stesso nome � gi� presente, compare una finestra di conferma
            if (CVManager.CheckEntry(CV))
            {
                // Debug.Log("CV gi� presente.");
                confirmPanel.SetActive(true);
            }
            else
            {
                SaveCV();
            }
        }
        // Reimposta allClear a true per il prossimo submit
        InputPanel.fieldsClear = true;
        InputPanel.dropdownsClear = true;
    }

    // Conferma il Submit
    public void SaveCV()
    {
        CVManager.AddCVEntry(CV);
        Debug.Log("SALVATO CON SUCCESSO");

        // Se l'operazione era una sovrascrittura
        if (CVManager.editCurrent)
        {
            CVManager.editCurrent = false;

            // Se il file durante la sovrascrittura ha cambiato nome
            if (!CVManager.IsCurrentCV(inputFields[0].text, inputFields[1].text))
            {
                Debug.Log("Il CV � stato sovrascritto e il file rinominato.\nElimino vecchio file");
                CVManager.RemoveCVEntry(CVManager.currentCV);
            }
        }
    }

    // Carica il CV scelto e ne inserisce i dati nei campi di input per la modifica.
    public void LoadCurrent()
    {
        CVEntry currentCV = CVManager.currentCV;
        Debug.Log("MODALITA' MODIFICA - " + currentCV.name + " " + currentCV.surname);


        inputFields[0].text = currentCV.name;
        inputFields[1].text = currentCV.surname;
       // inputFields[2].text = currentCV.job;
        
        // Debug.Log(((int)currentCV.gender));
        // Viene sommato 1 perch� il valore 0 del dropdown � la frase "Inserire Genere" (non compatibile con enum
        // Forse rimuovere inserire genere e +1 successivamente
        
        dropdowns[0].value = (int)currentCV.gender + 1;
        dropdowns[1].value = (int)currentCV.job + 1;
    }

    // Se la schermata viene disattivata, si esce dalla modalit� modifica
    public void OnDisable()
    {
        CVManager.editCurrent = false;
    }
}
