using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowsNest : MasterFacility_Class
{
    public GameObject Piston;
    bool liftDown = true;
    Vector3 targetPostion = new Vector3 (0, -870, 0);

    public override void SetUp()
    {
        notOK_Text = "";
        OK_Text = "E to Activate";
        ChargeRate = 1;
    }

    public override void FacilityEffect(MasterInventory_Class inventory)
    {
        if (liftDown)
        {
            liftDown = false;
            targetPostion = new Vector3(0, -390, 0);
        }

        else
        {
            liftDown = true;
            targetPostion = new Vector3(0, -870, 0);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Piston.transform.position != targetPostion)
        {
            Piston.transform.localPosition =
                Vector3.MoveTowards(
                Piston.transform.localPosition,
                targetPostion, 2f);
        }
    }
}
