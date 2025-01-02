using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace

public class RoomCounter : MonoBehaviour
{
    public int roomCount = 1;
    private TextMeshProUGUI roomCountText; // Private reference to the UI Text element
    void Start()
    {
        // Find the Text component in the children of this GameObject
        roomCountText = GetComponentInChildren<TextMeshProUGUI>();

        if (roomCountText == null)
        {
            Debug.LogError("RoomCountText (Text UI) is not found as a child of RoomCounter.");
        }
    }

    void Update()
    {
         if (roomCountText != null)
        {
            roomCountText.text = "Room: " + roomCount; // Set the text to show the current room count
        }
        
    }
}
