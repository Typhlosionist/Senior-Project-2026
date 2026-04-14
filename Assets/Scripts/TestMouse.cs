using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestMouse : MonoBehaviour
{
    void Start()
    {
        // Log all detected input devices
        foreach (var device in InputSystem.devices)
        {
            Debug.Log($"Device found: {device.name} | Type: {device.GetType().Name}");
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Space pressed");
            Destroy(this.gameObject);
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log($"Object '{gameObject.name}' was clicked and will now be destroyed.");
            Destroy(gameObject);
        }
    }
}