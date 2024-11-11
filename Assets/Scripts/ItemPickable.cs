using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickable : MonoBehaviour, IPickable
{
    public ItemSO itemScriptableObject;
    
    public void PickItem()
    {
        // Destroy the item from the scene
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
