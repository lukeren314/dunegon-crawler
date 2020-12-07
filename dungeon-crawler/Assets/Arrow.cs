using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;
    public float speed;
    public float lifeTime;

    public float distance;
    public LayerMask solids;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, solids);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy")) 
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
                    
                Debug.Log("Enemy has taken damage!");
            }
            Destroy(gameObject);

        }

        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
/*
    void onCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
            Debug.Log("Enemy has taken damage!");
        }
        
    }
*/
}
