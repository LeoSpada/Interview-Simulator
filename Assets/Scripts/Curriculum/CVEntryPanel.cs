using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CVEntryPanel : MonoBehaviour
{
    // Contiene i vari inputField
    public TMP_InputField[] inputFields;

    public TMP_Dropdown[] dropdowns;
    public GameObject rigaQualifica;
    public TMP_Dropdown sottoQualifica;

    private CVEntry CV;

    public GameObject overwritePanel;
    public GameObject confirmPanel;
    private TextMeshProUGUI confirmMessage;

    private void Start()
    {
        // Se in modalità modifica, carica il CV scelto
        if (CVManager.editCurrent)
            LoadCurrent();

        if (confirmPanel)
            confirmMessage = confirmPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Crea un CV a partire dai campi inseriti. Controlli su file esistenti e campi vuoti prima del salvataggio.
    public void Submit()
    {
        InputPanel.AcceptInputFields(inputFields);

        // Trovare modo di migliorare leggibilità per il programmatore
        // Possibile mettere un identificativo migliore senza rimuovere array?
        // Genderdropdown anziché dropdowns[0]

        CVEntry.Genere genere = InputPanel.AcceptDropdown<CVEntry.Genere>(dropdowns[0]);
        CVEntry.Occupazione occupazione = InputPanel.AcceptDropdown<CVEntry.Occupazione>(dropdowns[1]);

        //CVEntry.Istruzione istruzione;


        Istruzione.Qualifica qualifica = InputPanel.AcceptDropdown<Istruzione.Qualifica>(dropdowns[2]);
        Istruzione.Titolo titolo;

        if (sottoQualifica && qualifica != Istruzione.Qualifica.Medie)
        {
            titolo = InputPanel.AcceptDropdown<Istruzione.Titolo>(sottoQualifica);
        }
        else titolo = Istruzione.Titolo.Nessuno;



        //Debug.Log(qualifica.Qualifica);
        //

        // AGGIUNGERE NUOVI DROPDOWN QUI
        // FORSE FARE UNA FUNZIONE DA USARE CON OnValueChanged dei dropdown che svelano altri dropdown



        if (InputPanel.fieldsClear && InputPanel.dropdownsClear)
        {
            Istruzione istruzione = new(qualifica, titolo);

            CV = new(inputFields[0].text, inputFields[1].text, occupazione, genere, istruzione);

            // Se un file con lo stesso nome è già presente, compare una finestra di conferma
            if (CVManager.CheckEntry(CV))
            {
                // Debug.Log("CV già presente.");
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
                Debug.Log("Il CV è stato sovrascritto e il file rinominato.\nElimino vecchio file");
                confirmMessage.text += "\nIl CV è stato sovrascritto e il file rinominato.";
                CVManager.RemoveCVEntry(CVManager.currentCV);
            }
        }
    }

    public void ShowTitoloDropdown()
    {
        Istruzione.Qualifica qualifica = InputPanel.AcceptDropdown<Istruzione.Qualifica>(dropdowns[2]);

        sottoQualifica = rigaQualifica.GetComponentInChildren<TMP_Dropdown>();

        // Istruzione istruzione = new(qualifica);
        //Debug.Log(qualifica.Qualifica);

        string qualificaText = qualifica.ToString();
        if (qualificaText.Equals("Medie"))
        {
            // Debug.Log("medie");
            sottoQualifica.ClearOptions();
            rigaQualifica.SetActive(false);
        }
        else //if (qualificaText.Equals("Superiori"))
        {
            Debug.Log("SUPERIORI");
            rigaQualifica.SetActive(true);
            UpdateTitoloDropdownValues(qualificaText);
        }
    }

    public void UpdateTitoloDropdownValues(string qualifica)
    {
        sottoQualifica.ClearOptions();
        List<string> options = new();

        //{
        //    "Nessuno",
        //    "ITIS",
        //    "IPSIA",
        //    "Liceo"
        //};

        if (qualifica.Equals("Superiori"))
        {
            options = new()
            {
                "Liceo",
                "ITIS",
                "IPSIA"
            };


        }
        else if (qualifica.Equals("Università"))
        {
            options = new()
            {
                "Laurea 1",
                "Laurea 2",
                "Laurea 3"
            };
        }

        sottoQualifica.AddOptions(options);
    }

    // Carica il CV scelto e ne inserisce i dati nei campi di input per la modifica.
    public void LoadCurrent()
    {
        CVEntry currentCV = CVManager.currentCV;
        Debug.Log("MODALITA' MODIFICA - " + currentCV.name + " " + currentCV.surname);


        inputFields[0].text = currentCV.name;
        inputFields[1].text = currentCV.surname;

        // Debug.Log(((int)currentCV.genere));
        // Viene sommato 1 perché il valore 0 del dropdown è la frase "Inserire Genere" (non compatibile con enum)
        // Forse rimuovere inserire genere e +1 successivamente

        dropdowns[0].value = (int)currentCV.genere + 1;
        dropdowns[1].value = (int)currentCV.occupazione + 1;
    }

    // Se la schermata viene disattivata, si esce dalla modalità modifica
    public void OnDisable()
    {
        CVManager.editCurrent = false;
    }
}
