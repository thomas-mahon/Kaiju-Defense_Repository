using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{ 
    [SerializeField]
    KD_CharacterController KD_CC;

    public int InitiativeValue;
    Shooting shooting;

    public void Awake()
    {
        KD_CC = GetComponent<KD_CharacterController>();
        shooting = GetComponent<Shooting>();
        shooting.enabled = false;
    }

    public void ToggleControl(bool toggle)
    {
        KD_CC.IsBeingControlled = toggle;
        KD_CC.playerCamera.SetActive(toggle);
        shooting.enabled = true;
    }
}
