using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.instance.NextLevel();
    }
}
