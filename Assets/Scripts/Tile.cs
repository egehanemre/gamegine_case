using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool hasAlly;
    public GameObject allyObject; // Reference to the placed ally object
    public SpriteRenderer spriteRenderer; // Reference to the tile's sprite renderer

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}