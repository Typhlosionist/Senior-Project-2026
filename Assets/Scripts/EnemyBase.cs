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
        navGrid = GameObject.Find("NavGrid").GetComponent<NavGrid>();

        darknessController = GameObject.Find("DarknessController").GetComponent<DarknessController>();
    }

    private void FixedUpdate() {

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

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.name + " Entered hitbox of " + this.name);
    }


}
