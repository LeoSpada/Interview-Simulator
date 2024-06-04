using System;
using TMPro;
using UnityEngine;

public class CVEntryPanel : MonoBehaviour
{
    // Contiene i vari inputField
    public TMP_InputField[] inputFields;

    
    // public TMP_Dropdown genderDropdown;
    
    public TMP_Dropdown[] dropdowns;

    private bool allClear = true;

    private CVEntry CV;

    public GameObject confirmPanel;

    private void Start()
    {
        // Se in modalità modifica, carica il CV scelto
        if (CVManager.editCurrent)       
            LoadCurrent();      
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

        // Possibile usare foreach??
        // Cambia il tipo di variabile, trovare soluzione

        CVEntry.Genere genere = AcceptDropdown<CVEntry.Genere>(dropdowns[0]);

        CVEntry.Occupazione occupazione = AcceptDropdown<CVEntry.Occupazione>(dropdowns[1]);

        if (allClear)
        {
            CV = new(inputFields[0].text, inputFields[1].text, occupazione, genere);

            // Se un file con lo stesso nome è già presente, compare una finestra di conferma
            if (CVManager.CheckEntry(CV))
            {
                // Debug.Log("CV già presente.");
                confirmPanel.SetActive(true);
            }
            else
            {
                SaveCV();
            }
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
                Debug.Log("Il CV è stato sovrascritto e il file rinominato.\nElimino vecchio file");
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
        // Viene sommato 1 perché il valore 0 del dropdown è la frase "Inserire Genere" (non compatibile con enum
        // Forse rimuovere inserire genere e +1 successivamente
        
        dropdowns[0].value = (int)currentCV.gender + 1;
        dropdowns[1].value = (int)currentCV.job + 1;
    }

    // Se la schermata viene disattivata, si esce dalla modalità modifica
    public void OnDisable()
    {
        CVManager.editCurrent = false;
    }
}
