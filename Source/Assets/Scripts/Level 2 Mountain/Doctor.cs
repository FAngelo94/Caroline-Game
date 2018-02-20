using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doctor : InteractAnimal
{
    [Header("Balloon for ask fire")]
    public GameObject fireBalloon;
    [Header("Balloon for ask the water")]
    public GameObject waterBalloon;
    [Header("Balloon for ask flower")]
    public GameObject flowerBalloon;
    [Header("Balloon for turnip")]
    public GameObject turnipBalloon;
    [Header("Balloon for medicine is ready")]
    public GameObject medicineBalloon;
    [Header("Container for balloon")]
    public GameObject container;

    [Header("Name flower item")]
    public string flower = "";
    private bool flowerGiven;
    [Header("Name turnip item")]
    public string turnip = "";
    private bool turnipGiven;

    [Header("Fire of caudron")]
    public GameObject fire;
    [Header("Water of caudron")]
    public GameObject water;
    [Header("Medicine in the caudron")]
    public GameObject medicine;
    private bool medicineReady;

    [Header("Smilies")]
    public GameObject happyFace;
    public GameObject sadFace;
    public GameObject helperFace;

    [Header("Empty jar that appear")]
    public GameObject jar;
    private bool jarAppear;

    private bool userNear;

    private Animator animator;

    private int givePotionHash = Animator.StringToHash("GivePotion");
    private int cookHash= Animator.StringToHash("Cook");

    void Start()
    {
        userNear = false;
        fireBalloon.SetActive(false);
        waterBalloon.SetActive(false);
        flowerBalloon.SetActive(false);
        turnipBalloon.SetActive(false);
        medicineBalloon.SetActive(false);
        container.SetActive(false);

        happyFace.SetActive(false);
        sadFace.SetActive(true);
        helperFace.SetActive(false);

        jarAppear = false;
        jar.SetActive(false);
        turnipGiven = false;
        flowerGiven = false;
        medicineReady = false;

        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (userNear)
            StartCoroutine(ManageBalloon());
        if(!jarAppear && fire.activeSelf)
        {
            jarAppear = true;
            //jar.SetActive(true);
            animator.SetTrigger(givePotionHash);
        }
        if(flowerGiven && turnipGiven && water.activeSelf)
        {
            sadFace.SetActive(false);
            helperFace.SetActive(true);
            medicineReady = true;
        }
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
            fireBalloon.SetActive(false);
            waterBalloon.SetActive(false);
            flowerBalloon.SetActive(false);
            turnipBalloon.SetActive(false);
            medicineBalloon.SetActive(false);
            container.SetActive(false);
        }
    }

    override
    public void Interaction()
    {
        container.SetActive(true);
        if(!fire.activeSelf)
        {
            fireBalloon.SetActive(true);
        }
        else if(!medicineReady)
        {
            if (!flowerGiven)
                flowerBalloon.SetActive(true);
            if (!turnipGiven)
                turnipBalloon.SetActive(true);
            if (!water.activeSelf)
                waterBalloon.SetActive(true);
        }
        else
        {
            animator.SetTrigger(cookHash);
            water.SetActive(false);
            medicine.SetActive(true);
            medicineBalloon.SetActive(true);
            helperFace.SetActive(false);
            happyFace.SetActive(true);
        }

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
        if (equippedObject.Equals(flower))
        {//User give item 1 that doctor needs
            flowerGiven = true;
            if (flower != "")
            {
                ManageInventory.instance.RemoveItem(flower);                
                foreach (Transform child in transform)
                {
                    if (child.name.Equals(flower))
                        child.gameObject.SetActive(true);
                }
            }
            if (fire.activeSelf)
            {
                flowerBalloon.SetActive(false);
                waterBalloon.SetActive(false);
                turnipBalloon.SetActive(false);
                if (!flowerGiven)
                    flowerBalloon.SetActive(true);
                if (!turnipGiven)
                    turnipBalloon.SetActive(true);
                if (!water.activeSelf)
                    waterBalloon.SetActive(true);
            }
        }
        if (equippedObject.Equals(turnip))
        {//User give item 2 that doctor needs
            turnipGiven = true;
            if (turnip != "")
            {
                ManageInventory.instance.RemoveItem(turnip);
                foreach (Transform child in transform)
                {
                    if (child.name.Equals(turnip))
                        child.gameObject.SetActive(true);
                }
            }
            if (fire.activeSelf)
            {
                flowerBalloon.SetActive(false);
                waterBalloon.SetActive(false);
                turnipBalloon.SetActive(false);
                if (!flowerGiven)
                    flowerBalloon.SetActive(true);
                if (!turnipGiven)
                    turnipBalloon.SetActive(true);
                if (!water.activeSelf)
                    waterBalloon.SetActive(true);
            }
        }
    }
    override
    public bool UseObjectConditions()
    {
        string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
        Debug.Log(equippedObject);
        if (userNear && !equippedObject.Equals("") && (equippedObject.Equals(flower) || equippedObject.Equals(turnip)))
            return true;
        if (!equippedObject.Equals("") && userNear)
            ManageHelper.instance.WrongUseObject(equippedObject);
        return false;
    }

    public void ActivateJarInAnimation() {
      jar.SetActive(true);
    }
}
