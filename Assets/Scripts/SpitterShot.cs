using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpitterShot : MonoBehaviour
{
    Vector2 direction;
    float speed;
    public float Damage;


    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void setValues(Vector2 dir, float spd, float dmg, bool isNight)
    {
        direction = dir;
        speed = spd;
        Damage = dmg;

        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction * speed;

        if(!isNight)
        {
            GetComponent<Light2D>().intensity = 0;
        }

        StartCoroutine(selfDestruct());
    }

    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(5);
        
        Debug.Log("self Destruct");
        Destroy(this.gameObject);

    }

    void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("shot collided");
        Destroy(this.gameObject);
    }
}
