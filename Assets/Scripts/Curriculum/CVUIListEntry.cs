using TMPro;
using UnityEngine;

public class CVUIListEntry : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI surnameText;
    private CVEntry CVEntry;
        
    public void Setup(CVEntry entry)
    {
        CVEntry = entry;
        nameText.text = entry.name;
        surnameText.text = entry.surname;
    }

    public void OnEntrySelect()
    {
        // TEST
        // QUI VA INSERITA FUNZIONE CHE AVVIA SCENA DEL GIOCO CORRELATA AL CVENTRY SCELTO
        CVManager.DebugCV(CVEntry);
        CVManager.currentCV = CVEntry;
        GameManager.instance.LoadScene("Scena_Titolo");
    }
}
