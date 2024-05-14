using System;
using TMPro;
using UnityEngine;

public class CVEntryPanel : MonoBehaviour
{
    // Contiene i vari inputField
    public TMP_InputField[] inputFields;

    // Fare array di Dropdown?
    public TMP_Dropdown genderDropdown;

    private bool allClear = true;


    public void Submit()
    {
        foreach (TMP_InputField inputField in inputFields)
        {
            if (!AcceptInput(inputField))
            {
                Debug.Log("Campo inesistente");

                inputField.image.color = Color.red;

                allClear = false;

                // Rimettere break riduce i controlli ma non fa colorare di rosso tutti i campi (solo il primo non valido)
                // break;
            }

            else inputField.image.color = Color.white;
        }

        // Debug.Log(genderDropdown.value);

        // CONVERTIRE DA RIGA 38 a RIGA 55 per rendere generico

        // Ottiene il valore del testo del dropdown
        string genderText = genderDropdown.options[genderDropdown.value].text;
        
        // Debug.Log(genderText);

        // Prova ad ottenere il valore enum corretto corrispondente alla stringa
        CVEntry.Genere genere = new();
        try
        {
            genere = (CVEntry.Genere)Enum.Parse(typeof(CVEntry.Genere), genderText);
        }
        catch (ArgumentException)
        {
            allClear = false;
            genderDropdown.image.color = Color.red;
            Debug.Log("Il genere del dropdown non è consentito nell'Enum.\nRicontrollare codice.");
        }      

        if (allClear)
        {
            CVEntry CV = new(inputFields[0].text, inputFields[1].text, inputFields[2].text, genere);
            CVManager.AddCVEntry(CV);
        }
    }

    public bool AcceptInput(TMP_InputField inputField)
    {
        if (string.IsNullOrWhiteSpace(inputField.text)) return false;
        else return true;

        // nameCompletionSource.SetResult(inputField.text);
    }

}
