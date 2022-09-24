using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed = 5;
    // 斜方向移动需要降速
    public float slowDown = 0.6f;
    private float inputX;
    private float inputY;
    private Vector2 movementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        // 斜方向移动需要减速
        if (inputX != 0 && inputY != 0)
        {
            inputX *= slowDown;
            inputY *= slowDown;
        }

        movementInput = new Vector2(inputX, inputY);
    }

    private void Movement()
    {
        rb.MovePosition(rb.position + movementInput * speed * Time.deltaTime);
    }
}