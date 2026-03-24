using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public const int hrsinday = 24;
    public float dayDuration = 30f; //for testing
    public float nightDuration = .4f;
    public float sunriseHr = 6;

    float totalTime = 0;
    float currTime = 0;

    // Update is called once per frame
    void Update()
    {
        totalTime = Time.deltaTime;
        currTime = totalTime % dayDuration;
    }

    public float GetHour()
    {
        return currTime * hrsinday / dayDuration;
    }

    public float GetSunsetHour()
    {
        return (sunriseHr + (1 - nightDuration) * hrsinday) % hrsinday;
    }
    
}   

