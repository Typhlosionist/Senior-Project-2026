using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public const int hrsinday = 24;
    public float dayDuration = 30f; 
    public float sunsetHr = 18;

    float totalTime = 0;
    float currTime = 0;

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;
        currTime = totalTime % dayDuration;
    }

    public float GetHour()
    {
        return currTime * hrsinday / dayDuration;
    }
    
}   

