using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damager : MonoBehaviour
{
    public int damage = 1;
    
    protected void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag("Player") && gameObject.tag != "Player")
        {
            target.GetComponent<PlayerHpScript>().hurt(damage);
        }
        // depreciated from when I had my own enemy
        /// // if (target.CompareTag("bad") && gameObject.tag != "bad")
        // {
        //     target.GetComponent<hp>().hurt(damage);
        // }

    }
}
