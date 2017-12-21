using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEngine : MasterFacility_Class
{
    public override void SetUp()
    {
        notOK_Text = "Charging..." + Charge;
        OK_Text = "SYSTEMS NOMINAL";
        ChargeRate = 5;
        Charge = 100;
    }

    public override void FacilityEffect(MasterInventory_Class inventory)
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        notOK_Text = "Charging... " + Math.Round(Charge) + "%";
    }
}
