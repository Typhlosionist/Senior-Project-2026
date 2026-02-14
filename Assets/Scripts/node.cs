using UnityEngine;
using System.Collections.Generic;

public class node : MonoBehaviour
{
    public NavGrid parentSpawner;
    public int ID;
    public List<GameObject> neighbors = new List<GameObject>();

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public Vector2 getGlobalPosition()
    {
        return transform.position;
    }

}
