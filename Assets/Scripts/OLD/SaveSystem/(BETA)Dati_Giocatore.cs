using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Dati_Giocatore : MonoBehaviour
{
    public Scrittura_Dati Nome;

    void Start()
    {
        Nome.inputNome.text = PlayerPrefs.GetString("TestoSalvato", "");
        Nome.testoSalvato.text = PlayerPrefs.GetString("TestoSalvato", "");
    }

    private void OnEnable()
    {
        Nome.inputNome.onEndEdit.AddListener(TestoSalvato);
    }
    
    private void OnDisable()
    {
        Nome.inputNome.onEndEdit.RemoveListener(TestoSalvato);
    }

    public void TestoSalvato(string Testo)
    {
        PlayerPrefs.SetString("TestoSalvato", Testo);
        PlayerPrefs.Save();
        Nome.testoSalvato.text = Testo;
    }

    /*public void TestoDaCaricare()
    {
        Nome.testoSalvato.text = myText.text;
        SceneManager.LoadScene("Scena_Titolo");
    }*/

}
