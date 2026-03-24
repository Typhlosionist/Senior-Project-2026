using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    TimeManager tm;
    public RectTransform hand;
    const float hrstodegrees = 180/24;
    void Start()
    {
        tm = FindObjectOfType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        hand.rotation = Quaternion.Euler(0,0, 90- hrstodegrees * ((tm.GetHour() + TimeManager.hrsinday - tm.sunriseHr) % TimeManager.hrsinday));
    }
}
