using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioSFX : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    AudioManagerFinale audioManager;
    /*
     * Con il valore_audio si decide quale sfx far suonare a seconda del valore.
     * I suoni associati al valore_audio sono rispettivamente:
     *      * 1 = pulsante di conferma
     *      * 2 = pulsante di annullamento
     */
    [Header("---------- Value ----------")]
    public int valore_audio = 1;

    [Header("Debug")]
    public bool hasAudio = false;

    public Button button;

    private void Awake()
    {
      //  Debug.Log("AUDIO di "+gameObject.name);      
        
        
        if (GameObject.FindGameObjectWithTag("Audio").TryGetComponent<AudioManagerFinale>(out audioManager))
        {
        //    Debug.Log("L'audio manager e' " + audioManager);
            hasAudio = true;
        }
        else Debug.Log("errore audio");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hasAudio)
            audioManager.PlaySFX(audioManager.pulsante_seleziona);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (hasAudio)
            switch (valore_audio)
            {
                case 1:
                    audioManager.PlaySFX(audioManager.pulsante_conferma);
                    break;

                case 2:
                    audioManager.PlaySFX(audioManager.pulsante_annulla);
                    break;
            }       
    }
}
