using System.Collections;
using UnityEngine;

public class SpitterShot : MonoBehaviour
{
    Vector2 direction;
    float speed;
    float damage;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void setValues(Vector2 dir, float spd, float dmg)
    {
        Vector2 direction = dir;
        float speed = spd;
        float damage = dmg;

        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction * speed;

        StartCoroutine(selfDestruct());
    }

    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(5);
        
        Debug.Log("self Destruct");
        Destroy(gameObject);

    }

    public float getDamage()
    {
        return damage;
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("shot collided");
        Destroy(gameObject);
    }
}
