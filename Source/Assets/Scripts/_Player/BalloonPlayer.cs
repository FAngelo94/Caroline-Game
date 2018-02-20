using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages the helper balloon that appear in the head of player.
/// It mast be assigned to the player.
/// </summary>
public class BalloonPlayer : MonoBehaviour
{
    [Header("Balloon for Interact")]
    public GameObject BalloonInteract;
	[Header("Balloon for Use an object")]
	public GameObject BalloonUse;
    [Header("Balloon for when the player does something that does not work")]
    public GameObject BalloonWontWork;

	public Sprite keyboard_X;
	public Sprite keyboard_C;
	public Sprite joystick_X;
	public Sprite joystick_Y;

	public float showTime;

    private void Start()
    {
        DeactivateBaloons();
		if (Input.GetJoystickNames ().Length == 0) {
			BalloonUse.GetComponent<SpriteRenderer> ().sprite = keyboard_C;
			BalloonInteract.GetComponent<SpriteRenderer> ().sprite = keyboard_X;
		} else {
			BalloonUse.GetComponent<SpriteRenderer> ().sprite = joystick_Y;
			BalloonInteract.GetComponent<SpriteRenderer> ().sprite = joystick_X;
		}
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag.Equals ("Item") || collision.tag.Equals("BigItem")) {

			BalloonInteract.SetActive (true);
		} else if (collision.tag.Equals ("Creature")) {
			
			string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();

			BalloonInteract.SetActive (true);

			if (!equippedObject.Equals ("")) 
				BalloonUse.SetActive (true);
		}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
		if(!collision.tag.Equals("Ladder"))
			DeactivateBaloons ();
    }

	public void DeactivateBaloons ()
	{
		BalloonInteract.SetActive (false);
		BalloonUse.SetActive (false);
        BalloonWontWork.SetActive(false);
	}

    internal IEnumerator ShowWontWork()
    {
        BalloonWontWork.SetActive(true);
	    SoundManager.instance.Wrong();
		yield return new WaitForSeconds(showTime);
        BalloonWontWork.SetActive(false);
    }
}

