using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CVEntryPanel : MonoBehaviour
{
    [Header("Valori di input")]
    private CVEntry CV;
    private CVEntry.Genere genere;
    private CVEntry.Occupazione occupazione;
    private Istruzione.Qualifica qualifica;
    private Istruzione.Titolo titolo;
    CVEntry.Esperienza esperienza;
    CVEntry.Lingua lingua;
    CVEntry.Patente patente;


    // Inserire qui tutti quelli di submit

    // Contiene i vari inputField
    [Header("Campi liberi")]
    public TMP_InputField[] inputFields;

    // public TMP_Dropdown[] dropdowns;
    [Header("Dropdown")]
    public TMP_Dropdown genereDD;
    public TMP_Dropdown occupazioneDD;

    public TMP_Dropdown qualificaDD;
    public TMP_Dropdown titoloDD;

    public TMP_Dropdown esperienzaDD;
    public TMP_Dropdown linguaDD;
    public TMP_Dropdown patenteDD;

    [Header("GUI e altro")]
    public GameObject rigaQualifica;
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

    // FUNZIONANTE: TESTATO SOLO SU GENERE
    public void CheckAllInputs()
    {
        InputPanel.AcceptInputFields(inputFields);

        genere = InputPanel.AcceptDropdown<CVEntry.Genere>(genereDD);
        occupazione = InputPanel.AcceptDropdown<CVEntry.Occupazione>(occupazioneDD);
        qualifica = InputPanel.AcceptDropdown<Istruzione.Qualifica>(qualificaDD);
        esperienza = InputPanel.AcceptDropdown<CVEntry.Esperienza>(esperienzaDD);
        lingua = InputPanel.AcceptDropdown<CVEntry.Lingua>(linguaDD);
        patente = InputPanel.AcceptDropdown<CVEntry.Patente>(patenteDD);
    }

    // Crea un CV a partire dai campi inseriti. Controlli su file esistenti e campi vuoti prima del salvataggio.
    public void Submit()
    {
        //InputPanel.AcceptInputFields(inputFields);

        CheckAllInputs();
        
        Istruzione.Titolo titolo;


        if (titoloDD && qualifica != Istruzione.Qualifica.Medie)
        {
            titolo = InputPanel.AcceptDropdown<Istruzione.Titolo>(titoloDD);
        }
        else titolo = Istruzione.Titolo.Nessuno;

        if (InputPanel.fieldsClear && InputPanel.dropdownsClear)
        {
            Istruzione istruzione = new(qualifica, titolo);

            CV = new(inputFields[0].text, inputFields[1].text, occupazione, genere, istruzione, esperienza, lingua, patente);

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

    public void ShowTitoloDropdown()
    {
        Istruzione.Qualifica qualifica = InputPanel.AcceptDropdown<Istruzione.Qualifica>(qualificaDD);

        titoloDD = rigaQualifica.GetComponentInChildren<TMP_Dropdown>();

        // Istruzione istruzione = new(qualifica);
        //Debug.Log(qualifica.Qualifica);

        string qualificaText = qualifica.ToString();
        if (qualificaText.Equals("Medie"))
        {
            // Debug.Log("medie");
            titoloDD.ClearOptions();
            rigaQualifica.SetActive(false);
        }
        else //if (qualificaText.Equals("Superiori"))
        {
            // Debug.Log("SUPERIORI");
            rigaQualifica.SetActive(true);
            UpdateTitoloDropdownValues(qualificaText);
        }
    }

    public void UpdateTitoloDropdownValues(string qualifica)
    {
        titoloDD.ClearOptions();
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
        else if (qualifica.Equals("Laurea"))
        {
            options = new()
            {
                "Scientifica",
                "Umanistica",
                "Economica",
                "Sociale",
                "Tecnologica"
            };
        }

        titoloDD.AddOptions(options);
    }

    // Carica il CV scelto e ne inserisce i dati nei campi di input per la modifica.
    public void LoadCurrent()
    {
        CVEntry currentCV = CVManager.currentCV;
        Debug.Log("MODALITA' MODIFICA - " + currentCV.name + " " + currentCV.surname);


        inputFields[0].text = currentCV.name;
        inputFields[1].text = currentCV.surname;

        // Viene sommato 1 perch� il valore 0 dei dropdown � la frase "Inserire ..." (non compatibile con enum)

        genereDD.value = (int)currentCV.genere + 1;
        occupazioneDD.value = (int)currentCV.occupazione + 1;
        qualificaDD.value = (int)currentCV.istruzione.qualifica + 1;

        // NOTA: Il campo titoloDD attualmente non pu� essere recuperato perch� la dimensione / posizione delle sue opzioni � variabile 

        // Campi bonus facoltativi: Non hanno campo che obbliga a inserire un valore nel dropdown, quindi non � necessario incremento
        esperienzaDD.value = (int)currentCV.esperienza;
        linguaDD.value = (int)currentCV.secondaLingua;
        patenteDD.value = (int)currentCV.patente;

        // Probabilmente non � possibile ricaricare il titolo di Istruzione perch� le opzioni hanno indici diversi
    }

    // Se la schermata viene disattivata, si esce dalla modalit� modifica
    public void OnDisable()
    {
        CVManager.editCurrent = false;
    }
}
