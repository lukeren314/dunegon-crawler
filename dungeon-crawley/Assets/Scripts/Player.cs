using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    private const float playerSpeed = 3.0f;
    private Vector2 movement;

    private void Update()
    {
        Vector2 input = GetInput();
        movement = input * playerSpeed;
    }

    private void FixedUpdate()
    {
        rb.velocity = movement;
    }

    private Vector2 GetInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        return new Vector2(x, y);
    }
}
