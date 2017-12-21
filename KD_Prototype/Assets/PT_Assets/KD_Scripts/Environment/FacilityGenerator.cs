using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityGenerator : MonoBehaviour, IInteractable
{
    int StartingHealth = 1000;
    Health healthScript;

    public string DisplayText
    {
        get
        {
            return "Hitpoints: " + ((healthScript.HitPoints / StartingHealth)*100) + "%";
        }
    }

    public void Activate(MasterInventory_Class inventory)
    {
        Debug.Log("Fixed machine");
    }

    // Use this for initialization
    void Start ()
    {
        healthScript = GetComponentInChildren<Health>();
        healthScript.HitPoints = StartingHealth;
	}
}
