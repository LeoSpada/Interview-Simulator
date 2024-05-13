using TMPro;
using UnityEngine;

public class CVEntryPanel : MonoBehaviour
{

    public TMP_InputField[] InputFields;

    private bool allClear = true;


    public void Submit()
    {
        foreach (TMP_InputField inputField in InputFields)
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

        if (allClear)
        {
            CVEntry CV = new(InputFields[0].text, InputFields[1].text, InputFields[2].text);
            CVManager.AddCVEntry(CV);
        }


        //if (AcceptInput(nameInputField) && AcceptInput(surnameInputField) && AcceptInput(jobInputField))
        //{
        //    CVEntry CV = new(nameInputField.text, surnameInputField.text, jobInputField.text);
        //    CVManager.AddCVEntry(CV);
        //}
        //else Debug.Log("INSERIRE TUTTI I CAMPI");
    }

    public bool AcceptInput(TMP_InputField inputField)
    {
        if (string.IsNullOrWhiteSpace(inputField.text)) return false;
        else return true;

        // nameCompletionSource.SetResult(inputField.text);
    }

}
