using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeItem : InteractAnimal
{
    [Header("Name of original item")]
    public string oldItem = "";
    [Header("Name of new item")]
    public string newItem = "";

    private bool userNear;

    void Start()
    {
        userNear = false;
    }

    void Update()
    {
        if (userNear)
            StartCoroutine(ManageBalloon());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            userNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            userNear = false;
        }
    }

    override
    public void Interaction()
    {
    }
    override
    public bool InteractionConditions()
    {
        return userNear;
    }

    override
    public void ObjectUse()
    {
        ManageInventory.instance.RemoveItem(oldItem);
        ManageInventory.instance.AddItem(newItem);
        ManageInventory.instance.EquippedLastItemPicked();
    }
    override
    public bool UseObjectConditions()
    {
        string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
        if (userNear && equippedObject.Equals(oldItem))
            return true;
        return false;
    }
}

