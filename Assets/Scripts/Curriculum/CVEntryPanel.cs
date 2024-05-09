using TMPro;
using UnityEngine;

public class CVEntryPanel : MonoBehaviour
{ 
    public TMP_InputField nameInputField;
    public TMP_InputField surnameInputField;

    public void Submit()
    {
        Debug.Log("INVIO NUOVO CV");
        CVManager.AddCVEntry(nameInputField.text, surnameInputField.text);
    }

    // INSERIRE CONTROLLO CAMPI VUOTI, NON COMPATIBILI...
    
}
