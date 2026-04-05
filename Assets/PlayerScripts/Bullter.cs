using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bullter : MonoBehaviour
{
    public Vector3 targetPos;
    private Camera cam;
    public InputActionReference aim;
    private Rigidbody2D rb;
    
    private IEnumerator coroutine;
    public float time = 10f;
    
    public float damage = 1;
    
    public static bool flame = false;
    public static bool ice = false;
    public static bool gust = false;
    
    [Header("Ice Stats")]
    public float slowDuration = 4f;

    [Header("Burn Stats")]
    public float burnDamage = 1f;
    public float burnInterval = 2f;
    public float burnDuration = 6f;

    [Header("Gust Stats")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    public static bool wildfire = false;
    public static bool freeze = false;
    
    [Header("Wildfire Stats")]
    public float wildfireRange = 3f;

    
    void Start()
    {
        fire();
    }


    void fire()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 direction = targetPos - transform.position;
        Vector3 rotation = transform.position - targetPos;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * Shooting.bulletSpeed;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
        coroutine = destroyAfterTime(time);
        StartCoroutine(coroutine);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall")
        {
            Destroy(this.gameObject);
        }

        if (other.tag == "Enemy")
        {
            if (Bullter.wildfire)
            {
                EnemyBase hitEnemy = other.GetComponent<EnemyBase>();
                if (hitEnemy != null && hitEnemy.onFire && !hitEnemy.hasSpreadFire)
                {
                    hitEnemy.hasSpreadFire = true;

                    Collider2D[] nearby = Physics2D.OverlapCircleAll(other.transform.position, wildfireRange);
                    foreach (Collider2D col in nearby)
                    {
                        if (col.CompareTag("Enemy") && col.gameObject != other.gameObject)
                        {
                            EnemyBase nearbyEnemy = col.GetComponent<EnemyBase>();
                            if (nearbyEnemy != null && !nearbyEnemy.onFire)
                            {
                                nearbyEnemy.onFire = true;
                                nearbyEnemy.StartCoroutine(nearbyEnemy.Burn(1f, 2f, 6f));
                            }
                        }
                    }

                    // Spawn a separate object to run the coroutine so it survives bullet destruction
                    GameObject vfxRunner = new GameObject("WildfireVFX");
                    WildfireVFX vfx = vfxRunner.AddComponent<WildfireVFX>();
                    vfx.Show(other.transform.position, wildfireRange);
                }
            }

            Destroy(this.gameObject);
        }
    }
    
    private IEnumerator destroyAfterTime(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            Destroy(this.gameObject);
        }
    }
}