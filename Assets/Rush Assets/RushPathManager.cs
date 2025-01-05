using System.Collections.Generic;
using UnityEngine;

public class RushPathManager : MonoBehaviour
{
    private List<Transform> rushPathAnchors = new List<Transform>();

    private void Start()
    {
        RoomGen roomGen = GetComponent<RoomGen>();
        if (roomGen != null)
        {
            // Get the path anchors from the RoomGen script
            rushPathAnchors = roomGen.GetRushPathAnchors();

            // Debug to confirm the anchors are collected
            Debug.Log($"RushPathManager collected {rushPathAnchors.Count} path anchors.");
        }
        else
        {
            Debug.LogError("RushPathManager could not find the RoomGen component!");
        }
    }

    public List<Transform> GetPathAnchors()
    {
        return rushPathAnchors;
    }
}
