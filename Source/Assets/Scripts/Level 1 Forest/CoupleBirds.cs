using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoupleBirds : InteractAnimal
{
    [Header("Balloon to Show for item used")]
    public GameObject sadBalloon;

    [Header("Balloon to Show for item used")]
    public GameObject happyBalloon;

    [Header("Position goal after interaction")]
    public GameObject goal;

	[Header("Ballon that contains all the others")]
	public GameObject container;

    [Header("Item need to interact with this object")]
    public string itemNeeds = "";

    [Header("Speed of X")]
    [Range(0.5f, 3)]
    public float speedX = 1;

    [Header("Speed of Y")]
    [Range(0.5f, 3)]
    public float speedY = 1;

    [Header("Smilies")]
    public GameObject happyFace;
    public GameObject sadFace;
    public GameObject helperFace;

    private bool userNear;

    private bool mateArrived;

    private bool fly;

    private Rigidbody2D birdBody;

    private Animator animator;
    private int nextHash = Animator.StringToHash("Next");
    private int cryHash = Animator.StringToHash("Cry");

    private AudioSource _source;

    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.loop = false;
        sadBalloon.SetActive(false);
        happyBalloon.SetActive(false);
        container.SetActive(false);
        userNear = false;
        fly = false;
        mateArrived = false;
        birdBody = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        happyFace.SetActive(false);
        sadFace.SetActive(true);
        helperFace.SetActive(false);
    }

    void Update()
    {
        if(userNear)
            StartCoroutine(ManageBalloon());
        if (fly)
        {
            if (Mathf.Abs(birdBody.transform.position.x - goal.transform.position.x) < 0.1)
                birdBody.velocity = new Vector2(0, birdBody.velocity.y);
            if (Mathf.Abs(birdBody.transform.position.y - goal.transform.position.y) < 0.1)
                birdBody.velocity = new Vector2(birdBody.velocity.x, 0);
            if (birdBody.velocity.x == 0 && birdBody.velocity.y == 0)
            {
                fly = false;
                helperFace.SetActive(true);
                happyFace.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !fly)
        {
            userNear = true;
        }
        if (collision.name.Equals("Bird1"))
        {
            collision.GetComponent<CircleCollider2D>().enabled=false;
            collision.GetComponent<Animator>().SetTrigger("Stop");
            collision.transform.parent = gameObject.transform;
            StartCoroutine(WaitBirdStop());

            sadBalloon.SetActive(false);
            if (userNear)
                happyBalloon.SetActive(true);

            sadFace.SetActive(false);
            helperFace.SetActive(true);
        }
    }
    IEnumerator WaitBirdStop()
    {
        yield return new WaitForSeconds(2);
        mateArrived = true;
        animator.SetTrigger(nextHash);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
			container.SetActive (false);
            if (sadBalloon != null)
                sadBalloon.SetActive(false);
            if (happyBalloon != null)
                happyBalloon.SetActive(false);
            userNear = false;
        }
    }

    override
    public void Interaction()
    {
		container.SetActive (true);

        if (!mateArrived && fly==false)
        {
            sadBalloon.SetActive(true);
            animator.SetTrigger(cryHash);
        }

        else
            happyBalloon.SetActive(true);
    }
    override
    public bool InteractionConditions()
    {
        return userNear ;
    }

    override
    public void ObjectUse()
    {
		container.SetActive (false);
        sadBalloon.SetActive(false);
        happyBalloon.SetActive(false);

        int px = transform.position.x < goal.transform.position.x ? 1 : -1;
        int py = transform.position.y < goal.transform.position.y ? 1 : -1;
        birdBody.velocity = new Vector2(speedX * px, speedY * py);
        fly = true;

        helperFace.SetActive(false);
        happyFace.SetActive(true);
    }
    override
    public bool UseObjectConditions()
    {
        string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
        if (equippedObject.Equals(itemNeeds) && mateArrived==true && userNear==true)
            return true;
        if (!equippedObject.Equals("") && userNear)
            ManageHelper.instance.WrongUseObject(equippedObject);
        return false;
    }

    public void PlaySound()
    {
        _source.Play();
    }

}
