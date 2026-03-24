using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    // Update is called once per frame
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
