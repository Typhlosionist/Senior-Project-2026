using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public InputActionReference fire;
    public InputActionReference aim;
    
    
    private Camera cam;
    public Vector3 targetPos {get; set;}
    public GameObject bullet;
    public Transform bulletTrans;
    public bool canFire = true;
    private float timer;
    public float shootCooldown = .3f;
    
    public Vector3 rotation { get; set;}
    
    private bool freeze = PlayerMove.freeze;
    private bool isDashing = PlayerMove.isDashing;
    
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
                GameObject bull;
                bull = Instantiate(bullet, bulletTrans.position, Quaternion.identity);
                bull.tag = gameObject.tag;
            }
        }
    }
}
