using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeManager : MonoBehaviour
{
	//// Use this for initialization
	//void Start () {
		
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}



    //TempCode for day 1 testing
    [SerializeField]
    TemporaryMovement p1;

    [SerializeField]
    TemporaryMovement p2;

    public TemporaryMovement selectedP;

    void Start()
    {
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
