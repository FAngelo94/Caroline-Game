using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horn : MonoBehaviour {

	public float playDuration = 1f;

	private Animator carolineAnimator;
	private int playHash = Animator.StringToHash ("Play");
	private bool playing;
	// Use this for initialization
	void Awake()
	{
		carolineAnimator = GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!playing && Input.GetButtonDown ("Use"))
			StartCoroutine (PlaySequence ());
	}

	private IEnumerator PlaySequence() {
		playing = true;
		carolineAnimator.SetTrigger (playHash);
		yield return new WaitForSeconds (playDuration);
		playing = false;
	}
}
