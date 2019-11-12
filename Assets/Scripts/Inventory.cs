using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IPointerClickHandler //Dejan
{
    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if (Inventory.instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Another instance of: " + this + " , was tried to be instantiated, but was destroyed! This instance was tried to be instantiated on: " + this.gameObject);
            Destroy(this);
        }
    }
    #endregion


    public GameObject inventory; //a refernce to the inventory parent component
    public GridLayoutGroup itemSpace; //a reference to the item space GridLayout
    public Transform itemSpaceTransform; //a reference to the item space transform
    public GameObjectList itemList; //a reference to the Inventory Items scriptable object
    public GameObject character; //a reference to the main character who is to use inventory
    public Button throwButton; //a reference to the menu button used for discarding items

    private bool inventoryIsOpen;
    private float screenWidth;
    private float screenHeight;
    private GameObject selectedObject; //the last selected inventory item

    void Start()
    {
        // adjusts the canvas item sizes depending on screen width and height
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        itemSpace.padding.left = (int)(screenWidth / 110.8f);
        itemSpace.padding.right = (int)(screenWidth / 110.8f);
        itemSpace.padding.top = (int)(screenHeight / 41.6f);
        itemSpace.padding.bottom = (int)(screenHeight / 41.6f);
        itemSpace.cellSize = new Vector2(screenWidth / 21.3f, screenHeight / 9.6f);
        itemSpace.spacing = new Vector2(screenWidth / 110.8f, screenHeight / 50);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab) && inventoryIsOpen) //opens inventory and unlocks the mouse
        {
            inventory.SetActive(false);
            inventoryIsOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
        else if (Input.GetKeyUp(KeyCode.Tab) && !inventoryIsOpen) //closes inventory and locks the mouse
        {
            inventory.SetActive(true);
            inventoryIsOpen = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }

        if (selectedObject != null) //make throw button not interactable when an item is not selected
        {
            throwButton.interactable = true;
        }
        else
        {
            throwButton.interactable = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData) //defines last inventory item selected and makes it a little bit transparent
    {
        if (eventData.rawPointerPress.transform.parent == itemSpaceTransform)
        {
            if (selectedObject != null)
            {
                Color tempColor = selectedObject.GetComponent<Image>().color;
                tempColor.a = 1f;
                selectedObject.GetComponent<Image>().color = tempColor;
            }
            selectedObject = eventData.rawPointerPress;
            Color tempColor2 = selectedObject.GetComponent<Image>().color;
            tempColor2.a = 0.8f;
            selectedObject.GetComponent<Image>().color = tempColor2;
        }
    }

    public void CloseInvetory() //used to close invetory through the close button
    {
        inventory.SetActive(false);
        inventoryIsOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void AddItemToInventory(int itemID) //used to add item of itemID to the invetory
    {
        Instantiate(itemList.list[itemID].pair[1], itemSpaceTransform);
    }

    public void RemoveItemFromInvetory() //used to discard item of itemID from the invetory and return the item to the real world
    {
        if (selectedObject != null)
        {
            Destroy(selectedObject);
            GameObject gameObject = Instantiate(itemList.list[selectedObject.GetComponent<InventoryItem>().itemID].pair[0], character.transform.position, Quaternion.identity);
            //gameObject.GetComponent<GatherableItem>().inventory = this;
        }
    }
}
