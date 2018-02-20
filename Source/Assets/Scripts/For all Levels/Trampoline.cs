using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour {

    public float liftSpeed;

    private bool onTop = false;

    private Rigidbody2D playerRB;
    private Animator animator;
    private int triggerHash = Animator.StringToHash("Trigger");

    private void Awake()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (onTop)
        {
            Lift();
            onTop = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if(other.tag == "Player")
          onTop = true;
    }

    private void OnTriggerExit2D()
    {
        onTop = false;
    }

    private void Lift()
    {
        playerRB.velocity = new Vector2(playerRB.velocity.x, liftSpeed);
        animator.SetTrigger(triggerHash);
    }
}
