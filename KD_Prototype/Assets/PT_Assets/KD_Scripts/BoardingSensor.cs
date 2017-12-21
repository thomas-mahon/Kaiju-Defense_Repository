using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardingSensor : MonoBehaviour
{
    public bool TargetInRange;

    [SerializeField]
    GameObject bridge;

    public GameObject PlayerShip;

    void OnTriggerEnter(Collider other)
    {
        //if (bridge.activeSelf == false && bridge != null && other.gameObject == PlayerShip)
        if (bridge.activeSelf == false && bridge != null && other.gameObject.layer == 15)
        {
            TargetInRange = true;
            bridge.SetActive(true);
        }
    }
}
