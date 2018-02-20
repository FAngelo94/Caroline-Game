using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ForceTest : MonoBehaviour {

    public float waitTime = 2f;
    public float pushPower = 2f;

    private float time;
    private Rigidbody2D rb2d;

    void Start()
    {
        time = waitTime;
        rb2d = GetComponent<Rigidbody2D>();
    }
	// Update is called once per frame
	void Update () {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            rb2d.AddForce(Vector2.up * pushPower, ForceMode2D.Impulse);
            time = waitTime;
        }

    }
}
