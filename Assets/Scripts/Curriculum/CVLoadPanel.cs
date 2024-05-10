using TMPro;
using UnityEngine;

public class CVLoadPanel : MonoBehaviour
{
    // FARE ARRAY DI TEXTMESHPRO??

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI surnameText;
    public TextMeshProUGUI jobText;

    public TMP_InputField nameInputField;
    public TMP_InputField surnameInputField;


    public void LoadCV()
    {
        CVEntry cv = CVManager.GetCVEntry(nameInputField.text, surnameInputField.text);

        if (cv == null)
        {
            nameText.text = "Nessun cv trovato";
            surnameText.text = "Nessun cv trovato";
            jobText.text = "Nessun cv trovato";
            return;
        }
        nameText.text = cv.name;
        surnameText.text = cv.surname;
        jobText.text = cv.job;
    }

    public void ShowAll()
    {
        CVManager.GetAllCV();
    }
}