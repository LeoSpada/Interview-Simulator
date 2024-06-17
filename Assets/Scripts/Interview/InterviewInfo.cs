using TMPro;
using UnityEngine;

// Permette di stampare informazioni utili sul colloquio attuale
public class InterviewInfo : MonoBehaviour
{
    public TextMeshProUGUI pointsField;
    public TextMeshProUGUI averageField;
    public TextMeshProUGUI questionField;   

    public QuestionPanel questionPanel;

    public void ReloadInfo()
    {
       if(questionField) questionField.text = $"Domande risposte: {questionPanel.answered}/{questionPanel.questionNumber} [{questionPanel.currentJob}]";
       if(pointsField) pointsField.text = $"Punteggio {questionPanel.points}";
       if(averageField) averageField.text = $"Media: {questionPanel.average}";
    }
}