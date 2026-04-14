using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    
    public TMP_Text timerText;
    public float timeInSeconds;

    void Start()
    {
        timeInSeconds = GameManager.Instance.time;

        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
    


}
