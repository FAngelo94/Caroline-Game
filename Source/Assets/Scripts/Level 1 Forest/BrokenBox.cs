using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBox : MonoBehaviour
{
    [Header("item that appear after the box broke")]
    public GameObject item;

    public ParticleSystem dust;
    public float dustTime;
    public float timeBeforeDeactivate;

    private Rigidbody2D body;
    private bool fallen;

    void Start()
    {
        fallen = false;
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (body.velocity.y > Mathf.Epsilon)
            fallen = true;
        if(body.velocity.y < Mathf.Epsilon && fallen==true)
        {
            StartCoroutine(OpenBox());
        }
    }

    private IEnumerator OpenBox() {
        dust.Play();
        yield return new WaitForSeconds(timeBeforeDeactivate);
        gameObject.SetActive(false);
        item.transform.parent = gameObject.transform.parent;
        item.SetActive(true);
        yield return new WaitForSeconds(dustTime - timeBeforeDeactivate);
        dust.Stop();
    }
}
