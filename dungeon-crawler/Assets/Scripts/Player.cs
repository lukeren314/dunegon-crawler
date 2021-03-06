﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public Map map;

    private const float playerSpeed = 4.0f;
    private Vector2 movement;
    private Room currentRoom;

    public float maxHealth;
    private float currentHealth;

    private void Start()
    {
        transform.position = map.GetSpawnPoint();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        Vector3 input = GetInput();
        movement = input * playerSpeed;

        //adjusts parameters for animation
        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);
        animator.SetFloat("Magnitude", input.magnitude);
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject otherObject = collider.gameObject;
        if (otherObject.CompareTag("TriggerTile"))
        {
            currentRoom = otherObject.GetComponent<TriggerTile>().GetRoom();
            currentRoom.ActivateDoors();
        }
        else if (otherObject.CompareTag("ExitTile"))
        {
            // generate another level
        }
    }

    private void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("You have Died!");
        Destroy(gameObject);
    }

}
