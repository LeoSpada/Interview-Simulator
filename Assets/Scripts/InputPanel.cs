using System;
using TMPro;
using UnityEngine;

// Gestisce i controlli su inputField e Dropdown per segnalare valori non ammessi
public static class InputPanel
{
    // Indicano che tutti i dati sono ammessi
    public static bool fieldsClear = true;
    public static bool dropdownsClear = true;

    // Colore della casella con errore
    public static Color errorColor = new(0.5f, 0, 0, 0.2f);

    // Valore che il codice rimuove / ignora in alcuni contesti.
    // ATTENZIONE: deve essere un numero!
    public static string disabledText = "000";

    // Controlla che l'input field contenga dati utilizzabili
    public static bool CheckInputField(TMP_InputField inputField)
    {
        if (string.IsNullOrWhiteSpace(inputField.text)) return false;
        else return true;
    }

    // Controlla che l'input field contenga dati utilizzabili e agisce di conseguenza con colori e/o interruzioni
    public static void AcceptInputField(TMP_InputField inputField, bool canColor = true, bool canBreak = true, bool log = false)
    {
        if (!inputField.isActiveAndEnabled)
        {
            if (log) Debug.Log(inputField + " ignorato");
            return;
        }
        else if (!InputPanel.CheckInputField(inputField))
        {
            if (canColor) inputField.image.color = errorColor;
            if (canBreak) fieldsClear = false;
        }
        else if (canColor) inputField.image.color = Color.white;
    }

    // Variante di AcceptInputField per array
    public static void AcceptInputFields(TMP_InputField[] inputFields, bool canColor = true, bool canBreak = true)
    {
        foreach (TMP_InputField inputField in inputFields)
        {
            AcceptInputField(inputField, canColor, canBreak);
        }
    }

    // Rende attivo l'inputField
    public static void EnableInputField(TMP_InputField field)
    {
        field.ActivateInputField();
        field.enabled = true;
        field.gameObject.SetActive(true);
    }

    // Disattiva l'inputField e lo nasconde
    public static void DisableInputField(TMP_InputField field)
    {
        // Debug.Log("Ignorata");
        field.text = disabledText;
        field.gameObject.SetActive(false);
    }

    // Controlla che il valore del dropdown corrisponda ad un valore contenuto nell'enum
    public static T AcceptDropdown<T>(TMP_Dropdown dropdown, bool canColor = true, bool canBreak = true, bool log = false)
    {
        // Ottiene il valore del testo del dropdown
        string dropdownText = dropdown.options[dropdown.value].text;

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
                dropdown.image.color = errorColor;
            }

            if (canBreak)
            {
                dropdownsClear = false;
                if (log) Debug.Log("Il valore del dropdown non è consentito nell'Enum.\nRicontrollare codice.");
            }
            return default;
        }

        if (canColor)
            dropdown.image.color = Color.white;

        return enumObj;
    }

    // Reimposta i flag
    public static void ClearAll()
    {
        fieldsClear = true;
        dropdownsClear = true;
    }
}
