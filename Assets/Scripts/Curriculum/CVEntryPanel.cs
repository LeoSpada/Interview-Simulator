using TMPro;
using UnityEngine;

public class CVEntryPanel : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_InputField surnameInputField;

    public void Submit()
    {
        if (AcceptInput(nameInputField) && AcceptInput(surnameInputField))
            CVManager.AddCVEntry(nameInputField.text, surnameInputField.text);
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
