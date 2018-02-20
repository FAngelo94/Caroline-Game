using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour {

    [Header("Balloon to Show")]
    public GameObject balloon;

    [Header("Name item have in inventary")]
    public string iconName;

    private bool playerNear;

    // Use this for initialization
    void Start () {
        playerNear = false;
        if(balloon!=null)
            balloon.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Interact") && playerNear==true && Time.timeScale!=0)
        {
            SoundManager.instance.PickedUp();
            ManageInventory.instance.AddItem(iconName);
            gameObject.SetActive(false);
        }
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(balloon!=null)
                balloon.SetActive(true);
            playerNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(balloon!=null)
                balloon.SetActive(false);
            playerNear = false;
        }
    }
}
