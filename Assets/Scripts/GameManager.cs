using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void LoadScene(string nomeScena)
    {
        SceneManager.LoadScene(nomeScena);
    }

    public void QuitGame()
    {
        Debug.Log("Closing game...");
        Application.Quit();
    }
}
