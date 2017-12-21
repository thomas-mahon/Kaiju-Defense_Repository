using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDispenser : MasterFacility_Class
{
    public override void SetUp()
    {
        notOK_Text = "Charging..." + Charge;
        OK_Text = "E to restore Health";
        ChargeRate = 60;
    }

    public override void FacilityEffect(MasterInventory_Class inventory)
    {
        if (Charge >= 100 && inventory.EquippedRangedWeaponScipt != null)
        {
            inventory.healthScript.HitPoints = inventory.healthScript.HitPoints + 250;

            Charge = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        notOK_Text = "Charging... " + Math.Round(Charge) + "%";
    }
}
