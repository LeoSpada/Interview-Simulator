using UnityEngine;
using TMPro;

public class Imposta_Testo : MonoBehaviour
{
    public TextMeshProUGUI testoProva;


    void Start()
    {
        CVEntry loadedCV = CVManager.currentCV;
        if (loadedCV != null)
            testoProva.text = "Buongiorno Sig." + loadedCV.surname +" "+loadedCV.name+ "!";
        else testoProva.text = "Caricare un profilo";
    }   
}
