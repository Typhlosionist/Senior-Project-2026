using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{

    public DungeonManager dungeonManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameStateManager.Instance.CompleteLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
        }
    }
}
