using UnityEngine;
using TMPro;

public class Imposta_Testo : MonoBehaviour
{
    public TextMeshProUGUI testoProva;


    void Start()
    {
        CVEntry loadedCV = CVManager.currentCV;
        if (loadedCV != null)
        {
            string gender = loadedCV.genere.ToString();
            if(gender.Equals("M")) testoProva.text = "Buongiorno Sig. " + loadedCV.surname + " " + loadedCV.name + "!";
            else if (gender.Equals("F")) testoProva.text = "Buongiorno Sig.ra " + loadedCV.surname + " " + loadedCV.name + "!";
            else testoProva.text = "Buongiorno " + loadedCV.surname + " " + loadedCV.name + "!";
        }
            
        else testoProva.text = "Caricare un profilo";
    }
}
