using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagement : MonoBehaviour
{
    [Header("Map that manage")]
    public GameObject map;

    private bool mapOpen;

    void Start()
    {
        mapOpen = false;
        map.SetActive(false);
    }

    void Update()
    {
        if(Input.GetButtonDown("Map"))
        {
            if (mapOpen == false)
            {
                map.SetActive(true);
                mapOpen = true;
            }
            else
            {
                map.SetActive(false);
                mapOpen = false;
            }
        }
    }
}
