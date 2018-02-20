using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;
public class ShowEquipment: MonoBehaviour
{
    public static ShowEquipment instance;
    private Animator playerAnimator;
    private int equipHash = Animator.StringToHash("BigItemEquip");
    private int staffEquip = Animator.StringToHash ("StaffEquip");
    private int unequipHash = Animator.StringToHash ("Unequip");

    public GameObject[] Equipment;

    //private SpriteMeshAnimation swapper;

    public void Start()
    {
        instance = this;
        //swapper = gameObject.GetComponent<SpriteMeshAnimation>();
    }

    private void Awake()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    /// <summary>
    /// In this method we modify the graphycs of Caroline and put the item in her hand
    /// </summary>
    public void ShowObject(string nameObject)
    {
        //swapper.frame = 1;
        foreach(GameObject g in Equipment)
        {
            if(g.name.Equals(nameObject))
            {
                playerAnimator.SetBool(unequipHash, false);
                g.SetActive(true);
                if (g.tag.Equals ("BigItem"))
                    playerAnimator.SetTrigger (equipHash);
                else if (g.tag.Equals ("Staff"))
                    playerAnimator.SetTrigger (staffEquip);
                else if (g.tag.Equals ("Untagged"))
                    playerAnimator.SetBool(unequipHash, true);
                return;
            }
        }
    }

    public void HideObjects()
    {
        foreach (GameObject g in Equipment)
        {
            g.SetActive(false);
        }
        playerAnimator.SetBool(unequipHash, true);
        StartCoroutine(waitABit());
        Debug.Log("unequipping");

    }

    private IEnumerator waitABit() {
      yield return new WaitForEndOfFrame();
    }
}
