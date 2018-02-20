using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : InteractAnimal
{
    [Header("Balloon to Show the medicine he need")]
    public GameObject medicineBallon;
    [Header("Container for balloon")]
    public GameObject container;

    [Header("Name of medicine for drake")]
    public string medicine = "";
    private bool medicineGiven;

    [Header("Name of stick and firing stick")]
    public string stick;
    public string firingStick;
    [Header("Time needed to wait for fire to burn the stick")]
    public float delayTime = 1.0f;
    private bool stickTurnOn;

    [Header("Smilies")]
    public GameObject happyFace;
    public GameObject sadFace;
    public GameObject helperFace;

    private bool userNear;

    private Animator animator;
    private int breathHash = Animator.StringToHash("Breath");
    private int healedHash = Animator.StringToHash("Healed");

    void Start()
    {
        userNear = false;
        medicineGiven = false;
        stickTurnOn = false;

        medicineBallon.SetActive(false);
        container.SetActive(false);

        happyFace.SetActive(false);
        sadFace.SetActive(true);
        helperFace.SetActive(false);

        animator = gameObject.GetComponent<Animator>();
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
            medicineBallon.SetActive(false);
            container.SetActive(false);
        }
    }

    override
    public void Interaction()
    {

        if (medicineGiven)
        {
            container.SetActive(false);
            helperFace.SetActive(false);
            sadFace.SetActive(false);
            Debug.Log("Dragon Fly");
        }
        else
        {
            container.SetActive(true);
            medicineBallon.SetActive(true);
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
        if (equippedObject.Equals(medicine))
        {
            ManageInventory.instance.RemoveItem(medicine);
            sadFace.SetActive(false);
            helperFace.SetActive(true);
            medicineGiven = true;
            animator.SetTrigger(healedHash);
            StartCoroutine(EndTheLevel());
        }
        if (equippedObject.Equals(stick))
        {
            animator.SetTrigger(breathHash);
            StartCoroutine(SynchActionsWithFire(delayTime));
        }
    }
    override
    public bool UseObjectConditions()
    {
        string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
        if (userNear && (equippedObject.Equals(medicine) || equippedObject.Equals(stick)))
            return true;
        if (!equippedObject.Equals("") && userNear)
            ManageHelper.instance.WrongUseObject(equippedObject);
        return false;
    }

    private IEnumerator SynchActionsWithFire(float delay) {
        yield return new WaitForSeconds(delay);
        ManageInventory.instance.RemoveItem(stick);
        ManageInventory.instance.AddItem(firingStick);
        ManageInventory.instance.EquippedLastItemPicked();
        sadFace.SetActive(true);
        helperFace.SetActive(false);
        medicineBallon.SetActive(true);
        stickTurnOn = true;
    }

    private IEnumerator EndTheLevel() {
      yield return new WaitForSeconds(3f);
      GameManager.instance.changeScene("GameEnd1",null);
    }
}
