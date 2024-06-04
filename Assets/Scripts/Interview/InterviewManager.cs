using TMPro;
using UnityEngine;

public class InterviewManager : MonoBehaviour
{
    private CVEntry loadedCV;

    public TextMeshProUGUI text;

    public int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        loadedCV = CVManager.currentCV;

        if (loadedCV == null) Debug.Log("Nessun CV caricato. ERRORE");

       else if (loadedCV.job == 0) Debug.Log("Sviluppatore");
    }

    // Update is called once per frame
    void Update()
    {
        text.text = loadedCV.name;
    }
}
