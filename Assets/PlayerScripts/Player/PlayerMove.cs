using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    
    
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
        
        moveDir = move.action.ReadValue<Vector2>();
        
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
