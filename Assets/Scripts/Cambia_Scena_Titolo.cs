using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cambia_Scena_Titolo : MonoBehaviour
{
    public void LoadScene(string nomeScena)
    {
        SceneManager.LoadScene(nomeScena);
    }
}