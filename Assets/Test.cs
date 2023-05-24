using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.name);
    }
}