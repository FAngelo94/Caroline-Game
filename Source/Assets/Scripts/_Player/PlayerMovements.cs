using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements: MonoBehaviour {

    public float jumpForce = 200;
    public float jumpProgressionForce;

    public float speed = 5f;

    // dimensions of the box to check if the player is grounded
    public Vector2 groundBox;
    // state can be changed by other objects
    [HideInInspector] public String state = "";

    private Rigidbody2D rb;
    // position used to check if the player is grounded
    private Transform groundcheck;
    private bool grounded = true;

    private bool facingRight = true;
    // the object the player can interact with (works for ladder for now)
    private GameObject interacting = null;
    private GameManager gameManager;

    private Animator animator;
	private int velocityXHash = Animator.StringToHash("VelocityX");
	private int velocityYHash = Animator.StringToHash("VelocityY");
    private int groundedHash = Animator.StringToHash("Grounded");
    
    
    //for audio
    private AudioSource _source;
    public float timeToStep;
    private float elapsedTimeToStep;

    // Use this for initialization
    void Start ()
    {
        _source = GetComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.loop = false;
        
        gameManager = GameManager.instance;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // keep the player on his feet
        rb.freezeRotation = true;
        // state is used to check what the player can do
        state = "Idle";

        groundcheck = transform.Find("GroundCheck");
	}

    private void FixedUpdate()
    {
        if (gameManager.GamePaused)
            return;

        // basic movement management
        if(state == "Idle" || state == "CanClimb")
        {
            
			move ();

            if(Input.GetButton("Jump") && !grounded)
            {
                rb.AddForce(Vector2.up * jumpProgressionForce);
            }
        }

    }

	private void move()
	{
		float h = Input.GetAxis("Horizontal");

		rb.velocity = new Vector2(speed * h, rb.velocity.y);

        animator.SetFloat(velocityXHash, Mathf.Abs(h));

	    if (h != 0 && grounded)
	    {
	        if (elapsedTimeToStep <= 0)
	        {
	            elapsedTimeToStep = timeToStep;
			    _source.Play();
	        }
	        else
	            elapsedTimeToStep -= Time.deltaTime;
	    }
	    else
	    {
		    elapsedTimeToStep = 0;
		    _source.Stop();

	    }
	        
	    
		if (h > 0 && !facingRight)
			Flip();
		else if (h < 0 && facingRight)
			Flip();
	}

    private void Update()
    {
        if (gameManager.GamePaused)
            return;
        
        // the player can jump only if its grounded
        grounded = Physics2D.OverlapBox(groundcheck.position, groundBox, 0, layerMask: 1 << LayerMask.NameToLayer("Ground"));
        animator.SetBool(groundedHash, grounded);
        animator.SetFloat(velocityYHash, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && grounded)
            Jump();
        
        if (state == "CanClimb")
        {
            
            if (Input.GetButtonDown("Interact"))
            {
                state = "Climbing";
				// activate the layer for climbing
				animator.SetLayerWeight (1, 1);

                rb.isKinematic = true;
                rb.velocity = new Vector2(0, 0);
                if (grounded)
                    // if the ladder is over me, place me over the ground
                    if(interacting.transform.position.y > rb.position.y)
                        rb.position = new Vector2(interacting.transform.position.x, rb.position.y + 0.6f);
                    // otherwise place me down
                    else
                        rb.position = new Vector2(interacting.transform.position.x, rb.position.y - 1.1f);
                // this means that I'm jumping on the ladder while interacting
                else
                    rb.position = new Vector2(interacting.transform.position.x, rb.position.y);

            }
        }
        else if (state == "Climbing")
        {
            // end climbing
            if(grounded || Input.GetButtonDown("Back"))
            {
                state = "CanClimb";
				animator.SetLayerWeight (1, 0);
                rb.isKinematic = false;
            }
            // climb
            else
            {
                float v = Input.GetAxis("Vertical");
                rb.velocity = new Vector2(0, speed * v);
				animator.SetFloat(velocityYHash, rb.velocity.y);
            }
        }
    }

    private void Flip()
    {
        facingRight= !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ladder")
        {
            state = "CanClimb";
            interacting = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ladder")
        {
            state = "Idle";
			// deactivate the layer for climbing
			animator.SetLayerWeight (1, 0);
            rb.isKinematic = false;
            interacting = null;
        }
    }

}
