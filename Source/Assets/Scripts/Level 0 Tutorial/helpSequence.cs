	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helpSequence : MonoBehaviour {

	public float liftSpeed;
    public GameObject johnny;
	private Animator animator;
	private int liftHash = Animator.StringToHash("Lift");
	// Use this for initialization
	void Awake () {
		animator = johnny.GetComponent<Animator> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			animator.SetTrigger (liftHash);
			Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D> ();
			playerRb.velocity = Vector2.up * liftSpeed; 
		}
	}
}
