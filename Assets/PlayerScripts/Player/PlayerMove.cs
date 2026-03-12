using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    
    
    public float moveSpeed;
    public Rigidbody2D rb;
    public CircleCollider2D circleCollider;

    private Vector2 moveDir;

    public InputActionReference move;
    public InputActionReference dash;

    public static bool freeze = false;
    public static bool isDashing = false;
    
    public float dashLen;
    private float dashCd = 1f;
    private float dashCount;
    private float dashCool;
    public float dashSpeed;
    private Vector2 dashDir;
    

    void Update()
    {
        if (!freeze)
        {
            moveDir = move.action.ReadValue<Vector2>();
            
            
        }
        else
        {
            moveDir = Vector2.zero;
        }
        
        if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {
            StartCoroutine(dashing());
        }
    }

    private IEnumerator dashing()
    {
        isDashing = true;
        circleCollider.enabled = false;
        rb.linearVelocity = new Vector2(moveDir.x * dashSpeed, moveDir.y * dashSpeed);
        yield return new WaitForSeconds(dashLen);
        circleCollider.enabled = true;
        isDashing = false;
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
        }
    }
}
