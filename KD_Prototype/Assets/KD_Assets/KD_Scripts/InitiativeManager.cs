using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeManager : MonoBehaviour
{
    //TempCode for day 1 testing
    [SerializeField]
    CustomCharacterController p1;

    [SerializeField]
    CustomCharacterController p2;

    public CustomCharacterController selectedP;

    void Start()
    {
        p2.ToggleControl(false);
        selectedP = p1;
        selectedP.ToggleControl(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            selectedP.ToggleControl(false);

            if (selectedP == p1)
            {
                
                selectedP = p2;
            }

            else
            {
                selectedP = p1;
            }

            selectedP.ToggleControl(true);
        }
    }
}
