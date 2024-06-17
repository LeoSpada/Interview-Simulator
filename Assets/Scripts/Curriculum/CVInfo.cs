using TMPro;
using UnityEngine;

// Mostra le informazioni del curriculum caricato
public class CVInfo : MonoBehaviour
{
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI surnameField;
    public TextMeshProUGUI jobField;
    public TextMeshProUGUI genderField;

    public void ReloadInfo()
    {
        if (CVManager.currentCV != null)
        {
            nameField.text = CVManager.currentCV.name;
            surnameField.text = CVManager.currentCV.surname;
            jobField.text = CVManager.currentCV.occupazione.ToString();
            genderField.text = CVManager.currentCV.genere.ToString();
        }
        else
        {
            nameField.text = "NO CV";
            surnameField.text = "NO CV";
            jobField.text = "NO CV";
            genderField.text = "NO CV";
        }
    }
}
