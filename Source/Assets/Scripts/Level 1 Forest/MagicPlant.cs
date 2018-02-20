using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPlant : InteractAnimal
{
	[Header("Magic Potion need to grow up")]
	public string magicPotion = "";

	private bool userNear;
	private int growHash = Animator.StringToHash("Grow");
	public BoxCollider2D fatherCollider;

	void Start()
	{
		userNear = false;
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
		gameObject.GetComponent<Animator>().SetBool(growHash, true);
		fatherCollider.enabled = true;
		ManageInventory.instance.RemoveItem(magicPotion);
		GameManager.instance.GamePaused = true;
		GameManager.instance.StopPlayer();
	}
	override
	public bool UseObjectConditions()
	{
		string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
		if (userNear && equippedObject.Equals(magicPotion))
			return true;
        if (!equippedObject.Equals("") && userNear)
            ManageHelper.instance.WrongUseObject(equippedObject);
        return false;
	}
}
