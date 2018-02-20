using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : InteractAnimal {

    [Header("Position goal after interaction")]
    public GameObject goal;

    [Header("Player object")]
    public GameObject player;

    [Header("Item need to interact with this object")]
    public string itemNeeds = "";

    [Header("Speed of X")]
    [Range(0.5f, 10)]
    public float speedX = 8;

    [Header("Speed of Y")]
    [Range(0.5f, 10)]
    public float speedY = 8;

    private FollowAnimal follow;

    private bool userNear;

    private bool fly;
    private bool followPlayer;

    private Rigidbody2D birdBody;

    private Animator animator;
    private int flyingHash = Animator.StringToHash("Flying");
    private int stopHash = Animator.StringToHash("Stop");
    private int calledHash = Animator.StringToHash("Called");

    private AudioSource _source;

    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.loop = false;
        userNear = false;
        fly = false;
        followPlayer = false;
        birdBody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        player = GameManager.instance.GetPlayer();
    }

    void Update()
    {
		StartCoroutine(ManageBalloon());
        if(fly)
        {
            if (Mathf.Abs(birdBody.transform.position.x - goal.transform.position.x) < 0.1)
                birdBody.velocity = new Vector2(0, birdBody.velocity.y);
            if (Mathf.Abs(birdBody.transform.position.y - goal.transform.position.y) < 0.1)
                birdBody.velocity = new Vector2(birdBody.velocity.x, 0);
            if (birdBody.velocity.x == 0 && birdBody.velocity.y == 0)
            {
                fly = false;
                enabled = false;
            }
        }
        if(followPlayer)
        {
            follow.Follow(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" )
        {
            userNear = true;
        }
        if (collision.name.Equals("Miguel"))
        {
            animator.SetTrigger(stopHash);
            fly = false;
        }
        if(collision.name.Equals("CoupleBird"))
        {
            int px = transform.position.x < goal.transform.position.x ? 1 : -1;
            int py = transform.position.y < goal.transform.position.y ? 1 : -1;
            birdBody.velocity = new Vector2(speedX * px, speedY * py);
            gameObject.tag = "Untagged";
            fly = true;
            followPlayer = false;
            animator.SetTrigger(stopHash);
        }
        Debug.Log(collision.name);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            userNear = false;
        }
    }

    override
    public void Interaction() {

    }
    override
    public bool InteractionConditions()
    {
		return userNear;
    }

    override
    public void ObjectUse(){
        gameObject.tag = "Untagged";
        follow = new FollowAnimal();
        follow.Initialize(gameObject);
        follow.goal = player;
        follow.speedX = speedX;
        follow.speedY = speedY;
        followPlayer = true;
        animator.SetTrigger(calledHash);
        gameObject.GetComponent<CircleCollider2D>().radius = 2;
    }
    override
    public bool UseObjectConditions()
    {
        string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
        if (equippedObject.Equals(itemNeeds) && userNear==true)
            return true;
        return false;
    }

    public void PlaySound()
    {
        _source.Play();
    }

}
