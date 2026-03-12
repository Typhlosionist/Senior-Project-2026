using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    
    public Animator playerAnimator;
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
        playerAnimator.SetBool("Right", false);
        playerAnimator.SetBool("Left", false);
        playerAnimator.SetBool("Forward", false);
        playerAnimator.SetBool("Back", false);
        
        
        moveDir = move.action.ReadValue<Vector2>();
        if (moveDir.x == 0 && moveDir.y == 0)
        {
            playerAnimator.SetFloat("Speed", 0);
        }
        if (moveDir.x > 0)
        {
            playerAnimator.SetBool("Right", true);
            playerAnimator.SetFloat("Speed", moveSpeed);
        }
        else if (moveDir.x < 0)
        {
            playerAnimator.SetBool("Left", true);
            playerAnimator.SetFloat("Speed", moveSpeed);
        }

        if (moveDir.y > 0)
        {
            playerAnimator.SetBool("Back", true);
            playerAnimator.SetFloat("Speed", moveSpeed);
        }
        else if (moveDir.y < 0)
        {
            playerAnimator.SetBool("Forward", true);
            playerAnimator.SetFloat("Speed", moveSpeed);
        }

        Debug.Log("x " + moveDir.x);
        Debug.Log("y " + moveDir.y);
        
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