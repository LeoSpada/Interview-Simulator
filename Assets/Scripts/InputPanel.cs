using System;
using TMPro;
using UnityEngine;

public static class InputPanel
{

    public static bool allClear = true;

    // Controlla che l'input field contenga dati utilizzabili
    public static bool AcceptInput(TMP_InputField inputField)
    {
        if (string.IsNullOrWhiteSpace(inputField.text)) return false;
        else return true;
    }

    // Controlla che il valore del dropdown corrisponda ad un valore contenuto nell'enum
    public static T AcceptDropdown<T>(TMP_Dropdown dropdown)
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
            dropdown.image.color = Color.red;
            Debug.Log("Il valore del dropdown non � consentito nell'Enum.\nRicontrollare codice.");
            allClear = false;
            return default;
        }

        dropdown.image.color = Color.white;

        // Debug.Log("Enumobj: " + enumObj);
        return enumObj;
    }
}
