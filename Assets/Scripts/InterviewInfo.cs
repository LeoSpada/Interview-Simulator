using TMPro;
using UnityEngine;

public class InterviewInfo : MonoBehaviour
{
    public TextMeshProUGUI pointsField;
    public TextMeshProUGUI averageField;
    public TextMeshProUGUI questionField;   

    public QuestionPanel questionPanel;

    public void ReloadInfo()
    {
        questionField.text = $"Domanda {questionPanel.answered}/{questionPanel.questionNumber}";
        pointsField.text = $"Punteggio {questionPanel.points}";
        averageField.text = $"Media: {questionPanel.average}";
    }
}