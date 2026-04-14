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
        string buttonText = GetComponentInChildren<TextMeshProUGUI>().text;
        Debug.Log(buttonText + " clicked");

        switch (buttonText)
        {
            case "Bullet Speed":
                Shooting.bulletSpeed = 20f;
                break;

            case "Spread":
                Shooting.spreadOne = true;
                break;

            case "Spread+":
                if (Shooting.spreadOne)
                    Shooting.spreadTwo = true;
                else
                    Debug.Log("Spread 1 not unlocked");
                break;

            case "Fire Rate":
                Shooting.shootCooldown = .3f;
                break;

            case "Ice":
                Bullter.ice = true;
                break;

            case "Freeze":
                if (Bullter.ice)
                    Bullter.freeze = true;
                else
                    Debug.Log("Ice not unlocked");
                break;

            case "Fire":
                Bullter.flame = true;
                break;

            case "Wildfire":
                if (Bullter.flame)
                    Bullter.wildfire = true;
                else
                    Debug.Log("Fire not unlocked");
                break;

            case "Gust":
                Bullter.gust = true;
                break;

            default:
                Debug.Log("Unknown upgrade: " + buttonText);
                break;
        }
    }
}
