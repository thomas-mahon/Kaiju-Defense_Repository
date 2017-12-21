using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_Inventory : MasterInventory_Class
{
    public override void SetUp()
    {
        MaximumNumberOfRangedWeapons = 1;
    }

    void Start()
    {
        healthScript.HitPoints = 25;
    }

    public override void DeathEffect()
    {
        DropEverything();
    }
}
