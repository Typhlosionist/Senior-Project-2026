using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Retry()
    {
        Debug.Log("Retry");
        GameStateManager.Instance.currentLevel = 0;
        Shooting.spreadOne = false;
        Shooting.spreadTwo = false;
        Bullter.flame = false;
        Bullter.gust = false;
        Bullter.freeze = false;
        Bullter.wildfire = false;
        Bullter.ice = false;
        Shooting.shootCooldown = .8f;
        Shooting.bulletSpeed = 10f;
        SceneManager.LoadScene("SampleScene");
    }

    public void MainMenu()
    {
        Debug.Log("back to main menu");
        SceneManager.LoadScene("MainMenu");
    }
}
