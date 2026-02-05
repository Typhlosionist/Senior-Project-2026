using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healer : MonoBehaviour
{
    public int health = 1;

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Player"))
        {
            target.GetComponent<PlayerHpScript>().heal(health);
            Destroy(gameObject);
        }
    }
}