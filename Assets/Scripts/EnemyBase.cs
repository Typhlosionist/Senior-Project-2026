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
    [SerializeField] public float Damage = 1;


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
        darknessController = GameObject.Find("DarknessController").GetComponent<DarknessController>();
    }

    private void FixedUpdate() {

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

    void BecomeNightmode()
  {
    isNightmode = true;
    MoveSpeed = MoveSpeed * 1.5f;
  }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemy Trigger Entered");
    }
}
    