using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [Header("Goal for boomerang in front of Caroline")]
    public GameObject goal;

    [Header("Hand of Caroline")]
    public GameObject hand;

    [Header("Speed of boomerang")]
    [Range(3,10)]
    public float speed = 5;

	public float throwDuration = 0.333f;

    private float progresSpeed;

	private bool goToGoal = false;
    private bool goToCaroline = false;
    private Rigidbody2D body;
    private Transform originalParent;
    private Vector2 goalPosition;
	private Animator animator;
	private Animator carolineAnimator;
	private int flyHash = Animator.StringToHash ("Fly");
	private int throwHash = Animator.StringToHash ("Throw");

	private bool flying = false;

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        originalParent = gameObject.transform.parent;
		animator = gameObject.GetComponent<Animator> ();
    }

	void Awake()
	{
		carolineAnimator = GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ();
	}

	void Update()
	{
		if (flying) {
			UseBoomerangPhysic ();
		} else if(Input.GetButtonDown ("Use")) {
			StartCoroutine (ThrowSequence());
		}
	}

	private IEnumerator ThrowSequence() {
		carolineAnimator.SetTrigger (throwHash);
		yield return new WaitForSeconds (throwDuration);
		UseBoomerang ();
		flying = true;
		animator.SetBool (flyHash, true);
	}

    private void UseBoomerang()
    {
        goToGoal = true;
        goalPosition = new Vector2(goal.transform.position.x, goal.transform.position.y);
        gameObject.transform.parent=null;
        int direction = goalPosition.x > gameObject.transform.position.x ? 1 : -1;
        body.velocity = new Vector2(direction*speed, 0);
        progresSpeed = speed;
    }

    private void UseBoomerangPhysic()
    {
        if (goToGoal)
        {
            if(Mathf.Abs(gameObject.transform.position.x-goalPosition.x)<0.5f)
            {
                body.velocity = new Vector2(0, 0);
                goToGoal = false;
                goToCaroline = true;
            }
        }
        if (goToCaroline)
        {
            int dirX = hand.transform.position.x > gameObject.transform.position.x ? 1 : -1;
            int dirY = hand.transform.position.y > gameObject.transform.position.y ? 1 : -1;
            if (Mathf.Abs(gameObject.transform.position.x - hand.transform.position.x) < 0.05f)
            {
                body.velocity = new Vector2(0, body.velocity.y);
            }
            else
                body.velocity = new Vector2(progresSpeed * dirX, body.velocity.y);
            if (Mathf.Abs(gameObject.transform.position.y - hand.transform.position.y) < 0.05f)
            {
                body.velocity = new Vector2(body.velocity.x, 0);
            }
            else
                body.velocity = new Vector2(body.velocity.x, progresSpeed * dirY);
            //decrement speed when Boomerang return near
            if (Mathf.Abs(gameObject.transform.position.x - hand.transform.position.x) < 1f && Mathf.Abs(gameObject.transform.position.y - hand.transform.position.y) < 1f)
                progresSpeed = 5;
            else
                progresSpeed = speed;

            //stop boomerang
            if(body.velocity.x==0 && body.velocity.y==0)
            {
                goToCaroline = false;
                gameObject.transform.parent = originalParent;
				animator.SetBool (flyHash, false);
				flying = false;
            }
        }
    }
}
