using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void NewGame()
    {
        Debug.Log("New Game");
        if (GameStateManager.Instance != null){
            GameStateManager.Instance.currentLevel = 0;
            //reset upgrades
            Shooting.spreadOne = false;
            Shooting.spreadTwo = false;
            Bullter.flame = false;
            Bullter.gust = false;
            Bullter.freeze = false;
            Bullter.wildfire = false;
            Bullter.ice = false;
            Shooting.shootCooldown = .8f;
            Shooting.bulletSpeed = 10f;
        }
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
