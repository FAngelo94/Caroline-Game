using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEffect : MonoBehaviour {


	private float elapsedTime;
	public float timeToChange;
	
	void Awake()
	{
		elapsedTime = timeToChange;
	}

	void Update()
	{
		if (elapsedTime <= 0)
		{

			float scale = Random.Range(7, 10); 
			gameObject.transform.localScale = new Vector3(scale,scale,0);
			elapsedTime = timeToChange;
		}
		else
		{
			elapsedTime -= Time.deltaTime;
		}
	}
}

