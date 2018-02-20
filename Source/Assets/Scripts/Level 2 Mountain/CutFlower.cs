using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutFlower : MonoBehaviour
{
    [Header("Flower hanging")]
    public GameObject item;

    private bool fallen;
    private Rigidbody2D body;
    void Start()
    {
        fallen = false;
        body = item.GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Static;
    }

    void Update()
    {
        if (body.velocity.y > 0)
            fallen = true;
        if (body.velocity.y < Mathf.Epsilon && fallen == true)
        {
            //When item is on the ground it become trigger so user can keep it
            body.bodyType = RigidbodyType2D.Static;

            BoxCollider2D box = item.GetComponent<BoxCollider2D>();
            box.isTrigger = true;
            fallen = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision");
        Rigidbody2D body = item.GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Dynamic;
    }
}
