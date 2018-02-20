using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageHelper : MonoBehaviour
{

    [Header("Button for use object")]
    public GameObject useObjectButton;
    public GameObject useBoundary;
    public Sprite joystickUseSprite;
    public Sprite keyboardUseSprite;
    private string nameObject;
    [Header("Button for interaction")]
    public GameObject interactionButton;
    public GameObject interactionBoundary;
    public Sprite joystickIntSprite;
    public Sprite keyboardIntSprite;

    [Header("Button for open inventory")]
    public GameObject inventoryButton;
    public Sprite joystickInvSprite;
    public Sprite keyboardInvSprite;
    private string nameInteraction;

    [Header("Name of the items that are always visible on top of button")]
    public List<string> itemAlwaysVisible;

    public static ManageHelper instance;

    private Animator animator;
    private int confusedHash = Animator.StringToHash("Confused");


    public void Start()
    {
        if (instance == null)
            instance = this;
        nameInteraction = "";
        nameObject = "";
        animator = gameObject.GetComponent<Animator>();

        if (Input.GetJoystickNames().Length == 0)
        {
            useObjectButton.GetComponent<Image>().sprite = keyboardUseSprite;
            interactionButton.GetComponent<Image>().sprite = keyboardIntSprite;
            inventoryButton.GetComponent<Image>().sprite = keyboardInvSprite;
        } else
        {
            useObjectButton.GetComponent<Image>().sprite = joystickUseSprite;
            interactionButton.GetComponent<Image>().sprite = joystickIntSprite;
            inventoryButton.GetComponent<Image>().sprite = joystickInvSprite;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Creature") && !interactionButton.Equals("Speak"))
            ChangeInteraction("Speak");
        if ((collision.tag.Equals("Item") || collision.tag.Equals("BigItem")) && !interactionButton.Equals("Hand"))
            ChangeInteraction("Hand");
        if (collision.tag.Equals("Creature") || collision.tag.Equals("InteractingItem"))
            ShowItemIcon();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        RemoveInteraction();
        if (itemAlwaysVisible.IndexOf(nameObject) == -1)
            HideItemIcon();
    }

    public void ChangeObject(string name)
    {
        nameObject = name;
        if (itemAlwaysVisible.IndexOf(nameObject) != -1)
            ShowItemIcon();
    }

    public void RemoveObject()
    {
        if (!nameObject.Equals(""))
        {
            HideItemIcon();
            nameObject = "";
        }
    }

    public void WrongUseObject(string name)
    {
        if(CheckObjectConfused(name))
            animator.SetTrigger(confusedHash);
    }

    private bool CheckObjectConfused(string name)
    {
        if (itemAlwaysVisible.IndexOf(name) == -1)
            return true;
        return false;
    }

    private void HideItemIcon()
    {
        useBoundary.SetActive(false);
        GameObject tmp = useObjectButton.transform.Find(nameObject).gameObject;
        if (tmp != null && nameObject!="")
            tmp.SetActive(false);
    }

    private void ShowItemIcon()
    {
        if(nameObject!="") {
            useObjectButton.transform.Find(nameObject).gameObject.SetActive(true);
            useBoundary.SetActive(true);
        }
    }

    private void ChangeInteraction(string name)
    {
        if(!nameInteraction.Equals(""))
            RemoveInteraction();
        interactionButton.transform.Find(name).gameObject.SetActive(true);
        interactionBoundary.SetActive(true);
        nameInteraction = name;
    }

    private void RemoveInteraction()
    {
        interactionBoundary.SetActive(false);
        foreach(Transform child in interactionButton.transform) {
          child.gameObject.SetActive(false);
        }
        nameInteraction = "";
        /*if (!nameInteraction.Equals(""))
        {
            GameObject tmp = interactionButton.transform.Find(nameInteraction).gameObject;
            if (tmp != null)
                tmp.SetActive(false);
            nameInteraction = "";
        }*/
    }
}
