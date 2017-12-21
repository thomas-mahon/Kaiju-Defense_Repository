using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDispenser : MasterFacility_Class
{
    public override void SetUp()
    {
        notOK_Text = "Charging..." + Charge;
        OK_Text = "E to gain Ammo";
        ChargeRate = 45;
    }

    public override void FacilityEffect(MasterInventory_Class inventory)
    {
        if (Charge >= 100 && inventory.EquippedRangedWeaponScipt != null)
        {
            inventory.EquippedRangedWeaponScipt.reserveAmmo =
                inventory.EquippedRangedWeaponScipt.reserveAmmo + (inventory.EquippedRangedWeaponScipt.maxAmmoCapacity * 2);

            Charge = 0;
        }
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        notOK_Text = "Charging... " + Math.Round(Charge) + "%";
    }
}
