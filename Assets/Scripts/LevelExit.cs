using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class LevelExit : MonoBehaviour
{

    public DungeonManager dungeonManager;
    private bool exitOpened = false;
    private Light2D exitLight;

    void Awake()
    {
        exitLight = GetComponent<Light2D>();
        if (exitLight != null)
        {
            exitLight.enabled = false;
        }
    }
    
    public void OpenExit()
    {
        exitOpened = true;
        if (exitLight != null)
        {
            exitLight.enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!exitOpened) return;
        if (other.CompareTag("Player"))
        {
            if (GameStateManager.Instance.currentLevel >= 2)
            {
                SceneManager.LoadScene("WinScreen");
                return;
            }
            
            GameStateManager.Instance.CompleteLevel();
            SceneManager.LoadScene("SampleScene");
        }
    }
}
