using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Check();
    }

    private void OnEnable()
    {
        if (GameManager.instance != null)
            Check();
    }


    private void Check()
    {
        int level = GameManager.instance.GetSavedLevel();
        if (level == -1)
            gameObject.GetComponent<Selectable>().interactable = false;
        else
            gameObject.GetComponent<Selectable>().interactable = true;
    }
}
