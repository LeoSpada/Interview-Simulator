using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Prendi_Nome : MonoBehaviour
{
    [Header("Il valore che otteniamo dall'input field")]
    [SerializeField] private string inputText;

    [Header("Mostra la reazione al giocatore")]
    [SerializeField] private GameObject reactionGroup;
    [SerializeField] private TMP_Text reactionTextBox;

    public void GrabFromInputField (string input)
    {
        inputText = input;
        DisplayReactionToInput();
    }

    public void DisplayReactionToInput ()
    {
        reactionTextBox.text = "Buongiorno sig. " + inputText;
        reactionGroup.SetActive(true);
    }
}
