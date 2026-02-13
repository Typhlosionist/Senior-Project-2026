using UnityEngine;
using System.Collections.Generic;
using System.Net.NetworkInformation;

public static class ListExtensions
{
    public static void Shuffle<T>(this List<T> list)
    {
         for (int i = 0; i < list.Count; i++)
        {
            int randIdx = Random.Range(0, list.Count);
            T temp = list[i];
            list[i] = list[randIdx];
            list[randIdx] = temp;


        }
    }
}