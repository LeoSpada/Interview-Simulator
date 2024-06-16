using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private string interviewScene = "Scena_Colloquio";
    private string cvListScene = "Scena_Lista_CV";

    private void Awake()
    {
        instance = this;
    }

    public void LoadScene(string nomeScena)
    {
        SceneManager.LoadScene(nomeScena);
    }

    public void StartGame()
    {
        if(CVManager.currentCV != null)
        {
            LoadScene(interviewScene);
        }
        else LoadScene(cvListScene);
    } 

    public void QuitGame()
    {
        Debug.Log("Closing game...");
        Application.Quit();
    }
}
