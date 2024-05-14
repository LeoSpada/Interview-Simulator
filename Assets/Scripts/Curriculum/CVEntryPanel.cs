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

        CVEntry.Genere genere = AcceptDropdown<CVEntry.Genere>(genderDropdown);

        if (allClear)
        {
            CVEntry CV = new(inputFields[0].text, inputFields[1].text, inputFields[2].text, genere);
            CVManager.AddCVEntry(CV);
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
}
