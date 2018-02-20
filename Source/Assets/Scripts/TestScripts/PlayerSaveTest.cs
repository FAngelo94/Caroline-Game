using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveTest : MonoBehaviour, ISavableObject{
    public LayerMask groundCheck;
    public void Save()
    {

        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down,50,groundCheck);
        
        PlayerPrefs.SetFloat("player_x", transform.position.x);
        PlayerPrefs.SetFloat("player_y", transform.position.y - hit.distance);
        PlayerPrefs.SetFloat("player_z", transform.position.z);
    }

    public void Load()
    {
        float x = PlayerPrefs.GetFloat("player_x");
        float y = PlayerPrefs.GetFloat("player_y");
        float z = PlayerPrefs.GetFloat("player_z");
        transform.position = new Vector3(x, y, z);
    }

}


