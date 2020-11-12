using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public Animator animator;
    public Rigidbody2D rb;

    private const float playerSpeed = 4.0f;
    private Vector2 movement;
    private LevelGenerator levelGenerator;

    private void Start()
    {
        levelGenerator = new LevelGenerator();
        levelGenerator.GenerateLevel(10, 10, 10, 0.5f, 0.75f);
    }

    private void Update()
    {
        Vector3 input = GetInput();
        movement = input * playerSpeed;

        //adjusts parameters for animation
        //animator.SetFloat("Horizontal", input.x);
        //animator.SetFloat("Vertical", input.y);
        //animator.SetFloat("Magnitude", input.magnitude);
    }

    private void FixedUpdate()
    {
        rb.velocity = movement;
    }

    private Vector3 GetInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        return new Vector3(x, y, 0.0f);
    }
}
