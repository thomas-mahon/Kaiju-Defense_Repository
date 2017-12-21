using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempShipHealth : MonoBehaviour
{
	void Start ()
    {
        Health tempH = GetComponent<Health>();
        tempH.HitPoints = Mathf.Infinity;	
	}

}
