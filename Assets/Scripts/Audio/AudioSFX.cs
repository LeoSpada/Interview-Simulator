using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerFinale>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioManager.PlaySFX(audioManager.pulsante_seleziona);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
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
