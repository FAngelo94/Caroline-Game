using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObjects : MonoBehaviour
{

    [Header("Balloon to Show to Interact")]
    public GameObject balloon;

    [Header("Balloon to Show for item used")]
    public GameObject balloonItem;

    [Header("Item need to interact with thi object")]
    public string itemNeeds="";

    /// <summary>
    /// Player activate the trigger and have the necessary condition to activate
    /// the interaction
    /// </summary>
    private bool playerCanActive;

    /// <summary>
    /// True if player activates the enter trigger
    /// </summary>
    private bool playerInsideTrigger;

    private bool activeInteraction;

	private GameObject playerReference;

    // Use this for initialization
    void Start()
    {
        playerCanActive = false;
        activeInteraction = false;
        playerInsideTrigger = false;
        balloon.SetActive(false);
        balloonItem.SetActive(false);
    }

	void Awake()
	{
		playerReference = GameObject.FindGameObjectWithTag ("Player");
	}


    // Update is called once per frame
    void Update()
    {
        Debug.Log("Inter");
        Debug.Log(Input.inputString);
        //Manage interaction
        if (Input.GetButtonDown("Interact") && playerInsideTrigger == true && Time.timeScale != 0)
        {
            Debug.Log("Interacted");
            balloon.SetActive(false);
            balloonItem.SetActive(true);
			playerReference.gameObject.GetComponent<BalloonPlayer> ().DeactivateBaloons ();
        }

        //Manage object use
        if (Input.GetButtonDown("Use") && playerCanActive == true && Time.timeScale != 0)
        {
            Debug.Log("Used");
            string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
            if (equippedObject.Equals(itemNeeds))
            {
                interactWithObject();
                activeInteraction = true;
            }
        }
        if (activeInteraction == true)
            doInteraction();
    }

    public void destroyBalloon()
    {
        balloon.SetActive(false);
    }

    public void destroyItemBallon()
    {
        balloonItem.SetActive(false);
    }

    /// <summary>
    /// Stop the repeated interaction
    /// </summary>
    public void stopInteraction()
    {
        activeInteraction = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(balloon!=null)
                balloon.SetActive(true);
            playerCanActive = true;
            playerInsideTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(balloon!=null)
                balloon.SetActive(false);
            if (balloonItem != null)
                balloonItem.SetActive(false);
            playerCanActive = false;
        }
    }

    /// <summary>
    /// Use this method to modify the variable playerCanActive
    /// </summary>
    /// <param name="b"></param>
    public void setPlayerCanActive(bool b) { playerCanActive = b; }

    //PRIVATE METHODS

    /// <summary>
    ///  Implement the interaction need to do with this object
    /// </summary>
    virtual public void interactWithObject()
    {
    }

    /// <summary>
    /// The iteraction the object (in this case bird) must do until it reaches the goal
    /// </summary>
    virtual public void doInteraction() { }
}

