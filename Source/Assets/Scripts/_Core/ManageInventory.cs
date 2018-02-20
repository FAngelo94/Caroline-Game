using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageInventory : MonoBehaviour, ISavableObject {

    [Header("Rows in the inventory")]
    public int rows = 4;
    [Header("Columns in the inventory")]
    public int columns = 4;
    [Header("Palen for brush effect")]
    public GameObject panel;

    //Used by other class to use the method of this
    public static ManageInventory instance;

    //check if inventory is opened of closed
    public bool OpenedInventory { get; private set; }
    //Item select in the inventory open
    private int selectedItem;
    //Item equip 
    private int equippedItem;
    //Inventory that this class manage
    private GameObject inventory;
    private int itemsCount = 0;
    //Dictionary to translate order to pick up with order in inventary
    private int[] itemsPicked;
    private const string C_NAME = "inventory";

    public List<GameObject> items;
    //private List<string> nameItems;
    private int allHandItems;
    public GameObject handItems;

    public void Awake()
    {
        //handItems = GameObject.Find("Caroline Walking Arm1");
        allHandItems = handItems.transform.childCount;
        Debug.Log("all items=" + allHandItems);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        inventory = gameObject.transform.Find("Inventory").gameObject;
    }

    // Use this for initialization
    void Start () {
        
        NewInventory();
        inventory.SetActive(true);
        //inventory.GetComponent<CanvasRenderer>().SetAlpha(0.3f);
    }
	
    public void HideInventory()
    {
        inventory.GetComponent<CanvasRenderer>().SetAlpha(0f);
    }

    public void ShowInventory()
    {
        inventory.GetComponent<CanvasRenderer>().SetAlpha(0.3f);
    }

    public void DisableInventory()
    {
        inventory.SetActive(false);
    }
    public void AbleInventory()
    {
        inventory.SetActive(true);
    }

    public void NewInventory()
    {
        //items = new List<GameObject>();
        //nameItems = new List<string>();


        //List<GameObject> children = new List<GameObject>();
        //foreach (Transform child in inventory.transform) children.Add(child.gameObject);
        //children.ForEach(child => Destroy(child));
        itemsCount = 0;
        OpenedInventory = false;
        selectedItem = 0;
        equippedItem = -1;
        itemsPicked = new int[items.Count];

        foreach (GameObject g in items)
            g.SetActive(false);

    }

	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Inventory") && handItems.transform.childCount==allHandItems)
        {
            //Debug.Log(GameManager.instance.GamePaused);
            if (!OpenedInventory && !GameManager.instance.GamePaused)
            {
                panel.SetActive(true);
                OpenInventory();
            }
            else if (OpenedInventory == true && !GameManager.instance.IngameMenuOpened)
            {
                panel.SetActive(false);
                CloseInventory();
            }
        }
        if (OpenedInventory && !GameManager.instance.IngameMenuOpened)
        {
            ChangeSelectedItem();
            ChangeEquippedItem();
        }
    }

    ///<summary>
    ///The only assumption I do when I add an item is that the renderer in 
    ///icon object has size 1x1 (put "Draw Mode" equal to slice to see the size) 
    ///<para> name = Name that appear in inventory</para>
    ///<para> item = Item icon that appear in inventory </para>
    /// </summary>
    public void AddItem(string name)
    {
        //get window size
        RectTransform rt = (RectTransform)inventory.transform;
        float inventoryWidth = rt.rect.width;
        float inventoryHeight = rt.rect.height;
        //Debug.Log("size: " + inventoryWidth + " x " + inventoryHeight);
        Debug.Log("num items: " + items.Count);
        //newObjects position calculation
        int col = itemsCount % columns;
        int row = (int)items.Count / columns;
        float spaceCol = (inventoryWidth - columns) / (columns + 1);
        float spaceRow = (inventoryHeight - rows) / (rows + 1);
        //Debug.Log(spaceRow);
        float x = (-inventoryWidth / 2 + col + (col + 1) * spaceCol + 0.5f);
        //float y = (inventoryHeight / 2 - row - (row + 1) * spaceRow - 0.5f);
        float y = 0;
        //Debug.Log("X:"+x+" Y:"+y);


        GameObject newObject = null;
        int n = 0;
        foreach (GameObject g in items)
        {
            if (g.name == name)
            {
                newObject = g;
                break;
            }
            else
                n++;
        }
        itemsPicked[itemsCount] = n;

        if (newObject == null)
        {
            Debug.Log("tried to add: " + name + ", but failed: item not found");
            return;
        }

        //Create new object in the invetory equal to the object that player get
        //GameObject newObject = Instantiate(item, (RectTransform)inventory.transform, false);
        //set it's position  
        RectTransform rt1 = newObject.GetComponent<RectTransform>();
        rt1.position = new Vector3(rt.position.x+x, rt.position.y+y, -3);

        newObject.SetActive(true);
        //needed to set the overrideSorting
        
        foreach (Canvas c in newObject.GetComponents<Canvas>())
            c.overrideSorting = true;
           
        for(int i = 0; i < newObject.transform.childCount; i++)
        {
            GameObject o = newObject.transform.GetChild(i).gameObject;            
            o.SetActive(true);
            o.GetComponent<Canvas>().overrideSorting = true;
            o.SetActive(false);
        }
        itemsCount++;

    }

    /// <summary>
    /// Return the name of the item that player equip in this moment, if player doesn't equip
    /// any item return ""
    /// </summary>
    /// <returns></returns>
    public string ReturnNameEquippedItem()
    {
        
        if (equippedItem >= 0)
        {
            return items[equippedItem].name;// nameItems[equippedItem];
        }
        return "";
    }

    /// <summary>
    /// Remove an item from inventory using its name
    /// </summary>
    /// <param name="n">the name of item you want remove</param>
    public void RemoveItem(string name)
    {
        int n;
        for (n = 0; n < items.Count && items[n].name != name; n++) ;
        if (equippedItem >= 0 && n < items.Count && n >= 0)
        {
            items[n].SetActive(false);
        }
        else
        {
            Debug.LogError("Tried to remove not valid item");
            return;
        }
        //Find orderpicked
        int orderIndex = 0;
        for(int j=0;j<itemsCount;j++)
        {
            if (itemsPicked[j] == n)
            {
                orderIndex = j;
                break;
            }
        }
        for(int j=orderIndex;j<itemsCount-1;j++)
        {
            itemsPicked[j] = itemsPicked[j + 1];
        }
        int[] originalItemsPicked = new int[itemsPicked.Length];
        int originalItemsCount = itemsCount;
        for(int i=0;i<itemsCount-1;i++)
        {
            originalItemsPicked[i] = itemsPicked[i];
        }
        itemsCount = 0;
        for (int i = 0; i < originalItemsCount-1; i++)
        {
            AddItem(items[originalItemsPicked[i]].name);
        }
        equippedItem= -1;
        UnequippedAllItems();
    }


    public void Save()
    {
        OpenInventory();

        //Debug.Log("saving bag size: " + items.Count);
        //PlayerPrefs.SetInt(C_NAME + "size", items.Count);
        foreach(GameObject g in items)
        {
            PlayerPrefs.SetString(C_NAME + "item" + g.name, g.activeInHierarchy.ToString());
            //PlayerPrefs.SetString(C_NAME + "item" + i, items[i].name);
            Debug.Log("saving obj: " + g.name +" : " + g.activeInHierarchy);
        }
            
        PlayerPrefs.SetInt(C_NAME + "selected", selectedItem);
        PlayerPrefs.SetInt(C_NAME + "equipped", equippedItem);

        CloseInventory();
    }

    public void Load()
    {
        NewInventory();
        foreach (GameObject g in items)
        {
            Debug.Log(g.name +" : " + PlayerPrefs.GetString(C_NAME + "item" + g.name));
            if (PlayerPrefs.GetString(C_NAME + "item" + g.name) == "True") {
                AddItem(g.name);
                GameObject obj = GameObject.Find(g.name + "Object");
                if (obj != null)
                    obj.SetActive(false);
                else
                    Debug.Log("tried to deactice collectible : " + g.name + "Object");
            }
            //obj.SetActive(false);
        }
        selectedItem = PlayerPrefs.GetInt(C_NAME + "selected");
        EquipItem(PlayerPrefs.GetInt(C_NAME + "equipped"));
    }

    //PRIVATE METHODS

    /// <summary>
    /// Check when equip button is pressed
    /// </summary>
    private void ChangeEquippedItem()
    {
        if(Input.GetButtonDown("Select") && itemsCount>0)
        {
            EquipItem(itemsPicked[selectedItem]);
        }
    }
    
    /// <summary>
    /// Equip the item identify by the index
    /// </summary>
    /// <param name="n"></param>
    private void EquipItem(int n)
    {
        if(items.Count>0)
        {
            UnequippedAllItems();
            if (equippedItem != n) //In this way player can unequip an item
            {
                equippedItem = n;
                GameObject item = items[n];
                item.transform.Find("equipItem").gameObject.SetActive(true);
                ShowEquipment.instance.ShowObject(item.name);
                SoundManager.instance.EquipItem();

                //modify Helper
                ManageHelper.instance.ChangeObject(item.name);
            }
            else
            {
                SoundManager.instance.unequipItem();
                equippedItem = -1;
            }
                
        }
    }

    /// <summary>
    /// This method work for the items that change when you do something
    /// </summary>
    /// <param name="name"></param>
    public void EquippedLastItemPicked()
    {
        EquipItem(itemsPicked[itemsCount - 1]);
    }

    /// <summary>
    /// Unequipped all items
    /// </summary>
    private void UnequippedAllItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            GameObject item = items[i];
            item.transform.Find("equipItem").gameObject.SetActive(false);
        }
        ShowEquipment.instance.HideObjects();
        //Modify Helper
        ManageHelper.instance.RemoveObject();
    }

    /// <summary>
    /// Check when the buttons to change select item are pressed
    /// Fix this!
    /// </summary>
    bool wait=true;
    private void ChangeSelectedItem()
    {
        if (wait)
        { 
            float h = Input.GetAxisRaw("Horizontal");
            if (h != 0)
            {
                if (h < 0 && selectedItem > 0)
                {
                    SoundManager.instance.ButtonFocussed();
                    selectedItem--;
                }

                if (h > 0 && selectedItem < itemsCount - 1)
                {
                    SoundManager.instance.ButtonFocussed();
                    selectedItem++;
                }
                    
                
            /*if (Input.GetKeyDown(KeyCode.W) && selectedItem / columns > 0)
                selectedItem = selectedItem - columns;
            if (Input.GetKeyDown(KeyCode.S) && selectedItem + columns < items.Count)
                selectedItem = selectedItem + columns;*/
                SelectItem(itemsPicked[selectedItem]);
                wait = false;
                StartCoroutine(WaitChange());
            }
        }
    }
    float secondsSong = 2;
    IEnumerator WaitChange()
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + 0.2)
        {
            yield return null;
        }
        wait = true;
    }

    /// <summary>
    /// Select the item identify by index
    /// </summary>
    /// <param name="n"></param>
    private void SelectItem(int n)
    {
        Debug.Log(n);
        if (items.Count > 0)
        {
            DeselectAllItems();
            GameObject item = items[n];
            item.transform.Find("selectItem").gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// Deselect all items in inventory
    /// </summary>
    private void DeselectAllItems()
    {
        for(int i=0;i<items.Count;i++)
        {
            GameObject item = items[i];
            item.transform.Find("selectItem").gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// Stop the game and open the inventory
    /// </summary>
    private void OpenInventory()
    {
        inventory.GetComponent<CanvasRenderer>().SetAlpha(1f);
        OpenedInventory = true;
        //Debug.Log("Open Inventory");
        GameManager.instance.PauseGame();
        //inventory.SetActive(true);
        selectedItem = 0;
        if(itemsCount>0)
            SelectItem(itemsPicked[selectedItem]);
    }


    //public CloseInventory because it's needed in the GameManager

    /// <summary>
    /// Close the inventory and resume the game
    /// </summary>
    public void CloseInventory()
    {
        inventory.GetComponent<CanvasRenderer>().SetAlpha(0.3f);
        OpenedInventory = false;
        //Debug.Log("Close Inventory");
        //inventory.SetActive(false);
        GameManager.instance.ResumeGame();
        DeselectAllItems();
    }


}