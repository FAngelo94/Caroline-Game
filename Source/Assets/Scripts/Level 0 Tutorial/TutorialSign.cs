using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSign : MonoBehaviour {

	public GameObject keyboardButton;
	public GameObject joystickButton;

	// Use this for initialization
	void Start () {
		if (Input.GetJoystickNames().Length == 0) {
			keyboardButton.SetActive(true);
			joystickButton.SetActive(false);
		} else {
			keyboardButton.SetActive(false);
			joystickButton.SetActive(true);
		}
	}
}
