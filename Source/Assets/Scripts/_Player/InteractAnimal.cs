using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is imported in all creatures that interact with the player
/// in order to manage the different balloon that can appear in the head of creatures
/// and also manager the interaction with the creature.
/// It is a sort of model for all Creatures.
/// </summary>
public class InteractAnimal : MonoBehaviour
{
    // Use this for initialization
	void Awake()
	{
	}

    /// <summary>
    /// This method will be called in Update of all creatures
    /// </summary>
    public IEnumerator ManageBalloon()
    {
        //Manage interaction
        if (Input.GetButtonDown("Interact") && InteractionConditions() && Time.timeScale != 0)
        {
            Interaction();
        }

        //Manage object use
        if (Input.GetButtonDown("Use") && Time.timeScale != 0)
        {
            if (UseObjectConditions())
            {
                ObjectUse();
                yield return new WaitForEndOfFrame();
            }
            // TODO Check if it there is an easy way to make this work
            else
            {
				Debug.Log (gameObject.name);
            }

        }
    }

    //VIRTUAL METHOD
    //This method will be implemented in the creatures classes

    /// <summary>
    /// Interaction between player and creature
    /// </summary>
    virtual public void Interaction() { }
    /// <summary>
    /// Player use an object with a creature
    /// </summary>
    virtual public void ObjectUse() { }
    /// <summary>
    /// Check when a player can use some object with a creature
    /// </summary>
    /// <returns></returns>
    virtual public bool UseObjectConditions()
    {
        return false;
    }
    /// <summary>
    /// Check when a player can interact with a creature
    /// </summary>
    /// <returns></returns>
    virtual public bool InteractionConditions()
    {
        return false;
    }
}
