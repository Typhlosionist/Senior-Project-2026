using System.Collections;
using UnityEngine;

public class WildfireVFX : MonoBehaviour
{
    public void Show(Vector3 position, float radius)
    {
        StartCoroutine(ShowWildfireRadius(position, radius));
    }

    private IEnumerator ShowWildfireRadius(Vector3 position, float radius)
    {
        GameObject circle = new GameObject("WildfireRadius");
        circle.transform.position = position;

        SpriteRenderer sr = circle.AddComponent<SpriteRenderer>();
        sr.sprite = CreateCircleSprite();
        sr.color = new Color(1f, 0f, 0f, 0.3f);
        sr.sortingLayerName = "Default";
        circle.transform.localScale = Vector3.one * radius * 2f;

        yield return new WaitForSeconds(0.5f);

        Destroy(circle);
        Destroy(gameObject);
    }

    private Sprite CreateCircleSprite()
    {
        int resolution = 128;
        Texture2D tex = new Texture2D(resolution, resolution);
        Vector2 center = new Vector2(resolution / 2f, resolution / 2f);
        float radius = resolution / 2f;

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                tex.SetPixel(x, y, (dist >= radius - 4 && dist <= radius)
                    ? Color.white
                    : Color.clear);
            }
        }

        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, resolution, resolution), new Vector2(0.5f, 0.5f));
    }
}