using TMPro;
using UnityEngine;

public class CVUIListEntry : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI surnameText;
    private CVEntry CVEntry;

    public GameObject deleteConfirmPanel;

    // public Canvas entryPanel;

    public void Setup(CVEntry entry)
    {
        CVEntry = entry;
        nameText.text = entry.name;
        surnameText.text = entry.surname;
    }

    // Carica il CVEntry a partire dai campi nome e cognome già riempiti da CVLoadPanel 
    // RICERCA è una funzione non più implementata: forse rimuovere
    public void SearchSetup()
    {
        CVEntry = CVManager.GetCVEntry(nameText.text, surnameText.text);
    }

    public void LoadEntryScene()
    {
        // CVManager.DebugCV(CVEntry);
        Debug.Log("load check 0");
        CVManager.currentCV = CVEntry;
        Debug.Log("load check 1 (" + CVManager.currentCV.surname + ")");
        // GameManager.instance.LoadScene("Scena_Titolo");
        GameManager.instance.LoadScene("Scena_Colloquio");
    }

    public void EditEntry()
    {
        Debug.Log("L'edit funziona (check 0)");
        CVManager.editCurrent = true;
        CVManager.currentCV = CVEntry;
        Debug.Log("edit check 2");
        GameManager.instance.LoadScene("Scena_Curriculum");
    }

    public void DeleteEntry()
    {
        Debug.Log("Delete check 0");
        CVManager.RemoveCVEntry(CVEntry);
        Debug.Log("Delete check 1");
        GameManager.instance.LoadScene("Scena_Lista_CV");
        Debug.Log("Delete check 2");
    }

    public void DeleteButton()
    {
        deleteConfirmPanel.SetActive(true);
    }
}
