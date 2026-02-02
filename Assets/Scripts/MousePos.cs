using UnityEngine;
using UnityEngine.InputSystem;

public class MousePos : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 worldPoint2d = new Vector2(worldPoint.x, worldPoint.y);
        this.transform.position = worldPoint2d;

    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("MousePos hitbox entered");
    }
}
