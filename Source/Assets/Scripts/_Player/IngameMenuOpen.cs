using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenuOpen : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("IngameMenu"))
        {
            if (!GameManager.instance.IngameMenuOpened)
                GameManager.instance.OpenIngameMenu();
            else
                GameManager.instance.CloseIngameMenu();
        }
	}
}
