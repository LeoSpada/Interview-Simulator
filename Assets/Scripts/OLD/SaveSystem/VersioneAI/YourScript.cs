using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YourScript : MonoBehaviour
{
    public TextDataHandler textDataHandler;
    public TMP_InputField inputField;

    private void Start()
    {
        // Carica il testo salvato all'avvio della scena
        textDataHandler.LoadText();
        // Mostra il testo nell'InputField
        inputField.text = textDataHandler.textData.savedText;
    }

    public void SaveText()
    {
        // Salva il testo inserito
        textDataHandler.textData.savedText = inputField.text;
        // Chiama il metodo per salvare i dati
        textDataHandler.SaveText();
    }
}