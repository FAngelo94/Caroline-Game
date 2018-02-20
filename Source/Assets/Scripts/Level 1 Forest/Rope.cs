using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [Header("item hanging")]
    public GameObject item;
    [Header("Dust when box broken")]
    public GameObject dust;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision");
        Rigidbody2D body = item.GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Dynamic;
        item.transform.parent = gameObject.transform.parent;

        dust.transform.position=new Vector2(item.transform.position.x,dust.transform.position.y);
    }

}
