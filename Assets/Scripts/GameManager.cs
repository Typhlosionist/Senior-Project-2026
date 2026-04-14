using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float time;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // survives scene changes
        }
        else
        {
            Destroy(gameObject); // prevents duplicates
        }
    }

    [ContextMenu("LoadWinScreen")]
    public void LoadWinScreen()
    {
        SceneManager.LoadScene("WinScreen");
    }
}
