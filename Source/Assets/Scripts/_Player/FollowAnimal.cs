using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAnimal : MonoBehaviour
{
    [Header("Object that must follow")]
    public GameObject goal;

    [Header("Speed of X")]
    [Range(0.5f, 3)]
    public float speedX = 1;

    [Header("Speed of Y")]
    [Range(0.5f, 3)]
    public float speedY = 1;

    [Header("Mantain distance of X from goal")]
    [Range(0.5f, 5)]
    public float distanceX = 2;

    [Header("Mantain distance of Y from goal")]
    [Range(0.5f, 5)]
    public float distanceY = 2;

    private Rigidbody2D rigidBody;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        Debug.Log("prova");
        Follow();
    }

    public void Initialize()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(GameObject animal)
    {
        rigidBody = animal.GetComponent<Rigidbody2D>();
    }

    public void Follow()
    {
        int px = transform.position.x < goal.transform.position.x ? 1 : -1;
        int py = transform.position.y < goal.transform.position.y ? 1 : -1;
        if (Mathf.Abs(rigidBody.transform.position.x - goal.transform.position.x) < distanceX)
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        else
            rigidBody.velocity = new Vector2(speedX * px, rigidBody.velocity.y);
        if (Mathf.Abs(rigidBody.transform.position.y - goal.transform.position.y) < distanceY)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        else
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, speedY * py);
    }

    public void Follow(GameObject animal)
    {
        int px = animal.transform.position.x < goal.transform.position.x ? 1 : -1;
        int py = animal.transform.position.y < goal.transform.position.y ? 1 : -1;
        if (Mathf.Abs(rigidBody.transform.position.x - goal.transform.position.x) < distanceX)
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        else
            rigidBody.velocity = new Vector2(speedX * px, rigidBody.velocity.y);
        if (Mathf.Abs(rigidBody.transform.position.y - goal.transform.position.y) < distanceY)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        else
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, speedY * py);
    }
}
