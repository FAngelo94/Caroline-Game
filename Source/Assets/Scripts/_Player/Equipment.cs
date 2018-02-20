using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
	public GameObject caroline;

	internal Animator carolineAnimator;

    void Start()
    {
		carolineAnimator = caroline.GetComponent<Animator> ();
		Debug.Log (carolineAnimator.ToString ());
    }
}
