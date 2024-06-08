using TMPro;
using UnityEngine;

public class CVInfo : MonoBehaviour
{
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI surnameField;
    public TextMeshProUGUI jobField;
    public TextMeshProUGUI genderField;

    // Update is called once per frame
    void Update()
    {
        if (CVManager.currentCV != null)
        {
            nameField.text = CVManager.currentCV.name;
            surnameField.text = CVManager.currentCV.surname;
            jobField.text = CVManager.currentCV.job.ToString();
            genderField.text = CVManager.currentCV.gender.ToString();
        }
    }
}
