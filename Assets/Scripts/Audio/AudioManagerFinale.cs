using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerFinale : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip -----------")]
    public AudioClip background_1;
    public AudioClip background_2;
    public AudioClip pulsante_seleziona;
    public AudioClip pulsante_conferma;
    public AudioClip pulsante_annulla;

    public static AudioManagerFinale instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*
     * =========================================================================================
     * Metodo che fa partire la musica all'inizio del gioco.
     * =========================================================================================
     */
    public void Start()
    {
        SetMusicForCurrentScene();
    }

    /*
     * =========================================================================================
     * Metodo che fa partire la musica quando viene caricata una scena.
     * =========================================================================================
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetMusicForCurrentScene();
    }

    /*
     * =========================================================================================
     * Metodo per far suonare gli effetti sonori.
     * =========================================================================================
     */
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    /*
     * =========================================================================================
     * Metodi che gestiscono le scene.
     * =========================================================================================
     */
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /*
     * =========================================================================================
     * Metodo che gestisce la musica di background a seconda della scena
     * su cui ci si trova il giocatore attualmente.
     * =========================================================================================
     */
    private void SetMusicForCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        AudioClip newClip = null;

        // Controlla il nome della scena e imposta la traccia audio di conseguenza
        if (sceneName == "Scena_Colloquio")
        {
            newClip = background_2;
        }
        else
        {
            newClip = background_1;
        }

        // Cambia la traccia solo se è diversa da quella attuale
        if (musicSource.clip != newClip)
        {
            StartCoroutine(FadeOutAndChangeMusic(newClip));
        }
    }

    /*
     * =========================================================================================
     * Metodo che gestisce il tempo di fade-in e fade-out della musica.
     * Ho scritto questo metodo per evitare del lag tra il cambio delle scene.
     * =========================================================================================
     */
    private IEnumerator FadeOutAndChangeMusic(AudioClip newClip)
    {
        float fadeOutTime = 0.25f;
        float fadeInTime = 0.25f;
        float startVolume = musicSource.volume;

        // Fade out
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeOutTime);
            yield return null;
        }

        musicSource.volume = 0;
        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();

        // Fade in
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, startVolume, t / fadeInTime);
            yield return null;
        }

        musicSource.volume = startVolume;
    }
}