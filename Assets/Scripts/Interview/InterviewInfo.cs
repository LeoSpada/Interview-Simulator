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
        questionField.text = $"Domande risposte: {questionPanel.answered}/{questionPanel.questionNumber} [{questionPanel.currentJob}]";
        pointsField.text = $"Punteggio {questionPanel.points}";
        averageField.text = $"Media: {questionPanel.average}";
    }
}