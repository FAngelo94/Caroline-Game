using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalCutsceneEvents : MonoBehaviour {

	public GameObject bear;

	public void MoveBear(float bearSpeed) {
		bear.GetComponent<Rigidbody2D>().velocity = new Vector2(bearSpeed, 0f);
	}

	public void StopBear() {
		bear.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
	}
}
