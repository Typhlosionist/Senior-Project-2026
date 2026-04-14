using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Retry()
    {
        Debug.Log("Retry");
        GameStateManager.Instance.currentLevel = 0;
        SceneManager.LoadScene("SampleScene");
    }

    public void MainMenu()
    {
        Debug.Log("back to main menu");
        SceneManager.LoadScene("MainMenu");
    }
}
