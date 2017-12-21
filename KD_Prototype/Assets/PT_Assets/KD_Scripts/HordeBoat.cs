using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HordeBoat : MonoBehaviour
{
    [SerializeField]
    BoardingSensor[] bridges;
    [SerializeField]
    NavMeshAgent thisAgent;
    public GameObject PlayerShip;
    public bool inPostion;
    int i = 0;
    [SerializeField]
    Health FakeGenerator;
    [SerializeField]
    GameObject modelToSwap;
    public bool ready;
    BoxCollider KillBox;
    void Start()
    {
        KillBox = GetComponent<BoxCollider>();
        thisAgent = GetComponent<NavMeshAgent>();
        bridges = GetComponentsInChildren<BoardingSensor>();

        foreach (BoardingSensor x in bridges)
        {
            x.PlayerShip = PlayerShip;
        }

        FakeGenerator.HitPoints = 250;
    }

    void FixedUpdate()
    {
        if (FakeGenerator.isDestroyed == true)
        {
            DeathExplosion();
        }

        if (thisAgent != null && ready)
        {
            i = 0;

            foreach (BoardingSensor x in bridges)
            {
                if (x.TargetInRange)
                {
                    i++;
                }
            }

            if (i > 0)
            {
                inPostion = true;
            }
        }
    }

    void DeathExplosion()
    {
        //RaycastHit hit;
        Collider[] hitColliders = Physics.OverlapBox(KillBox.transform.position, KillBox.size);
           
        foreach (Collider x in hitColliders)
        {
            MinionAI tempMAI = x.transform.gameObject.GetComponent<MinionAI>();
            if (tempMAI != null)
            {
                tempMAI.StartDespawn();
                tempMAI.MinionInventory.healthScript.isDestroyed = true;
            }
        }
        Instantiate(modelToSwap, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(KillBox.transform.position, KillBox.size);
    }
}

