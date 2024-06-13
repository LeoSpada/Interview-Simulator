using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceManager : MonoBehaviour
{
    public Material Materiale_Bocca;
    public Material Materiale_Occhi;
    public Texture[] Texture_Bocca;
    public Texture[] Texture_Occhi;
    private int Indice_Texture_Attuale_Bocca = 0;
    private int Indice_Texture_Attuale_Occhi = 0;
    private bool isKeyBoccaPressed = false;
    private bool isKeyOcchiPressed = false;

    public QuestionPanel questionPanel;

    // Update is called once per frame
    void Update()
    {
        if (questionPanel.average >= 0.75)
        {
            Materiale_Occhi.mainTexture = Texture_Occhi[0];
            Materiale_Bocca.mainTexture = Texture_Bocca[2];
        }
        else
        {
            Materiale_Occhi.mainTexture = Texture_Occhi[1];
            Materiale_Bocca.mainTexture = Texture_Bocca[0];
        }

        //Gestione tasto per cambiare la bocca
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    if (!isKeyBoccaPressed)
        //    {
        //        Cambia_Texture_Bocca();
        //        isKeyBoccaPressed = true;
        //    }
        //}

        //if(Input.GetKeyUp(KeyCode.UpArrow))
        //{
        //    isKeyBoccaPressed = false;
        //}

        ////Gestione tasto per cambiare gli occhi
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    if (!isKeyOcchiPressed)
        //    {
        //        Cambia_Texture_Occhi();
        //        isKeyOcchiPressed = true;
        //    }
        //}

        //if (Input.GetKeyUp(KeyCode.DownArrow))
        //{
        //    isKeyOcchiPressed = false;
        //}
    }

    void Cambia_Texture_Bocca()
    {
        Indice_Texture_Attuale_Bocca++;
        if (Indice_Texture_Attuale_Bocca >= Texture_Bocca.Length)
        {
            Indice_Texture_Attuale_Bocca = 0;
        }
        Materiale_Bocca.mainTexture = Texture_Bocca[Indice_Texture_Attuale_Bocca];
    }

    void Cambia_Texture_Occhi()
    {
        Indice_Texture_Attuale_Occhi++;
        if (Indice_Texture_Attuale_Occhi >= Texture_Occhi.Length)
        {
            Indice_Texture_Attuale_Occhi = 0;
        }
        Materiale_Occhi.mainTexture = Texture_Occhi[Indice_Texture_Attuale_Occhi];
    }
}