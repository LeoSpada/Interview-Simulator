using TMPro;
using UnityEngine;

public class CVUIListEntry : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI surnameText;

    
    
    public void Setup(CVEntry entry)
    {
        nameText.text = entry.name;
        surnameText.text = entry.surname;
    }
}
