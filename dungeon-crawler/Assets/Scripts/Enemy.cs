using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //public variables
    public Map map;
    public string enemyName;
    public float maxHealth;
    public float enemySpeed;
    public float enemyDamage;
    public float attackRange;
    public float inRangeSpeed; //As stated in design document, enemies
                               //should slow down before attacking.
    public float attackCooldown;

    //private variables
    private float timerForAttack;
    private bool canAttack = false;
    private float currentHealth;
    private float currentSpeed;
    private Transform target;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentSpeed = enemySpeed;
        timerForAttack = attackCooldown;
        
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        transform.position = map.GetSpawnPoint();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }
    void FixedUpdate()
    {    
        FollowPlayer();
    }

    private void StartCooldown()
    {
        currentSpeed = inRangeSpeed;

        if (timerForAttack > 0)
        {
            timerForAttack -= Time.deltaTime;
        }
        else
        {   
            canAttack = true;
            timerForAttack = attackCooldown;
        }
    }
    
    private void Attack()
    {
        //this checks if the player is in the enemy's range
        if (Vector2.Distance(transform.position, target.position) <= attackRange)
        {

            StartCooldown();

            if (canAttack)
            {
                //Just to give visual representation of when the enemy attacked
                Debug.Log(enemyName + " has attacked");
                canAttack = false;
            }
        } 
        else  //resets the cooldown if the player leaves the enemy's range in the next frame/update
        {
            timerForAttack = attackCooldown;  
        }
    }

    private void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        currentSpeed = enemySpeed; // if speed was changed because of the Attack
    }
}
