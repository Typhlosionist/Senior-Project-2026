using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void NewGame()
    {
        Debug.Log("New Game");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1)
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
