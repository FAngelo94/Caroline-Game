using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBlink : MonoBehaviour {

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = gameObject.GetComponent<Animator>();
        InvokeRepeating("Blink", 0f, 0.5f);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void Blink()
    {
        float r = Random.value;
        if (r > 0.8)
            animator.SetTrigger("Blink");
    }
}
