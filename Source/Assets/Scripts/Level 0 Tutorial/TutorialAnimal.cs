using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimal : InteractAnimal
{
    [Header("Balloon to Show when animal is not hungry")]
    public GameObject notHungryBalloon;

    [Header("Balloon to Show when animal is hungry")]
    public GameObject hungryBalloon;

	[Header("Ballon that contains all the others")]
	public GameObject container;

  public GameObject containerPosition2;

    [Header("Item needed to interact with this object")]
    public string itemNeeds = "";

    [Header("Smilies")]
    public GameObject happyFace;
    public GameObject sadFace;
    public GameObject helperFace;

    private bool userNear;
    /// <summary>
    /// Become true when player feed animal
    /// </summary>
    private bool hungry;

    private CapsuleCollider2D capsule;
    private helpSequence sequenceScript;
    [Header("Arm of the creature")]
    public GameObject arm;

	private Animator animator;
    private int interactionHash = Animator.StringToHash("Interact");
    private int eatHash = Animator.StringToHash("Eat");

    void Start()
    {
        hungry = true;
        userNear = false;
		animator = gameObject.GetComponent<Animator> ();
        arm.GetComponent<CapsuleCollider2D>().enabled = false;

        sequenceScript = arm.GetComponent<helpSequence>();
        sequenceScript.enabled = false;
        capsule = arm.GetComponent<CapsuleCollider2D>();
        capsule.enabled = false;

        hungryBalloon.SetActive(false);
        notHungryBalloon.SetActive(false);
        container.SetActive(false);
        sadFace.SetActive(true);
        helperFace.SetActive(false);
        happyFace.SetActive(false);
    }

    void Update()
    {
        if(userNear)
           StartCoroutine(ManageBalloon());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            userNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            userNear = false;
            hungryBalloon.SetActive(false);
            notHungryBalloon.SetActive(false);
            container.SetActive(false);

        }
    }

    override
    public void Interaction()
    {
        if (hungry)
        {
            StartCoroutine(InteractionSequence());
        } else
        {
            notHungryBalloon.SetActive(true);
            container.SetActive(true);
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
        ManageInventory.instance.RemoveItem(itemNeeds);
        SoundManager.instance.Eat();
        arm.GetComponent<CapsuleCollider2D>().enabled = true;
        StartCoroutine(WaitForSound());
        container.transform.position = containerPosition2.transform.position;
    }

    IEnumerator WaitForSound()
    {
        yield return new WaitForSeconds(1);
        hungry = false;
        animator.SetTrigger(eatHash);
        happyFace.SetActive(true);
        sadFace.SetActive(false);
        hungryBalloon.SetActive(false);
        notHungryBalloon.SetActive(true);
        container.SetActive(true);
    }

    IEnumerator InteractionSequence()
    {
        animator.SetTrigger(interactionHash);
        yield return new WaitForSeconds(3f);
        if(userNear) {
          hungryBalloon.SetActive(true);
          container.SetActive(true);
        }
    }

    override
    public bool UseObjectConditions()
    {
        string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
        if (equippedObject.Equals(itemNeeds) && userNear == true)
            return true;
        if (!equippedObject.Equals("") && userNear)
            ManageHelper.instance.WrongUseObject(equippedObject);
        return false;
    }
}
