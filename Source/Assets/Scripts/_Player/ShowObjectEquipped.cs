using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectEquipped : MonoBehaviour
{
    public static ShowObjectEquipped instance;
    private GameObject playerReference;
    private Animator playerAnimator;
    private int EquipHash = Animator.StringToHash("Equipped");

    public void Start()
    {
        instance = this;
    }

    void Awake()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = playerReference.GetComponent<Animator>();
    }

    /// <summary>
    /// In this method we modify the graphycs of Caroline and put the item in her hand
    /// </summary>
    public void ShowObject(string nameObject)
    {
        Transform g = transform.Find(nameObject);
        Debug.Log(g);
        if (g != null)
        {
            g.gameObject.SetActive(true);
            if (g.gameObject.tag.Equals("BigItem"))
                playerAnimator.SetBool(EquipHash, true);
        }
    }

    public void HideObject(string nameObject)
    {
        Transform g = transform.Find(nameObject);
        if (g != null)
        {      
            g.gameObject.SetActive(false);
            if (g.gameObject.tag.Equals("BigItem"))
                playerAnimator.SetBool(EquipHash, false);
        }
    }
}
