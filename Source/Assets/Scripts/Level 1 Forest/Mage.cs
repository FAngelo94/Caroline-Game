using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : InteractAnimal
{

    [Header("Balloon to Show the items he need")]
    public GameObject itemsBalloon;
    [Header("Balloon to Show the item 1 he need")]
    public GameObject item1Balloon;
    [Header("Balloon to Show the item 2 he need")]
    public GameObject item2Balloon;
    [Header("Balloon to Say that the potion is ready")]
    public GameObject confirmBalloon;
    [Header("Container for balloon")]
    public GameObject container;

    [Header("Item 1 mage needs")]
    public string item1Needs = "";
    [Header("Item 2 mage needs")]
    public string item2Needs = "";

	public GameObject item1Object;
	public GameObject item2Object;

    [Header("Magic potion that appear")]
    public GameObject magicPotion;

    [Header("Smilies")]
    public GameObject happyFace;
    public GameObject sadFace;
    public GameObject helperFace;

    private bool userNear;
    private bool item1Obtain;
    private bool item2Obtain;

	private Animator animator;
	private int item2Hash = Animator.StringToHash("StaffDelivered");
  private int allDeliveredHash = Animator.StringToHash("AllDelivered");

    void Start()
    {
        userNear = false;
        item1Obtain = false;
        item2Obtain = false;
        itemsBalloon.SetActive(false);
        item1Balloon.SetActive(false);
        item2Balloon.SetActive(false);
        confirmBalloon.SetActive(false);
        magicPotion.SetActive(false);
        container.SetActive(false);

        happyFace.SetActive(false);
        sadFace.SetActive(true);
        helperFace.SetActive(false);

		animator = gameObject.GetComponent<Animator> ();
    }

    void Update()
    {
        if(userNear)
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
                itemsBalloon.SetActive(false);
                item1Balloon.SetActive(false);
                item2Balloon.SetActive(false);
                container.SetActive(false);
                confirmBalloon.SetActive(false);
            }
    }

    override
    public void Interaction()
    {
        container.SetActive(true);
        if(!item1Obtain && !item2Obtain)
            itemsBalloon.SetActive(true);
        else
        {
            if (!item1Obtain)
                item1Balloon.SetActive(true);
            if (!item2Obtain)
                item2Balloon.SetActive(true);
        }
        if (item1Obtain && item2Obtain)
            confirmBalloon.SetActive(true);
    }
    override
    public bool InteractionConditions()
    {
        return userNear;
    }

    override
    public void ObjectUse()
    {
        string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
        bool giveSomething = false;
        if (equippedObject.Equals(item1Needs))
        {//User give item 1 that mage needs
            item1Obtain = true;
            giveSomething = true;
            Debug.Log("Dentro");
            if (item1Needs != "")
            {
                Debug.Log("Dentro 2");
                ManageInventory.instance.RemoveItem(item1Needs);
                container.SetActive(true);
                itemsBalloon.SetActive(false);
                item2Balloon.SetActive(true);
                item1Object.SetActive (true);
            }

        }
        if (equippedObject.Equals(item2Needs))
        {//User give item 2 that mage needs
            item2Obtain = true;
            giveSomething = true;
            if (item2Needs != "")
            {
                ManageInventory.instance.RemoveItem(item2Needs);
                container.SetActive(true);
                itemsBalloon.SetActive(false);
                item1Balloon.SetActive(true);
                item2Object.SetActive (true);
				animator.SetTrigger (item2Hash);
            }
        }
        if (item1Obtain && item2Obtain && giveSomething)
        {//Player gives all 2 items the mage needs
            sadFace.SetActive(false);
            happyFace.SetActive(true);
            container.SetActive(false);
            itemsBalloon.SetActive(false);
            item1Balloon.SetActive(false);
            item2Balloon.SetActive(false);
            animator.SetTrigger(allDeliveredHash);
        }
    }
    override
    public bool UseObjectConditions()
    {
        string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
        if (userNear && (equippedObject.Equals(item1Needs) || equippedObject.Equals(item2Needs)))
            return true;
        if (!equippedObject.Equals("") && userNear)
            ManageHelper.instance.WrongUseObject(equippedObject);
        return false;
    }

    public void ActivatePotionBalloon() {
      if(userNear) {
        container.SetActive(true);
        confirmBalloon.SetActive(true);
      }
    }
}
