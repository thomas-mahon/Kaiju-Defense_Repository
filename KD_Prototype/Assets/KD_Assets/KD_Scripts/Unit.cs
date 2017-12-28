using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{ 
    [SerializeField]
    KD_CharacterController KD_CC;

    public int InitiativeValue;

    public void Awake()
    {
        KD_CC = GetComponent<KD_CharacterController>();
    }

    public void ToggleControl(bool toggle)
    {
        KD_CC.enabled = toggle;
        KD_CC.playerCamera.SetActive(toggle);
    }
}
