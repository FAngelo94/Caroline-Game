using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour {

	public ParticleSystem magicEffect;
	public GameObject magicPotion;

	public void PlayMagic()
	{
		magicEffect.Play();
	}

	public void StopMagic()
	{
		magicEffect.Stop();
	}

	public void PotionAppear()
	{
		magicPotion.SetActive(true);
	}
}
