using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementsAnima: MonoBehaviour {

    public float jumpForce = 200;
    public float jumpProgressionForce;

    public float speed = 5f;
    // position used to check if the player is grounded
    public Transform groundcheck;
    // dimensions of the box to check if the player is grounded
    public float groundRadius;

    [Header("Questin mark on top of the head")]
    public SpriteRenderer questionMark;

    private Rigidbody2D rb;

    public bool grounded {get; private set;}
    private bool facingRight = true;
    private Animator animator;
	private int velocityXHash = Animator.StringToHash("VelocityX");
	private int velocityYHash = Animator.StringToHash("VelocityY");
    private int groundedHash = Animator.StringToHash("Grounded");


    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // keep the player on his feet
        rb.freezeRotation = true;
	}

    private void Update()
    {
        if (GameManager.instance.GamePaused)
            return;

        grounded = Physics2D.OverlapCircle(groundcheck.position, groundRadius,
            layerMask: 1 << LayerMask.NameToLayer("Ground"));

        animator.SetBool(groundedHash, grounded);

        if (Input.GetButtonDown("Jump") && grounded)
            Jump();

        if (Input.GetButton("Jump") && !grounded && rb.velocity.y > 0)
        {
            rb.AddForce(Vector2.up * jumpProgressionForce);
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.GamePaused)
            return;

        move();

        animator.SetFloat(velocityYHash, rb.velocity.y);
    }

	private void move()
	{
		float h = Input.GetAxis("Horizontal");
		rb.velocity = new Vector2(speed * h, rb.velocity.y);
        animator.SetFloat(velocityXHash, Mathf.Abs(h));

		if (h > 0 && !facingRight)
			Flip();
		else if (h < 0 && facingRight)
			Flip();
	}

    private void Flip()
    {
        questionMark.flipX = !questionMark.flipX;
        facingRight = !facingRight;
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
