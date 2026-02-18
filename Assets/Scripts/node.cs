using UnityEngine;
using System.Collections.Generic;

public class node : MonoBehaviour
{
    public NavGrid parentSpawner;
    public int ID;
    public List<GameObject> neighboors = new List<GameObject>();

    public void DrawConnections()
    {
        foreach(GameObject neighboor in neighboors)
        {
            Debug.DrawLine(this.transform.position, neighboor.transform.position, Color.white, 0f);
        }
    }

    public void AddNeighboor(GameObject neighboor)
    {
        neighboors.Add(neighboor);
    }

    public Vector2 getGlobalPosition()
    {
        return transform.position;
    }



}
