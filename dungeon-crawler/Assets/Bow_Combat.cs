using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow_Combat : MonoBehaviour
{
    public GameObject arrow;
    public Transform shotPoint;
    public float offset;

    private float timeBetweenShots;
    public float startTimeBetweenShots;

    // Update is called once per frame
    void Update()
    {
        Vector2 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (timeBetweenShots <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(arrow, shotPoint.position, transform.rotation);
                timeBetweenShots = startTimeBetweenShots;
            }
        }
        timeBetweenShots -= Time.deltaTime;

    }
}
