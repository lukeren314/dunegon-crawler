using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTile : MonoBehaviour
{
    public Collider2D collider2d;
    public SpriteRenderer spriteRenderer;
    public Sprite floorSprite;
    public Sprite doorSprite;

    public void Deactivate()
    {
        collider2d.enabled = false;
        spriteRenderer.sprite = floorSprite;
    }

    public void Activate()
    {
        collider2d.enabled = true;
        spriteRenderer.sprite = doorSprite;
    }
}
