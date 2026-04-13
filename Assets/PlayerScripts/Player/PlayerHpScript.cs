using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHpScript : MonoBehaviour
{

    public static bool god = false;
    public int maxHp = 3;
    public int currentHp;
    public float iFrames;
    public int flashes;
    public SpriteRenderer sprite;
    public HealthBar healthBar;
    
    private bool isDashing;

    [Header("SFX")]
    [SerializeField] private AudioClip damageSFX;
    
    [SerializeField] private Volume volume;
    private Vignette vignette;
    
    void Start()
    {
        currentHp = maxHp;
        volume.profile.TryGet<Vignette>(out vignette);
        sprite = GetComponent<SpriteRenderer>();
        healthBar.SetMaxHealth(maxHp);
    }
    
    void Update()
    {
        /*if (Keyboard.current.spaceKey.wasPressedThisFrame) //testing health bar
        {
            Debug.Log("Space presssed");
            hurt(1);
        }*/
        if (vignette != null)
        {
            float hpPercent = (float)currentHp / maxHp;
        
            float baseIntensity = Mathf.Lerp(0.6f, 0f, Mathf.Pow(hpPercent, 3));
        
            float pulseSpeed = Mathf.Lerp(4f, 1f, hpPercent);
            float pulseAmount = Mathf.Lerp(0.15f, 0f, Mathf.Pow(hpPercent, 0.4f));
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f * pulseAmount;

            vignette.color.value = Color.Lerp(Color.red, Color.black, hpPercent);
            vignette.intensity.value = baseIntensity + pulse;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isDashing = PlayerMove.isDashing;
        if (!isDashing)
        {
            if (other.CompareTag("Enemy"))
            {
                hurt(1);
            }
        }
    }
    
    public void heal(int health)
    {
        if (currentHp < 3)
        {
            currentHp += health;
            healthBar.SetHealth(currentHp);
        }
    }

    public void hurt(int damage)
    {
        if (!god)
        {
            currentHp -= damage;
            SFXManager.instance.PlaySFX(damageSFX, transform, 1f);
            healthBar.SetHealth(currentHp);

            if (currentHp <= 0)
            {
                die();
            }
        }
    }

    public void die()
    {
        //play death anim
        //go to gameover
        gameObject.SetActive(false);
        Debug.Log("RIP");
    }

    // private IEnumerator invuln()
    // {
    //     Physics2D.IgnoreLayerCollision(10, 11, true);
    //     for (int i = 0; i < flashes; i++)
    //     {
    //         sprite.color = new Color(1, 0, 0, .5f);
    //         yield return new WaitForSeconds(iFrames / (flashes * 2));
    //         sprite.color = Color.white;
    //         yield return new WaitForSeconds(iFrames / (flashes *2));
    //     }
    //     Physics2D.IgnoreLayerCollision(10, 11, false);
    // }
}
