using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject player;
    public float distance;

    private Vector3 pos;
    private float angle;
    private void Update()
    {
        pos = Input.mousePosition;
        pos.z = (player.transform.position.z - Camera.main.transform.position.z);
        pos = Camera.main.ScreenToWorldPoint(pos);
        pos = pos - player.transform.position;
        angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(0, 0, angle);
        float x_pos = Mathf.Cos(Mathf.Deg2Rad * angle) * distance;
        float y_pos = Mathf.Sin(Mathf.Deg2Rad * angle) * distance;
        transform.localPosition = new Vector3(x_pos, y_pos, 0);
    }
}
