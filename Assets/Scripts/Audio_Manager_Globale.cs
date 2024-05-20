using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager_Globale : MonoBehaviour
{
    public static Audio_Manager_Globale Instance { get; private set; }
    private AudioSource audioSource;

    private void Awake()
    {
        //Funzione che permette ai suoni riprodotti di persistere, nonostante i cambiamenti di scena
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip) 
    {
        audioSource.PlayOneShot(clip);
    }
}
