using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    void OntriggerEnter2D(Collider2D other)
    {
        Debug.Log("I collided with " + other.name);
        if (other.CompareTag("Enemy"))
        {
            GetComponent<PlayerHpScript>().hurt(1);
        }
    }
}
