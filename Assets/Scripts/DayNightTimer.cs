using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class DayNightTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float nightThreshold = 5f;          // Time in seconds to trigger event

    public float elapsedTime = 0f;
    public bool hasTriggered = false;

    public TMP_Text timeText;

    // Optional event you can hook into from inspector or code
    public DarknessController nightfall;
    public RectTransform handle;

    [Header("SFX")]
    [SerializeField] private AudioClip daySFX;
    [SerializeField] private AudioClip nightSFX;
    bool SFXplaying = false;

    private void Start()
    {
        if(nightfall == null)
        {
            nightfall = GameObject.Find("DarknessController").GetComponent<DarknessController>();
        }

        if(handle == null){
            handle = transform.Find("Canvas/Timer/handle").GetComponent<RectTransform>();
        }
        SFXManager.instance.PlaySFX(daySFX, transform, .2f);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        DisplayTime();
        if(!hasTriggered){
            // Clamp between 0 and 1
            float t = Mathf.Clamp01(elapsedTime / nightThreshold);

            // Interpolate from 80 to -80
            float angle = Mathf.Lerp(75f, -75f, t);

            // Apply rotation (Z axis)
            handle.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            handle.localRotation = Quaternion.Euler(0f, 0f, -75f);
        }

        if (elapsedTime >= nightThreshold)
        {
            if (!hasTriggered)
            {
                Trigger();
            }
            if (!SFXplaying)
            {
                StartCoroutine(NightTime());
            }
        }
    }

    void Trigger()
    {
        Debug.Log("Threshold reached at: " + elapsedTime);

        nightfall.TriggerDarkness();

        hasTriggered = true;
    }

    // Public getter if you want to read elapsed time
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    // Optional reset method
    public void ResetTimer()
    {
        elapsedTime = 0f;
        hasTriggered = false;
    }

    public void DisplayTime()
    {
        float minutes = Mathf.FloorToInt(elapsedTime / 60);
        float seconds = Mathf.FloorToInt(elapsedTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    private IEnumerator NightTime()
    {
        SFXManager.instance.PlaySFX(nightSFX, transform, .3f);
        SFXplaying = true;
        yield return new WaitForSeconds(nightSFX.length);
        SFXplaying = false;
    }
}