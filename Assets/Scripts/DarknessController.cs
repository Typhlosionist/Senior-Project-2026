using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DarknessController : MonoBehaviour
{
    Light2D lighting;
    public float darkLevel = 0.02f;
    public float transitionTime = 2;
    public bool isNight = false;

    

    void Start()
    {
        lighting = GetComponent<Light2D>();

        lighting.intensity = 1;
    }

    [ContextMenu("Trigger Darkness")]
    public void TriggerDarkness()
    {
        StartCoroutine(TransitionDarkness());
    }
    
    IEnumerator TransitionDarkness()
    {
        float time = 0;

        while (time < transitionTime)
        {
            float t = time / transitionTime;
            lighting.intensity = Mathf.Lerp(1, darkLevel, t);

            time += Time.deltaTime;
            yield return null;
        }

        lighting.intensity = darkLevel;
        isNight = true;
    }
}
