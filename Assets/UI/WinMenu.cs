using UnityEngine;
using UnityEngine.SceneManagement;


public class WinMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Replay()
    {
        Debug.Log("Replay");
        GameStateManager.Instance.currentLevel = 0;
        Shooting.spreadOne = false;
        Shooting.spreadTwo = false;
        Bullter.flame = false;
        Bullter.gust = false;
        Bullter.freeze = false;
        Bullter.wildfire = false;
        Bullter.ice = false;
        SceneManager.LoadScene("SampleScene");
    }
    
    public void MainMenu()
    {
        Debug.Log("MainMenu");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
