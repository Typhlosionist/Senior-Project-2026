using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;
using UnityEditor;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] public float MaxHealth = 1;
    public float CurrentHealth;
    [SerializeField] public float MoveSpeed = 1;
    private float originalMoveSpeed;
    [SerializeField] public float Damage = 1;
    [SerializeField] public bool slowed = false;
    [SerializeField] public bool frozen = false;
    [SerializeField] public bool onFire = false;
    [SerializeField] public bool knockedBack = false;
    [SerializeField] public bool hasSpreadFire = false;

    [Header("Targeting")]
    [SerializeField] public GameObject AttackTarget;
    public bool LineOfSight = false;
    [SerializeField] private LayerMask raycastMask;

    [Header("Pathfinding Variables")]
    public List<GameObject> path;
    public NavGrid navGrid;
    public float pathfindCooldown = 1;
    public bool pathfindOnCooldown = false;
    public float moveToNodeDist = 0.1f;

    [Header("Enemy Components")]
    public Rigidbody2D rb;

    void Awake()
    {
        navGrid = GameObject.Find("NavGrid").GetComponent<NavGrid>();
        originalMoveSpeed = MoveSpeed;
    }

    private void FixedUpdate()
    {
        isDead();
        
        Vector2 direction = AttackTarget.transform.position - transform.position;
        float distance = direction.magnitude;

        RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, distance, raycastMask);

        if(ray.collider != null)
        {
            LineOfSight = ray.collider.CompareTag("Player");
            if (LineOfSight)
            {
                Debug.DrawRay(transform.position, AttackTarget.transform.position - transform.position, color: Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, AttackTarget.transform.position - transform.position, color: Color.red);
            }
        }
        else
        {
            Debug.Log("Raycast error, make sure to set raycast mask");
        }
    }

    public IEnumerator Pathfind()
    {
        if(!pathfindOnCooldown){
            pathfindOnCooldown = true;
            path = navGrid.FindNodePath(transform.position, AttackTarget.transform.position);
            yield return new WaitForSeconds(pathfindCooldown);
            pathfindOnCooldown = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name + " Entered hitbox of " + this.name);

        if (other.name == "Bullet(Clone)")
        {
            Bullter bullet = other.GetComponent<Bullter>();

            if (Bullter.ice)
            {
                if (!slowed && !frozen)
                {
                    StartCoroutine(Freeze(bullet.slowDuration));
                }
                else if (slowed && !frozen && Bullter.freeze)
                {
                    StopCoroutine("Freeze");
                    StartCoroutine(Freeze(bullet.slowDuration));
                }
            }

            if (Bullter.flame && !onFire)
            {
                onFire = true;
                StartCoroutine(Burn(bullet.burnDamage, bullet.burnInterval, bullet.burnDuration));
            }

            if (Bullter.gust)
            {
                Vector2 knockbackDir = (transform.position - other.transform.position).normalized;
                StartCoroutine(Gust(knockbackDir, bullet.knockbackForce, bullet.knockbackDuration));
            }
        
            CurrentHealth -= bullet.damage;
        }
    }

    private IEnumerator Freeze(float slowDuration)
    {
        if (slowed)
        {
            frozen = true;
            MoveSpeed = 0;
            Debug.Log(this.name + " is frozen!");

            yield return new WaitForSeconds(slowDuration);

            MoveSpeed = originalMoveSpeed;
            slowed = false;
            frozen = false;
            Debug.Log(this.name + " is no longer frozen");
        }
        else
        {
            slowed = true;
            MoveSpeed = originalMoveSpeed / 2;
            Debug.Log(this.name + " is slowed!");

            yield return new WaitForSeconds(slowDuration);

            if (!frozen)
            {
                MoveSpeed = originalMoveSpeed;
                slowed = false;
                Debug.Log(this.name + " is no longer slowed");
            }
        }
    }

    public IEnumerator Burn(float burnDamage, float burnInterval, float burnDuration)
    {
        float elapsed = 0f;

        while (elapsed < burnDuration)
        {
            yield return new WaitForSeconds(burnInterval);
            CurrentHealth -= burnDamage;
            elapsed += burnInterval;
            Debug.Log(this.name + " is burning");
        }

        onFire = false;
        Debug.Log(this.name + " is no longer burning");
    }

    private IEnumerator Gust(Vector2 direction, float knockbackForce, float knockbackDuration)
    {
        knockedBack = true;
        float elapsed = 0f;

        while (elapsed < knockbackDuration)
        {
            rb.linearVelocity = direction * knockbackForce;
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = Vector2.zero;
        knockedBack = false;
    }

    void isDead()
    {
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void TransitionVelocity(Vector2 targetVal, float rate)
    {
        
    }
}