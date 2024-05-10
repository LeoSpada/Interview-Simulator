using TMPro;
using UnityEngine;

public class CVEntryPanel : MonoBehaviour
{
    // FARE ARRAY DI INPUTFIELD?

    public TMP_InputField nameInputField;
    public TMP_InputField surnameInputField;
    public TMP_InputField jobInputField;


    public void Submit()
    {
        if (AcceptInput(nameInputField) && AcceptInput(surnameInputField) && AcceptInput(jobInputField))
        {
            CVEntry CV = new(nameInputField.text, surnameInputField.text, jobInputField.text);
            CVManager.AddCVEntry(CV);
        }
        else Debug.Log("INSERIRE TUTTI I CAMPI");
    }

    // INSERIRE CONTROLLO CAMPI VUOTI, NON COMPATIBILI...

    public bool AcceptInput(TMP_InputField inputField)
    {
        if (string.IsNullOrWhiteSpace(inputField.text)) return false;
        else return true;

        // nameCompletionSource.SetResult(inputField.text);
    }

}
