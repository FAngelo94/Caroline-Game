using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
	public AudioClip buttonFocusSound;
	public AudioClip buttonPressedSound;
	public AudioClip pickupSound;
	public AudioClip hornSound;
	public AudioClip equipSound;
	public AudioClip unequipSound;
	public AudioClip eatSound;
	public AudioClip wrongTry;
	public static SoundManager instance;

	private AudioSource source;
	
	// Use this for initialization
	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else if(instance!=this)
			Destroy(gameObject);

		source = GetComponent<AudioSource>();
		source.playOnAwake = false;
		source.loop = false;
	}

	public void ButtonFocussed()
	{
		if (buttonFocusSound)
		{
			source.pitch = 1;
			source.volume = 1;
			source.PlayOneShot(buttonFocusSound);
		}

	}
	
	public void ButtonPressed()
	{
		if (buttonPressedSound)
		{
			source.pitch = 1;
			source.volume = 1;
			source.PlayOneShot(buttonPressedSound);
		}
			
	}

	public void PickedUp()
	{
		if (pickupSound)
		{
			source.pitch = 1;
			source.volume = 0.5f;
			source.PlayOneShot(pickupSound);
		}
			
	}

	public void UseHorn()
	{
		if (hornSound)
		{
			source.pitch = 1.8f;
			source.volume = 0.4f;
			source.PlayOneShot(hornSound);
		}
			
	}
	
	public void EquipItem()
	{
		if (equipSound)
		{
			source.pitch = 1;
			source.volume = 1;
			source.PlayOneShot(equipSound);
		}
			
	}
	
	public void unequipItem()
	{
		if (unequipSound)
		{
			source.pitch = 1;
			source.volume = 1;
			source.PlayOneShot(unequipSound);
		}
			
	}

	public void Eat()
	{
		if (eatSound)
		{
			source.pitch = 1;
			source.volume = 1;
			source.PlayOneShot(eatSound);
		}
	}


	public void Wrong()
	{
		if (wrongTry)
		{
			source.pitch = 1;
			source.volume = 1;
			source.PlayOneShot(wrongTry);
		}
	}
	
}
