using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    public GameObject[] objectsToSave;
    public AudioClip background;
    [Range(0, 1)] public float volume;
    public Transform playerSpawn;
    public DeadzoneCamera camera;


    private void Start()
    {
        if (background)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = background;
            source.loop = true;
            source.volume = volume;
            source.Play();
        }
    }
}
