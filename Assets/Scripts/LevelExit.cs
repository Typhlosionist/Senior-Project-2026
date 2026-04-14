using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{

    public DungeonManager dungeonManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
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
