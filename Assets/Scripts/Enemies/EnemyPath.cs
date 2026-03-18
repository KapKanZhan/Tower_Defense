using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    public Transform[] Waypoints => waypoints;

    private void OnValidate()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            waypoints = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                waypoints[i] = transform.GetChild(i);
            }
        }
    }
}
