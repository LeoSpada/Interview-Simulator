using System;
using TMPro;
using UnityEngine;

public static class InputPanel
{

    // public static bool allClear = true;
    public static bool fieldsClear = true;
    public static bool dropdownsClear = true;

    // Controlla che l'input field contenga dati utilizzabili
    public static bool AcceptInput(TMP_InputField inputField)
    {
        if (string.IsNullOrWhiteSpace(inputField.text)) return false;
        else return true;
    }

    // Controlla che il valore del dropdown corrisponda ad un valore contenuto nell'enum
    public static T AcceptDropdown<T>(TMP_Dropdown dropdown, bool canColor = true, bool canBreak = true)
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
            if (canColor)
            {
                dropdown.image.color = Color.red;
                     
            }

            if (canBreak)
            {
                dropdownsClear = false;
                Debug.Log("Il valore del dropdown non è consentito nell'Enum.\nRicontrollare codice.");
            }            
            return default;
        }

        if(canColor)
        dropdown.image.color = Color.white;

        // Debug.Log("Enumobj: " + enumObj);
        return enumObj;
    }
}
