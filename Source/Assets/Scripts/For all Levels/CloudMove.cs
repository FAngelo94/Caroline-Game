using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// By this class we can animate all elements in the game that we want.
/// When player (or what we want) enter i contact with an element in graphics
/// that has this script the element in graphics activate an animation using an
/// animator variabl
/// </summary>
public class CloudMove: MonoBehaviour
{
    [Header("Start direction of cloud, 1=right, -1=left")]
    public int direction=1;

    [Header("Width of the level")]
    public float levelWidth = 150;

    [Header("Max and min speed of clouds")]
    public float maxSpeed=10;
    public float minSpeed=3;

    [Header("Max and min scale of clouds")]
    public float maxDim = 3;
    public float minDim = 1;

    private Rigidbody2D body;
    private float initialPosition;

    public void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        direction *= -1;
        Restart();
    }
    public void Update()
    {
        if (gameObject.transform.position.x > initialPosition + levelWidth || gameObject.transform.position.x < initialPosition - levelWidth)
            Restart();
    }

    public void Restart()
    {
        float scale_X = Random.Range(minDim, maxDim);
        float scale_Y = Random.Range(minDim, maxDim);
        gameObject.transform.localScale = new Vector2(scale_X, scale_Y);
        initialPosition = gameObject.transform.position.x;
        float speed = Random.Range(minSpeed, maxSpeed);
        direction *= -1;
        body.velocity = new Vector2(speed * direction, 0);
    }
}
