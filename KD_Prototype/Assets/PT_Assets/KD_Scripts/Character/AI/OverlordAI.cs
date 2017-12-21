using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OverlordAI : MasterEnemyAI_Class
{
    public bool TestingStart;

    public GameObject PlayerEngine;

    bool DemoToggle;

    #region Set as dead fields
    public bool IsDead;
    public int SurvivingHordeCount;
    #endregion

    #region SetUpFields
    public List<Transform> SpawnLocations = new List<Transform>();
    public List<GameObject> ShipPrefabs = new List<GameObject>();
    public List<GameObject> HordePrefabs = new List<GameObject>();
    public List<GameObject> WeaponPrefabs = new List<GameObject>();
    public List<GameObject> ArmorPrefabs = new List<GameObject>();
    public bool HordeSpawned;
    public GameObject TargetShip;
    #endregion

    #region Ship & Horde
    public GameObject MyShip;
    public HordeBoat hordeBoatScript;
    public List<MinionAI> MyHorde = new List<MinionAI>();
    //public List<NavMeshAgent> MyHordeAgents = new List<NavMeshAgent>();
    #endregion

    #region DetectionFields
    //demo
    float DetectionRange = 80;
    //float DetectionRange = 40;
    public FactionTag[] otherFactionTags;
    public List<GameObject> PossibleTargets = new List<GameObject>();
    float FindAllRate = 2f;
    float DetectLOS = 0.25f;
    float DetectBest = 1f;
    #endregion

    #region Setup Functions
    public override void SetUp()
    {
        factionTag = GetComponent<FactionTag>();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);
    }
    void Start()
    {
        if (TestingStart)
        {
            StartDetectionRoutines();
        }

        else
        {
            SpawnShip();
            GenerateShipArmor();
            //
            SpawnHorde();
            //GenerateHordeArmor();
            hordeBoatScript.ready = true;
            HordeSpawned = true;
            foreach (MinionAI x in MyHorde)
            {
                x.waitingTodie = true;
            }
            //
            //StartCoroutine(PushShipToTargetRoutine());
            //
            StartDetectionRoutines();
        }
    }
    #endregion

    //temp changes for activting Agents
    #region Fixed Updates Functions
    void FixedUpdate()
    {
        CenterOverlord();

        if (hordeBoatScript.inPostion == true && DemoToggle == false)
        {
            Rigidbody tempBoatRB = MyShip.GetComponent<Rigidbody>();
            tempBoatRB.isKinematic = true;

            NavMeshAgent tempBoatNMA = MyShip.GetComponent<NavMeshAgent>();
            Destroy(tempBoatNMA);

            foreach (MinionAI x in MyHorde)
            {
                if (x!= null)
                {
                    x.transform.parent = null;
                    Rigidbody tempRB = x.gameObject.GetComponent<Rigidbody>();
                    tempRB.useGravity = true;
                    NavMeshAgent tempNMA = x.gameObject.GetComponent<NavMeshAgent>();
                    tempNMA.enabled = true;
                    tempNMA.destination = PlayerEngine.transform.position;
                }
            }

            DemoToggle = true;
        }
    }
    void CenterOverlord()
    {
        if (HordeSpawned == true && IsDead == false)
        {
            Vector3 MyHordeMidPoint = Vector3.zero;

            foreach (MinionAI x in MyHorde)
            {
                //if (x.MinionInventory.healthScript.isDestroyed == false && x != null)
                if (x != null)
                {
                    MyHordeMidPoint = new Vector3(
                        MyHordeMidPoint.x + x.transform.position.x,
                        MyHordeMidPoint.y + x.transform.position.y,
                        MyHordeMidPoint.z + x.transform.position.z);
                }
            }

            transform.position = new Vector3(
            MyHordeMidPoint.x / SurvivingHordeCount,
            MyHordeMidPoint.y / SurvivingHordeCount,
            MyHordeMidPoint.z / SurvivingHordeCount);
            //MyHordeMidPoint.x / MyHorde.Count,
            //MyHordeMidPoint.y / MyHorde.Count,
            //MyHordeMidPoint.z / MyHorde.Count);

            //transform.position = MyHordeMidPoint;
        }
    }
    #endregion

    #region Spawn Ship & Crew
    //Spawn a ship on the Overlord & get that ship's crew spawn locations
    void SpawnShip()
    {
        MyShip = Instantiate(ShipPrefabs[Random.Range(0, ShipPrefabs.Count)],
            transform.position, transform.rotation);
        hordeBoatScript = MyShip.GetComponent<HordeBoat>();
        hordeBoatScript.PlayerShip = TargetShip;
        NavMeshAgent boatNMA = MyShip.GetComponent<NavMeshAgent>();
        boatNMA.destination = hordeBoatScript.PlayerShip.transform.position;

        foreach (Transform x in MyShip.gameObject.transform)
        {
            foreach (Transform y in x)
            {
                if (y.gameObject.tag == "HordeSpawn")
                {
                    SpawnLocations.Add(y.gameObject.transform);
                }
            }
        }
    }
    //Get our ship's armor spawn locations & Generate our ship's armor
    void GenerateShipArmor()
    {
        ArmorGenerator tempShipAG = MyShip.GetComponentInChildren<ArmorGenerator>();
        tempShipAG.ArmorSelection = ArmorPrefabs;
        tempShipAG.GenerateArmor();
    }
    //Spawn Horde on ship's crew spawn locations & spawn their weapons
    void SpawnHorde()
    {
        foreach (Transform x in SpawnLocations)
        {
            GameObject hordeClone = Instantiate(HordePrefabs[Random.Range(0, HordePrefabs.Count)],
                x.transform.position, x.transform.rotation);

            hordeClone.transform.parent = x;

            MinionAI tempMAI = hordeClone.GetComponent<MinionAI>();

            MyHorde.Add(tempMAI);

            //MyHordeAgents.Add(tempMAI.navAgent);

            Minion_Inventory tempMI = hordeClone.GetComponent<Minion_Inventory>();

            //tempMI.SetUp();

            tempMI.readyToEquip = true;

            GameObject WeaponClone = Instantiate((WeaponPrefabs[Random.Range(0, WeaponPrefabs.Count)]),
                tempMI.RangedWeaponPosition.position, tempMI.RangedWeaponPosition.rotation);

            RangedWeapon_Class tempWep = WeaponClone.GetComponent<RangedWeapon_Class>();

            WeaponClone.transform.parent = tempMI.RangedWeaponPosition.transform;

            tempWep.MoveToPosition(tempMI.RangedWeaponPosition);

            tempMI.EquippedRangedWeaponScipt = tempWep;

            tempWep.TurnOffInteraction();

            tempMI.EquipNewRangedWeapon(tempWep);

            //tempMI.EquipNewRangedWeapon(tempWep);


            //tempMI.ChangeWeapon();

            //tempMAI.SetRange();
        }

        StartCoroutine(SetToDeadRoutine());
    }
    //Get our horde's armor spawn locations // Archived
    //void GenerateHordeArmor()
    //{
    //    foreach (MinionAI x in MyHorde)
    //    {
    //        ArmorGenerator[] tempHordeAGs = x.GetComponentsInChildren<ArmorGenerator>();
    //        foreach (ArmorGenerator y in tempHordeAGs)
    //        {
    //            y.ArmorSelection = ArmorPrefabs;
    //            y.GenerateArmor();
    //        }
    //    }
    //}
    #endregion

    #region Move Ship To Position // Archived
    //Move the ship in the direction of the target location
    //IEnumerator PushShipToTargetRoutine()
    //{
    //    if (MyShip != null && IsDead == false)
    //    { 
    //        PushShipToTarget();
    //        yield return new WaitForSeconds(0.05f);
    //        StartCoroutine(PushShipToTargetRoutine());
    //    }
    //}
    //void PushShipToTarget()
    //{
    //    if (MyShip != null)
    //    {
    //        float dist = Vector3.Distance(TargetShip.transform.position, MyShip.transform.position);

    //        //float force = 20;
    //        //float force = 10;
    //        float force = 50;

    //        if (dist > 5)
    //        {
    //            Vector3 Direction = new Vector3(
    //                TargetShip.transform.position.x - MyShip.transform.position.x,
    //                0,
    //                TargetShip.transform.position.z - MyShip.transform.position.z);

    //            Rigidbody tempShipRb = MyShip.GetComponent<Rigidbody>();
    //            tempShipRb.AddForce(Direction * force * Time.deltaTime);
    //        }
    //    }
    //}
    #endregion 

    #region Detection
    void StartDetectionRoutines()
    {
        StartCoroutine(FindAllTargetFactionTagsRoutine());
        StartCoroutine(DetectAllPossibleLOSTargetsRoutine());
        StartCoroutine(DetectBestTargetRoutine());
    }

    //Find all faction tags in the scene
    IEnumerator FindAllTargetFactionTagsRoutine()
    {
        FindAllTargetFactionTags();
        yield return new WaitForSeconds(FindAllRate);

        if (IsDead == false)
        {
            StartCoroutine(FindAllTargetFactionTagsRoutine());
        }
    }
    void FindAllTargetFactionTags()
    {
        otherFactionTags = FindObjectsOfType(typeof(FactionTag)) as FactionTag[];

        PossibleTargets.Clear();

        foreach (FactionTag x in otherFactionTags)
        {
            if (x.SelectedFaction != factionTag.SelectedFaction)
            {
                PossibleTargets.Add(x.gameObject);
            }
        }
    }

    //Find every target every minion can see
    IEnumerator DetectAllPossibleLOSTargetsRoutine()
    {
        if (PossibleTargets != null)
        {
            DetectAllPossibleLOStargets();
        }
        yield return new WaitForSeconds(DetectLOS);

        if (IsDead == false)
        {
            StartCoroutine(DetectAllPossibleLOSTargetsRoutine());
        }
    }
    void DetectAllPossibleLOStargets()
    {
        foreach (MinionAI x in MyHorde)
        {
            if (x != null)
            {
                x.LOSTargetsInRange.Clear();

                foreach (GameObject y in PossibleTargets)
                {
                    if (y != null)
                    {
                        Vector3 targetDir = y.transform.position - x.transform.position;

                        RaycastHit hit;

                        if (Physics.Raycast(x.transform.position, targetDir, out hit, DetectionRange))
                        {
                            if (hit.collider.gameObject == y)
                            {
                                Debug.DrawLine(x.transform.position, hit.point, Color.red, 0.025f);
                                x.LOSTargetsInRange.Add(y);
                            }
                        }
                    }
                }
            }
        }
    }

    //Choose the best target for every minion, unless they already have a target
    IEnumerator DetectBestTargetRoutine()
    {
        DetectBestTarget();
        yield return new WaitForSeconds(DetectBest);

        if (IsDead == false)
        {
            StartCoroutine(DetectBestTargetRoutine());
        }
    }
    void DetectBestTarget()
    {
        foreach (MinionAI x in MyHorde)
        {
            if (x.LOSMaintained == false)
            {
                GameObject BestTarget = null;

                float MinimumDistanceToTarget = Mathf.Infinity;

                foreach (GameObject i in x.LOSTargetsInRange)
                {
                    if (i != null)
                    {
                        float DistanceToTarget = Vector3.Distance(i.transform.position, transform.position);

                        if (DistanceToTarget < DetectionRange && DistanceToTarget < MinimumDistanceToTarget)
                        {
                            MinimumDistanceToTarget = DistanceToTarget;
                            BestTarget = i;
                        }
                    }
                }

                x.CurrentTarget = BestTarget;
            }
        }
    }
    #endregion

    #region Set as dead
    //Check every few second if our horde is dead,
    //if they are, we go to origin and set ourself as dead
    IEnumerator SetToDeadRoutine()
    {
        SurvivingHordeCount = 0;

        foreach (MinionAI x in MyHorde)
        {
            if (x != null) 
                //&& x.MinionInventory.healthScript.isDestroyed == false)
            {
                SurvivingHordeCount++;
            }

            ////MEGA TEMP
            if (x.MinionInventory.healthScript.isDestroyed == true)
            {
                if (x.MinionRigidBody != null)
                {
                    x.MinionRigidBody.constraints = RigidbodyConstraints.None;
                    x.MinionRigidBody.isKinematic = false;
                }
                //if (x.navAgent != null)
                //{
                //    Destroy(x.navAgent);
                //}
            }
        }

        if (SurvivingHordeCount == 0)
        {
            transform.position = new Vector3(0, 0, 0);
            //Rigidbody tempRb = MyShip.GetComponent<Rigidbody>();
            //tempRb.constraints = RigidbodyConstraints.None;
            //tempRb.isKinematic = false;
            //tempRb.useGravity = true;
            //foreach (Transform x in MyShip.transform)
            //{
            //    Rigidbody tempRb = x.GetComponent<Rigidbody>();
            //    tempRb.constraints = RigidbodyConstraints.None;
            //    tempRb.isKinematic = false;
            //    tempRb.useGravity = true;
            //}
            MyShip = null;
            IsDead = true;
        }

        yield return new WaitForSeconds(5);

        if (IsDead == false)
        {
            StartCoroutine(SetToDeadRoutine());
        }
    }
    #endregion
}
