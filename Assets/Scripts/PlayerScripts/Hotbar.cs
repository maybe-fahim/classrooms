using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [SerializeField]
    private InputActionReference Hotbar1, Hotbar2, Hotbar3, Hotbar4, Hotbar5; // References to the hotbar input actions
    [Header("General")]
    public List<itemType> inventoryList;
    public int selectedItem;
    [Header("Item gameobjects")]
    [SerializeField] GameObject Item1;
    [SerializeField] GameObject Item2;
    [SerializeField] GameObject Item3;
    [SerializeField] GameObject Item4;
    [SerializeField] GameObject Item5;
    [SerializeField] GameObject Item6;
    [SerializeField] GameObject Item7;
    


    [Header("Item prefabs")]
    [SerializeField] GameObject Item1Prefab;
    [SerializeField] GameObject Item2Prefab;
    [SerializeField] GameObject Item3Prefab;
    [SerializeField] GameObject Item4Prefab;
    [SerializeField] GameObject Item5Prefab;
    [SerializeField] GameObject Item6Prefab;
    [SerializeField] GameObject Item7Prefab;

    
    public Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>() { };
    public Dictionary<itemType, GameObject> itemInstantiate = new Dictionary<itemType, GameObject>() { };
    void Start() // Bind the hotbar keys to their functions
    {
        Hotbar1.action.performed += Hotbar1Pressed;
        Hotbar2.action.performed += Hotbar2Pressed;
        Hotbar3.action.performed += Hotbar3Pressed;
        Hotbar4.action.performed += Hotbar4Pressed;
        Hotbar5.action.performed += Hotbar5Pressed;

        itemSetActive.Add(itemType.Black, Item1);
        itemSetActive.Add(itemType.Green, Item2);
        itemSetActive.Add(itemType.Orange, Item3);
        itemSetActive.Add(itemType.Pink, Item4);
        itemSetActive.Add(itemType.White, Item5);
        itemSetActive.Add(itemType.Torch, Item6);
        itemSetActive.Add(itemType.Key, Item7);

        itemInstantiate.Add(itemType.Black, Item1Prefab);
        itemInstantiate.Add(itemType.Green, Item2Prefab);
        itemInstantiate.Add(itemType.Orange, Item3Prefab);
        itemInstantiate.Add(itemType.Pink, Item4Prefab);
        itemInstantiate.Add(itemType.White, Item5Prefab);
        itemInstantiate.Add(itemType.Torch, Item6Prefab);
        itemInstantiate.Add(itemType.Key, Item7Prefab);

        NewItemSelected();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void NewItemSelected()
    {
        Item1.SetActive(false);
        Item2.SetActive(false);
        Item3.SetActive(false);
        Item4.SetActive(false);
        Item5.SetActive(false);
        Item6.SetActive(false);
        Item7.SetActive(false);

        GameObject selectedItemGameobject = itemSetActive[inventoryList[selectedItem]];
        selectedItemGameobject.SetActive(true);
    }

    private void Hotbar1Pressed(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar 1 pressed");
        if (inventoryList.Count > 0)
        {
            selectedItem = 0;
            NewItemSelected();
        }
    }

    private void Hotbar2Pressed(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar 2 pressed");
        if (inventoryList.Count > 1)
        {
            selectedItem = 1;
            NewItemSelected();
        }
    }

    private void Hotbar3Pressed(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar 3 pressed");
        if (inventoryList.Count > 2)
        {
            selectedItem = 2;
            NewItemSelected();
        }
    }

    private void Hotbar4Pressed(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar 4 pressed");
        if (inventoryList.Count > 3)
        {
            selectedItem = 3;
            NewItemSelected();
        }
    }

    private void Hotbar5Pressed(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar 5 pressed");
        if (inventoryList.Count > 4)
        {
            selectedItem = 4;
            NewItemSelected();
        }
    }
}
