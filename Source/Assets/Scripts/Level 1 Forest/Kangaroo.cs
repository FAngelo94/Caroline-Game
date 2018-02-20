using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kangaroo : InteractAnimal
{
    [Header("Balloon to Show before catch the boomerang")]
    public GameObject beforeCatch;
    [Header("Balloon to Show after catch the boomerang")]
    public GameObject afterCatch;

	[Header("Ballon that contains all the others")]
	public GameObject container;

    [Header("Boomerang object")]
    public GameObject boomerang;
    private Animator boomerangAnimator;

    [Header("Smilies")]
    public GameObject happyFace;
    public GameObject sadFace;
    public GameObject helperFace;

    private bool userNear = false;

    private string boomerangDirection;

    private Animator animator;
	//private int flyHash = Animator.StringToHash("Fly");
	private int catchedHash = Animator.StringToHash("Catched");
  private bool catched = false;

    void Start()
    {
        beforeCatch.SetActive(false);
        afterCatch.SetActive(false);
        //boomerangAnimator = boomerang.GetComponent<Animator>();
        animator = gameObject.GetComponent<Animator> ();

        afterCatch.SetActive(false);
        beforeCatch.SetActive(false);
        container.SetActive(false);

        happyFace.SetActive(false);
        sadFace.SetActive(false);
        helperFace.SetActive(true);
    }

    void Update()
    {
        if(userNear)
            StartCoroutine(ManageBalloon());
        if (!boomerang.activeSelf && !catched)
        {
            animator.SetTrigger(catchedHash);
            catched = true;
            //boomerangAnimator.SetTrigger(flyHash);
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
			container.SetActive (false);
            beforeCatch.SetActive(false);
        }
    }

    override
    public void Interaction()
    {
        container.SetActive(true);


        if (!boomerang.activeSelf)
        {
            happyFace.SetActive(true);
            helperFace.SetActive(false);
            afterCatch.SetActive(true);
        }
        else
        {
            beforeCatch.SetActive(true);//boomerang is active
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

    }
    override
    public bool UseObjectConditions()
    {
        string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
        if (!equippedObject.Equals("") && userNear)
            ManageHelper.instance.WrongUseObject(equippedObject);
        return false;
    }
}
