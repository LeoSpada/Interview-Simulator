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

    public GameObject buttons;


    public void LoadCV()
    {
        CVEntry cv = CVManager.GetCVEntry(nameInputField.text, surnameInputField.text);

        if (cv == null)
        {
            // Debug.Log("Nessun CV trovato");
            nameText.text = "Nessun cv trovato";
            surnameText.gameObject.SetActive(false);
            jobText.gameObject.SetActive(false);
            buttons.SetActive(false);
            return;
        }

        nameText.text = cv.name;
        surnameText.text = cv.surname;
        jobText.text = cv.occupazione.ToString();

        surnameText.gameObject.SetActive(true);
        jobText.gameObject.SetActive(true);
        buttons.SetActive(true);

        // CVManager.DebugCV(cv);
    }
}