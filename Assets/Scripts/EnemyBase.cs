using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // Enemy Stats
    [SerializeField] public float MaxHealth = 1;
    public float CurrentHealth;
    [SerializeField] public float MoveSpeed = 1;
    [SerializeField] public float Damage = 1;

    //Targeting
    [SerializeField] public GameObject AttackTarget;

    //Enemy Components
    public Rigidbody2D rb; 


    //Transitions the object's velocity from it's current velocity to the desired velocity
    //TODO
    //Note: IEnumerator better?
    void TransitionVelocity(Vector2 targetVal, float rate)
    {
        
    }

    void SeachForTarget()
    {
        
    }

    public void MoveToTarget()
    {
        
    }

}
