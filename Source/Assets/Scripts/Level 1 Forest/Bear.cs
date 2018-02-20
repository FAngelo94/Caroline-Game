using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : InteractAnimal
{
    [Header("How many seconds cry lasts")]
    [Range(1, 5)]
    public float secondsCry = 2;
	[Header("Homw many seconds between two cryies")]
	[Range(1,5)]
	public float secondsPause = 2;

    [Header("Balloon to Show when bear sleep")]
    public GameObject sleepBalloon;

    [Header("Balloon to show when hear some sound, but not enough")]
    public GameObject wakeUpBalloon;

	[Header("Ballon that contains all the others")]
	public GameObject container;

    [Header("Item need to interact with this object")]
    public string itemNeeds = "";

    [Header("Smilies")]
    public GameObject happyFace;
    public GameObject sadFace;
    public GameObject helperFace;

    public float timeBeforeHornSound;



    /// <summary>
    /// When birds are singing this variable true, if player sing when this variable
    /// is true the bear wake up
    /// </summary>
    private bool birdsSinging;
    private bool bearSleep;
    private bool userNear;

	private Animator animator;
	private int wakeUpHash = Animator.StringToHash("Awaken");

    private GameObject bird1;
    private GameObject bird2;

	private AudioSource _source;

    void Start()
    {
	    _source = GetComponent<AudioSource>();
        wakeUpBalloon.SetActive(false);
        sleepBalloon.SetActive(false);
        container.SetActive(false);
        userNear = false;
        birdsSinging = false;
        bearSleep = true;

		animator = gameObject.GetComponent<Animator> ();

        happyFace.SetActive(false);
        sadFace.SetActive(false);
        helperFace.SetActive(false);
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
        if (collision.name.Equals("CoupleBird"))
        {
            Debug.Log("Uccelli Arrivati");
            collision.GetComponent<CircleCollider2D>().enabled = false;
            bird1 = GameObject.Find("Bird1");
            bird2 = collision.gameObject;

            StartCoroutine(BirdsSinging(bird1));
            StartCoroutine(BirdsSinging(bird2));
        }

    }
    IEnumerator BirdsSinging(GameObject bird)
    {
        yield return new WaitForSeconds(secondsPause);
		bird.GetComponent<Animator>().SetTrigger("Cry");
        birdsSinging = true;
	    yield return new WaitForSeconds(timeBeforeHornSound);
	    bird2.GetComponent<CoupleBirds>().PlaySound();
        yield return new WaitForSeconds(secondsCry);
		bird.GetComponent<Animator>().SetTrigger("Stop");
        birdsSinging = false;
        if(bearSleep)
            StartCoroutine(BirdsSinging(bird));
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
			container.SetActive (false);
            sleepBalloon.SetActive(false);
            wakeUpBalloon.SetActive(false);
            userNear = false;
        }
    }

    override
    public void Interaction()
    {

        if (bearSleep)
        {
            container.SetActive(true);
            sleepBalloon.SetActive(true);
            _source.Play();
        }
        else
        {
            container.SetActive(true);
            wakeUpBalloon.SetActive(true);
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
        container.SetActive(true);
        sleepBalloon.SetActive(false);
		wakeUpBalloon.SetActive (true);
        bearSleep = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Debug.Log("Bear wakes up");
		animator.SetBool (wakeUpHash, true);

        happyFace.SetActive(true);
    }
    override
    public bool UseObjectConditions()
    {
        string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
		Debug.Log (itemNeeds + "-" + userNear.ToString() + "-" + birdsSinging.ToString());

		if (!bearSleep)
			return false;

		if (equippedObject.Equals (itemNeeds)) {
			if (userNear && birdsSinging)
				return true;
			else {
				container.SetActive (true);
				sleepBalloon.SetActive (true);
			}
		}
        else
        {
            if (!equippedObject.Equals("") && userNear)
                ManageHelper.instance.WrongUseObject(equippedObject);
        }

        return false;
    }
}
