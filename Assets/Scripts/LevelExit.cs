using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public Transform nextLevelSpawnPoint;

    public GameObject previousLevelParent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached level exit, going to next level");
            TeleportPLayer(other.transform);
            CleanUpPreviousLevel();
        }
    }

    private void TeleportPLayer(Transform player)
    {
        if (nextLevelSpawnPoint != null)
        {
            player.position = nextLevelSpawnPoint.position;
        }
        else
        {
            Debug.LogWarning("Next level spawn point is not set");
        }
    }

    private void CleanUpPreviousLevel()
    {
        if (previousLevelParent != null)
        {
            Destroy(previousLevelParent);
        }
    }
}
