using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSpawnPointTrigger : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<ISavableObject>().Load();
            GameManager.instance.PlayerDied();
        }
            
    }
}
