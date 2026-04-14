using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Retry()
    {
        Debug.Log("Retry");
        SceneManager.LoadScene("SampleScene");
    }

    public void MainMenu()
    {
        Debug.Log("back to main menu");
        SceneManager.LoadScene("MainMenu");
    }
}
