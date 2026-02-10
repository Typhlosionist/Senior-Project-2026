using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    
    public Animator playerAnimator;
    public float moveSpeed;
    public Rigidbody2D rb;

    private Vector2 moveDir;

    public InputActionReference move;
    
    private float dashLen = .25f;
    private float dashCd = 1f;
    private float dashCount;
    private float dashCool;
    public float dashSpeed;
    
    void Update()
    {
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
        
        //maybe make this invincible
        // if (Input.GetKeyDown(KeyCode.LeftShift))
        // {
        //     if (dashCool <= 0 && dashCount <= 0)
        //     {
        //         movingSpeed = dashSpeed;
        //         dashCount = dashLen;
        //     }
        // }
        //
        // if (dashCount > 0)
        // {
        //     dashCount -= Time.deltaTime;
        //
        //     if (dashCount <= 0)
        //     {
        //         movingSpeed = moveSpeed;
        //         dashCool = dashCd;
        //     }
        // }
        //
        // if (dashCool > 0)
        // {
        //     dashCool -= Time.deltaTime;
        // }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }
}
