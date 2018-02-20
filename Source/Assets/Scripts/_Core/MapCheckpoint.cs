using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCheckpoint : MonoBehaviour {

    [Header("The shadow that must be destroy")]
    public GameObject shadow;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            shadow.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
