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
    public float moveToNodeDist = 0.1f;

    [Header("Enemy Components")]
    public Rigidbody2D rb;

    void Awake()
    {
        navGrid = GameObject.Find("NavGrid").GetComponent<NavGrid>();
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

    //Transitions the object's velocity from it's current velocity to the desired velocity
    //TODO
    //Note: IEnumerator better?
    void TransitionVelocity(Vector2 targetVal, float rate)
    {
        
    }

}
