using TMPro;
using UnityEngine;

// Gestisce le funzioni del singolo elemento della lista dei CV.
public class CVUIListEntry : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI surnameText;

    private CVEntry CVEntry;
    public GameObject deleteConfirmPanel;

    public void Setup(CVEntry entry)
    {
        CVEntry = entry;
        nameText.text = entry.name;
        surnameText.text = entry.surname;
    }

    public void LoadEntryScene()
    {
        CVManager.currentCV = CVEntry;
        GameManager.instance.LoadScene("Scena_Colloquio");
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
        GameManager.instance.LoadScene("Scena_Lista_CV");
    }

    public void DeleteButton()
    {
        deleteConfirmPanel.SetActive(true);
    }
}
