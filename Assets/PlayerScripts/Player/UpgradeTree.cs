using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UIElements;

public class UpgradeTree : MonoBehaviour
{
    public GameObject upgradeTree;
    
    private bool isOpen = false;
    
    void Update()
    {
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            if (!isOpen)
            {
                PlayerMove.freeze = true;
                upgradeTree.SetActive(true);
                isOpen = true;
            }
            else
            {
                PlayerMove.freeze = false;
                upgradeTree.SetActive(false);
                isOpen = false;
            }
        }
    }

    public void Clicked()
    {
        Debug.Log(GetComponentInChildren<TextMeshProUGUI>().text + " clicked");
        if (GetComponentInChildren<TextMeshProUGUI>().text == "Bullet Speed")
        {
            Shooting.bulletSpeed = 20f;
        }
        
        if (GetComponentInChildren<TextMeshProUGUI>().text == "Spread")
        {
            Shooting.spreadOne = true;
        }
        
        if (GetComponentInChildren<TextMeshProUGUI>().text == "Spread2" && Shooting.spreadOne)
        {
            Shooting.spreadTwo = true;
        }
        else
        {
            Debug.Log("Spread 1 not unlocked");
        }
        
        if (GetComponentInChildren<TextMeshProUGUI>().text == "Fire Rate")
        {
            Shooting.shootCooldown  = .3f;
        }
    }
}
