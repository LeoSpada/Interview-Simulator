using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        ShowScore();
    }

    public void ShowScore()
    {
        CVEntry cv = CVManager.currentCV;
        if (PlayerPrefs.HasKey("score") && cv != null)
        {
            float score = PlayerPrefs.GetFloat("score");
            float average = PlayerPrefs.GetFloat("average");


            string message = $"Candidato/a {cv.name} {cv.surname}\n\nIn base al colloquio da Lei sostenuto/a, la informiamo che la sua candidatura è stata\n\n";
            if (average >= 0.75f) message += $"ACCETTATA!"/*\nPunteggio: {score} Media: {average}"*/; //Non serve mostrare il punetggio e la media
            else message += $"RESPINTA..."/*\nPunteggio: {score} Media: {average}"*/; //Non serve mostrare il punteggio e la media

            scoreText.text = message;
        }
        else scoreText.text = "Nessun punteggio trovato";
    }
}
