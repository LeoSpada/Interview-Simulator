using TMPro;
using UnityEngine;

public class CVUIListEntry : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI surnameText;
    private CVEntry CVEntry;

    // public Canvas entryPanel;

    public void Setup(CVEntry entry)
    {
        CVEntry = entry;
        nameText.text = entry.name;
        surnameText.text = entry.surname;
    }

    // Carica il CVEntry a partire dai campi nome e cognome già riempiti da CVLoadPanel 
    public void SearchSetup()
    {
        CVEntry = CVManager.GetCVEntry(nameText.text, surnameText.text);
    }

    public void LoadEntryScene()
    {
       // CVManager.DebugCV(CVEntry);
        CVManager.currentCV = CVEntry;
        GameManager.instance.LoadScene("Scena_Titolo");
    }

    public void EditEntry()
    {
        CVManager.editCurrent = true;
        CVManager.currentCV = CVEntry;
        GameManager.instance.LoadScene("Scena_Curriculum");
    }

    public void DeleteEntry()
    {
        CVManager.RemoveCVEntry(CVEntry);
    }
}
