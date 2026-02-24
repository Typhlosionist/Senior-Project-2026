using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UIElements;

public class UpgradeTree : MonoBehaviour
{
    public GameObject upgradeTree;
    
    private bool isOpen = false;

    void Start()
    {
        upgradeTree = GameObject.Find("UpgradeTree");
    }
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
    }
}
