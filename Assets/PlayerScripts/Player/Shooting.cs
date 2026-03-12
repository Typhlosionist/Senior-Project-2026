using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public InputActionReference fire;
    public InputActionReference aim;


    private Camera cam;
    public Vector3 targetPos { get; set;}
    public GameObject bullet;
    public Transform bulletTrans;
    public bool canFire = true;
    private float timer;
    public static float shootCooldown = .8f;
    
    public static float bulletSpeed = 10f;
    public Vector3 rotation { get; set;}
    
    private bool freeze = PlayerMove.freeze;
    private bool isDashing = PlayerMove.isDashing;

    public static bool spreadOne = false;
    public static bool spreadTwo = false;
    
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    
    void Update()
    {
        freeze = PlayerMove.freeze;
        if (!freeze || isDashing)
        {
            targetPos = cam.ScreenToWorldPoint(aim.action.ReadValue<Vector2>());

            rotation = targetPos - transform.position;

            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            if (!canFire)
            {
                timer += Time.deltaTime;
                if (timer > shootCooldown)
                {
                    canFire = true;
                    timer = 0;
                }
            }

            if ((fire.action.ReadValue<float>() == 1) && canFire)
            {
                canFire = false;

                int num = 1;

                float spreadAngle = 0;
                
                if (spreadOne)
                {
                    num = 2;
                    spreadAngle = 10f;
                }

                if (spreadTwo)
                {
                    num = 3;
                    spreadAngle = 20f;
                }
                
                for (int i = 0; i < num; i++)
                {
                    float angleOffset = 0;

                    if (num > 1)
                    {
                        angleOffset = -spreadAngle / 2 + (spreadAngle / (num - 1)) * i;
                    }

                    Vector3 dir = (targetPos - bulletTrans.position).normalized;

                    Vector3 spreadDir = Quaternion.Euler(0, 0, angleOffset) * dir;

                    Vector3 spreadTarget = bulletTrans.position + spreadDir * 100f;

                    GameObject bull = Instantiate(bullet, bulletTrans.position, bulletTrans.rotation);
                    bull.tag = gameObject.tag;

                    bull.GetComponent<Bullter>().targetPos = spreadTarget;
                }
            }
        }
    }
}
