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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentHealth = MaxHealth;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    //Transitions the object's velocity from it's current velocity to the desired velocity
    //TODO
    //Note: IEnumerator better?
    public void TransitionVelocity(Vector2 targetVal, float rate)
    {
        
    }

}
