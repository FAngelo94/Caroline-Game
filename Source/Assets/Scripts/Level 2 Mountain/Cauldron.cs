using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron: InteractAnimal
{
    [Header("Name of jarFull item")]
    public string jarFull = "";
    [Header("Name of jarEmpty item")]
    public string jarEmpty = "";
    [Header("Name of jarMedicine item")]
    public string jarMedicine = "";
    [Header("Water in the cauldron")]
    public GameObject water;

    [Header("Name of fireStick item")]
    public string fireStick = "";
    [Header("Name of stick item")]
    public string stick = "";
    [Header("Fire under the cauldron")]
    public GameObject fire;

    [Header("Medicine in the cauldron")]
    public GameObject medicine;
    [Header("The Doctor")]
    public GameObject mole;

    private bool userNear;

    void Start()
    {
        userNear = false;
        water.SetActive(false);
        fire.SetActive(false);
        medicine.SetActive(false);
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
        bool ok = false;
        string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
        if (equippedObject.Equals(jarFull) && !water.activeSelf)
        {
            ManageInventory.instance.RemoveItem(jarFull);
            ManageInventory.instance.AddItem(jarEmpty);
            ManageInventory.instance.EquippedLastItemPicked();
            water.SetActive(true);
            ok = true;
        }
        if (equippedObject.Equals(fireStick) && !fire.activeSelf)
        {
            fire.SetActive(true);
            ManageInventory.instance.RemoveItem(fireStick);

            mole.GetComponent<Doctor>().sadFace.SetActive(false);
            mole.GetComponent<Doctor>().helperFace.SetActive(true);
            ok = true;
        }
        if(equippedObject.Equals(jarEmpty) && medicine.activeSelf)
        {
            ManageInventory.instance.RemoveItem(jarEmpty);
            ManageInventory.instance.AddItem(jarMedicine);
            ManageInventory.instance.EquippedLastItemPicked();
            ok = true;
        }
        if(ok==false)
        {//User is using a wrong object
            ManageHelper.instance.WrongUseObject(equippedObject);
        }
    }
    override
    public bool UseObjectConditions()
    {
        string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
        if (userNear && !equippedObject.Equals(""))
            return true;
        return false;
    }
}
