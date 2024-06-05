using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InterviewManager;

public class QuestionPanel : MonoBehaviour
{

    public TextMeshProUGUI questionText;
    public Button[] ansButtons;
    private Question question;

    // public Canvas entryPanel;

    // Rimettere question come argomento funzione
    public void Setup()
    {
        // Domanda di prova
        Question q = GetQuestion("Medico", 1);

        question = q;
        questionText.text = q.question;
        
        for (int i = 0; i < ansButtons.Length; i++)
        {
            // Test indice corretto: Trovare come rendere la corretta quella che dà punti
            if (i == q.correctIndex) ansButtons[i].name += " [CORRETTA]";

            TextMeshProUGUI buttonText = ansButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = q.answers[i];
        }       
    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
