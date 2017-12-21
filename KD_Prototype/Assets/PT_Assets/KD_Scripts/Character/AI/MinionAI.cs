using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MasterEnemyAI_Class
{
    public bool inAir;
    public bool waitingTodie;

    NavMeshAgent myAgent;

    #region Object and script references
    //The part of our body that holds our weapons and aims
    public GameObject UpperBody;
    //Minion's inventory
    public Minion_Inventory MinionInventory;
    //Minion's Rigidbody
    public Rigidbody MinionRigidBody;

    //public NavMeshAgent navAgent;
    #endregion

    #region Combat and Detection
    //The target we want to fire at
    public GameObject CurrentTarget;
    //All targets we can see
    public List<GameObject> LOSTargetsInRange = new List<GameObject>();
    //If we are maintaining LOS of our target
    public bool LOSMaintained;
    //The range we want to start shtooting at our target (30 - 2(weaponspread)) + 10
    public float EffectiveRange = 80;
    //Aiming speed
    int AimSpeed = 2;
    #endregion

    //temp for demo
    #region Set Up Functions
    public override void SetUp()
    {
        MinionInventory = GetComponent<Minion_Inventory>();
        MinionRigidBody = GetComponent<Rigidbody>();
        myAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        StartCoroutine(CheckForLOSofTargetRoutine());
        StartCoroutine(AimRoutine());
        StartCoroutine(FireRoutine());
    }

    void LateUpdate()
    {
        if (MinionInventory.healthScript.isDestroyed && waitingTodie == true && myAgent.enabled == true)
        {
            myAgent.isStopped = true;
            myAgent.velocity = Vector3.zero;
            Destroy(myAgent);
            //StopAllCoroutines();
            StartCoroutine(Despawn());
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up * -1, out hit, 1f))
        {
            if (hit.collider.gameObject.layer == 15)
            //|| hit.collider.gameObject.layer == 12)
            {
                inAir = false;
            }
        }

        else
        {
            inAir = true;
        }

        if (myAgent != null)
        {
            if (inAir == true && myAgent.enabled == true && MinionInventory.healthScript.isDestroyed == false)
            {
                MinionRigidBody.isKinematic = false;
                MinionRigidBody.useGravity = true;
            }
        }
    }
    #endregion

    #region Combat and Detection functions

    //Checks if we can actually see the target
    IEnumerator CheckForLOSofTargetRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        if (CurrentTarget != null)
        {
            CheckForLOSofTarget();
        }

        StartCoroutine(CheckForLOSofTargetRoutine());
    }
    void CheckForLOSofTarget()
    {
        RaycastHit hit;

        //temp
        if (Physics.Raycast(UpperBody.transform.position, UpperBody.transform.forward, out hit, EffectiveRange))
        {
            if (hit.collider.gameObject == CurrentTarget)
            {
                Debug.DrawLine(UpperBody.transform.position, hit.point, Color.yellow, 0.1f);
                LOSMaintained = true;
                //Debug.Log("LOS_TRUE");
            }
            else
            {
                LOSMaintained = false;
                //Debug.Log("LOS_FALSE");
            }
        }
    }

    //If we have a target aim at it
    IEnumerator AimRoutine()
    {
        Aim();
        yield return new WaitForFixedUpdate();
        StartCoroutine(AimRoutine());
    }
    void Aim()
    {
        if (CurrentTarget != null)
        {
            //Debug.Log("target to aim at");
            Vector3 targetDir = CurrentTarget.transform.position - UpperBody.transform.position;
            float step = AimSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(UpperBody.transform.forward, targetDir, step, 0.0F);
            UpperBody.transform.rotation = Quaternion.LookRotation(newDir);
        }

        if (CurrentTarget == null)
        {
            //Debug.Log("no target to aim");
            if (UpperBody.transform.localRotation != new Quaternion(0, 0, 0, 0))
            {
                UpperBody.transform.localRotation =
                    Quaternion.Slerp(UpperBody.transform.localRotation, Quaternion.Euler(0, 0, 0), AimSpeed * Time.deltaTime);
            }
        }
    }

    //Fire the weapon if we can see the target
    IEnumerator FireRoutine()
    {
        if (MinionInventory.EquippedRangedWeaponScipt != null)
        {
            if (MinionInventory.EquippedRangedWeaponScipt.currentAmmo != 0)
            {
                Fire();

                if (MinionInventory.EquippedRangedWeaponScipt.currentAmmo == 0 &&
                    MinionInventory.EquippedRangedWeaponScipt != null)
                {
                    MinionInventory.EquippedRangedWeaponScipt.Reload();
                    yield return new WaitForSeconds(MinionInventory.EquippedRangedWeaponScipt.reloadSpeed);
                }

                if (MinionInventory.EquippedRangedWeaponScipt != null)
                {
                    yield return new WaitForSeconds(MinionInventory.EquippedRangedWeaponScipt.fireRate);
                }
            }
        }

        yield return new WaitForEndOfFrame();

        StartCoroutine(FireRoutine());
    }
    private void Fire()
    {
        //if (LOSMaintained && CurrentTarget != null && MinionInventory.EquippedRangedWeaponScipt != null)
        if (LOSMaintained)
        {
            //Debug.Log("Fire");
            MinionInventory.EquippedRangedWeaponScipt.Fire();
        }
    }
    #endregion

    public void StartDespawn()
    {
        StartCoroutine(Despawn());
    }

    //temp for demo
    public IEnumerator Despawn()
    {
        //Debug.Log("DIEEEEEEEEEEEEE");
        Destroy(myAgent);
        waitingTodie = false;   
        Rigidbody tempRb = GetComponent<Rigidbody>();
        tempRb.constraints = RigidbodyConstraints.None;
        tempRb.isKinematic = false;
        tempRb.AddTorque(-100 * UpperBody.transform.right);

        yield return new WaitForSeconds(3f);
        //yield return new WaitForFixedUpdate();
        Destroy(this.gameObject);
    }
}
