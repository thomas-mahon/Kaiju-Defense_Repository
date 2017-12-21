using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionTag : MonoBehaviour
{
    public enum Faction
    {
        Player,
        Bandit,
        Fire,
        Shark,
        Robot
    }

    public Faction SelectedFaction;

}
