using System;
using TMPro;
using UnityEngine;

public class CVEntryPanel : MonoBehaviour
{
    // Contiene i vari inputField
    public TMP_InputField[] inputFields;

    public TMP_Dropdown[] dropdowns;

    private CVEntry CV;

    public GameObject overwritePanel;
    public GameObject confirmPanel;
    private TextMeshProUGUI confirmMessage;

    private void Start()
    {
        // Se in modalit� modifica, carica il CV scelto
        if (CVManager.editCurrent)
            LoadCurrent();

        if (confirmPanel)
            confirmMessage = confirmPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Crea un CV a partire dai campi inseriti. Controlli su file esistenti e campi vuoti prima del salvataggio.
    public void Submit()
    {
        InputPanel.AcceptInputFields(inputFields);

        // Trovare modo di migliorare leggibilit� per il programmatore
        // Possibile mettere un identificativo migliore senza rimuovere array?
        // Genderdropdown anzich� dropdowns[0]

        CVEntry.Genere genere = InputPanel.AcceptDropdown<CVEntry.Genere>(dropdowns[0]);
        CVEntry.Occupazione occupazione = InputPanel.AcceptDropdown<CVEntry.Occupazione>(dropdowns[1]);


        if (InputPanel.fieldsClear && InputPanel.dropdownsClear)
        {
            CV = new(inputFields[0].text, inputFields[1].text, occupazione, genere);

            // Se un file con lo stesso nome � gi� presente, compare una finestra di conferma
            if (CVManager.CheckEntry(CV))
            {
                // Debug.Log("CV gi� presente.");
                overwritePanel.SetActive(true);
            }
            else
            {
                confirmPanel.SetActive(true);
                SaveCV();
            }
        }
        // Reimposta allClear a true per il prossimo submit
        InputPanel.ClearAll();
    }

    // Conferma il Submit
    public void SaveCV()
    {
        CVManager.AddCVEntry(CV);
        Debug.Log("SALVATO CON SUCCESSO");
        confirmMessage.text = "CV salvato con successo.";

        // Se l'operazione era una sovrascrittura
        if (CVManager.editCurrent)
        {
            CVManager.editCurrent = false;

            // Se il file durante la sovrascrittura ha cambiato nome
            if (!CVManager.IsCurrentCV(inputFields[0].text, inputFields[1].text))
            {
                Debug.Log("Il CV � stato sovrascritto e il file rinominato.\nElimino vecchio file");
                confirmMessage.text += "\nIl CV � stato sovrascritto e il file rinominato.";
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

        // Debug.Log(((int)currentCV.gender));
        // Viene sommato 1 perch� il valore 0 del dropdown � la frase "Inserire Genere" (non compatibile con enum)
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
