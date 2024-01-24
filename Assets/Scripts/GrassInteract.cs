using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrassInteract : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            Vector3 tilePosition = transform.position;
            Debug.Log("OnTriggerEnter2D activated for grass tile at position: " + tilePosition);
        }
    }
}
