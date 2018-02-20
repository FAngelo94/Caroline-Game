using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsReturn : InteractAnimal
{
    public Animator parentAnimator;
    [Header("Position where is the item")]
    public GameObject goalItem;
    [Header("Position where bird must bring the item")]
    public GameObject goalNearUser;
    [Header("Position where is the tree")]
    public GameObject goalNearTree;


    [Header("Item that appear when bird go to user")]
    public GameObject item;
    [Header("Item that disappear when bird take it")]
    public GameObject fakeItem;

    [Header("Item need to interact with this object")]
    public string itemNeeds = "";

    [Header("Speed of X")]
    [Range(0.5f, 20)]
    public float speedX = 15;

    [Header("Speed of Y")]
    [Range(0.5f, 20)]
    public float speedY = 15;

    [Header("Smilies")]
    public GameObject happyFace;
    public GameObject sadFace;
    public GameObject helperFace;

    private bool userNear;

    //This booleans identify in which position is the bird
    private bool startPosition;
    private bool nearUserPosition;

    //check if bird is fling
    private bool flyToUser;
    private bool flyToItem;
    private bool flyToTree;

    //check if bird take the item
    private bool itemTaken;

    private Rigidbody2D birdBody;

    private int doingSomethingHash = Animator.StringToHash("DoingSomething");

    void Start()
    {
        userNear = false;
        startPosition = true;
        nearUserPosition = false;
        flyToUser = false;
        flyToItem = false;
        itemTaken = false;
        birdBody = gameObject.GetComponent<Rigidbody2D>();

        fakeItem.SetActive(true);
        item.SetActive(false);
        happyFace.SetActive(false);
        sadFace.SetActive(false);
        helperFace.SetActive(false);
    }

    void Update()
    {
        if (userNear)
            StartCoroutine(ManageBalloon());
        if(flyToUser)
        {
            if (Mathf.Abs(birdBody.transform.position.x - goalNearUser.transform.position.x) < 0.1)
                birdBody.velocity = new Vector2(0, birdBody.velocity.y);
            if (Mathf.Abs(birdBody.transform.position.y - goalNearUser.transform.position.y) < 0.1)
                birdBody.velocity = new Vector2(birdBody.velocity.x, 0);
            if (birdBody.velocity.x == 0 && birdBody.velocity.y == 0)
            {
                flyToUser = false;

                nearUserPosition = true;
                if(itemTaken)
                {
                    item.SetActive(true);
                    flyToTree = true;
                    FlyToGoal(goalNearTree);
                }
                else
                    helperFace.SetActive(true);
            }
        }
        if (flyToItem)
        {
            if (Mathf.Abs(birdBody.transform.position.x - goalItem.transform.position.x) < 0.1)
                birdBody.velocity = new Vector2(0, birdBody.velocity.y);
            if (Mathf.Abs(birdBody.transform.position.y - goalItem.transform.position.y) < 0.1)
                birdBody.velocity = new Vector2(birdBody.velocity.x, 0);
            if (birdBody.velocity.x == 0 && birdBody.velocity.y == 0)
            {
                flyToItem = false;
                FlyToGoal(goalNearUser);
                flyToUser = true;
                Flip();
                itemTaken = true;
                fakeItem.SetActive(false);
            }
        }
        if (flyToTree)
        {
            if (Mathf.Abs(birdBody.transform.position.x - goalNearTree.transform.position.x) < 0.1)
                birdBody.velocity = new Vector2(0, birdBody.velocity.y);
            if (Mathf.Abs(birdBody.transform.position.y - goalNearTree.transform.position.y) < 0.1)
                birdBody.velocity = new Vector2(birdBody.velocity.x, 0);
            if (birdBody.velocity.x == 0 && birdBody.velocity.y == 0)
            {
                flyToTree = false;
            }
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
        if(flyToItem==false && flyToUser==false)
        {
            if(startPosition)
            {
                FlyToGoal(goalNearUser);
                startPosition = false;
                flyToUser = true;
                parentAnimator.SetBool(doingSomethingHash, true);
            }
            if(nearUserPosition)
            {
                FlyToGoal(goalItem);
                nearUserPosition = false;
                flyToItem = true;
                helperFace.SetActive(false);
            }
        }
    }
    override
    public bool UseObjectConditions()
    {
        string equippedObject = ManageInventory.instance.ReturnNameEquippedItem();
        if (userNear && equippedObject.Equals(itemNeeds) && itemTaken==false)
            return true;
        if (!equippedObject.Equals("") && userNear)
            ManageHelper.instance.WrongUseObject(equippedObject);
        return false;
    }

    private void FlyToGoal(GameObject goal)
    {
        int px = transform.position.x < goal.transform.position.x ? 1 : -1;
        int py = transform.position.y < goal.transform.position.y ? 1 : -1;
        birdBody.velocity = new Vector2(speedX * px, speedY * py);
    }

    private void Flip()
    {
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
