using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Audio_Pulsanti : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip suono_click;
    public AudioClip suono_seleziona;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Suona quando il pulsante viene cliccato
        Audio_Manager_Globale.Instance.PlaySound(suono_click);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Suona quando il pulsante viene selezionato
        Audio_Manager_Globale.Instance.PlaySound(suono_seleziona);
    }
}
