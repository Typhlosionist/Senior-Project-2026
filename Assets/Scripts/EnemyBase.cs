using System.Collections;
using UnityEngine;
using System.Collections.Generic;

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

    [HideInInspector] public Vector2 desiredVelocity = Vector2.zero;
    public float accelerationRate = 0.5f;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform sprite;

    //Night Transitioning Variables
    [HideInInspector] public DarknessController darknessController;
    [HideInInspector] public bool isNightmode = false;

    

    void Awake()
    {
        originalMoveSpeed = MoveSpeed;
        darknessController = GameObject.Find("DarknessController").GetComponent<DarknessController>();
    }

    private void FixedUpdate() {
        
        isDead();

        Vector2 origin = transform.position;
        Vector2 target = AttackTarget.transform.position;
        Vector2 direction = (target - origin).normalized;
        float distance = Vector2.Distance(origin, target);

        // Get collider half width (for left/right offsets)
        float halfWidth = GetComponent<CircleCollider2D>().bounds.extents.x;
    
        // Perpendicular to direction (for side offsets)
        Vector2 perp = new Vector2(-direction.y, direction.x);

        // Two ray origins (left and right edges)
        Vector2 originLeft = origin + perp * halfWidth;
        Vector2 originRight = origin - perp * halfWidth;

        RaycastHit2D hitLeft = Physics2D.Raycast(originLeft, direction, distance, raycastMask);
        RaycastHit2D hitRight = Physics2D.Raycast(originRight, direction, distance, raycastMask);

        //Both rays must collide with player
        if (hitLeft.collider != null && hitRight.collider != null)
        {
            if (hitLeft.collider.CompareTag("Player") && hitRight.collider.CompareTag("Player"))
            {
                LineOfSight = true;
                Debug.DrawRay(transform.position, AttackTarget.transform.position - transform.position, color: Color.green);
            }
            else
            {
                LineOfSight = false;
                Debug.DrawRay(transform.position, AttackTarget.transform.position - transform.position, color: Color.red);
            }
        }
        else
        {
            Debug.Log("Raycast error, make sure to set raycast mask");
        }

        //Interpolate current velocity towards desired velocity
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, desiredVelocity, Time.deltaTime * accelerationRate);
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

    public void ObjectCollide(Collider2D other)
    {
        Debug.Log(other.name + " Entered hitbox of " + this.name);

        if (other.CompareTag("bullet"))
        {
            
            Bullter bullet = other.GetComponent<Bullter>();

            Debug.Log("Hit with " + bullet.damage);

            CurrentHealth -= bullet.damage;

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
}
    