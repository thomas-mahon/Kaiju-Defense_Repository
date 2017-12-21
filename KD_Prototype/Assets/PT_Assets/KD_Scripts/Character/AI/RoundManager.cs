using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerEngine;
    [SerializeField]
    Health PlayerEngineHealth;
    public int Round;
    //demo
    float timer = 60;
    //float timer = 45;
    public GameObject OverlordPrefab;
    public GameObject OverlordTargetShip;
    //public int chanceToSpawn = 35;
    int chanceToSpawn = 100;
    int chanceToSpawnRoll;
    public List<OverlordAI> SpawnedOverlords = new List<OverlordAI>();
    public List<Transform> OverlordSpawns = new List<Transform>();

    //temp
    [SerializeField]
    Player_Inventory[] playerInvens;

    //[SerializeField]
    int OverlordsToSpawnCount = 1;
    int OverlordsSpawnedCount;
    [SerializeField]
    int OverlordsDead;

    [SerializeField]
    float ChanceToSpawnCrates;
    [SerializeField]
    Transform[] crateSpawns;
    [SerializeField]
    GameObject CratePrefab;

    public bool TankSpawned;
    public bool RifleSpawned;
    public bool MedicSpawned;
    public bool EngieSpawned;

    [SerializeField]
    Transform[] PlayerSpawns;

    [SerializeField]
    GameObject[] PlayerPrefabs;

    [SerializeField]
    GameObject[] PlayersInGame;

    //[SerializeField]
    //GameObject PlayerCharacters;

    //public bool[] CharactersSelected = new bool[] { false, false, false, false };
    //public bool[] CharactersSpawned = new bool[] { false, false, false, false };

    public void Start()
    {
        SpawnPlayers();
        StartRound();

    }

    void SpawnPlayers()
    {
        foreach (Transform x in PlayerSpawns)
        {
            GameObject tempGO =
                Instantiate(PlayerPrefabs[Array.IndexOf(PlayerSpawns, x)], 
                x.transform.position, 
                x.transform.rotation);

            PlayersInGame[Array.IndexOf(PlayerSpawns, x)] = tempGO;

            CustomCharController tempCCC = PlayersInGame[Array.IndexOf(PlayerSpawns, x)].GetComponent<CustomCharController>();

            tempCCC.PlayerNumber = Convert.ToString(Array.IndexOf(PlayerSpawns, x) + 1);

            Player_Inventory tempPI = PlayersInGame[Array.IndexOf(PlayerSpawns, x)].GetComponent<Player_Inventory>();

            tempPI.RespawnPoint = x;
        }
        //This would be determined by the Character Selection in the final build
        //for (int i = 0; i < CharactersSelected.Length; i++)
        //{
        //    CharactersSelected[i] = true;
        //}

        //for (int i = 0; i < CharactersSelected.Length; i++)
        //{

        //}
    }

    void RespawnPlayers()
    {
        for (int i = 0; i < PlayersInGame.Length; i++)
        {
            if (PlayersInGame[i] == null)
            {
                //Debug.Log("hit");
                GameObject tempGO =
                Instantiate(PlayerPrefabs[i],
                PlayerSpawns[i].transform.position,
                PlayerSpawns[i].transform.rotation);

                PlayersInGame[i] = tempGO;

                CustomCharController tempCCC = PlayersInGame[i].GetComponent<CustomCharController>();

                tempCCC.PlayerNumber = Convert.ToString(i + 1);

                Player_Inventory tempPI = PlayersInGame[i].GetComponent<Player_Inventory>();

                tempPI.RespawnPoint = PlayerSpawns[i];

                //temp for demo
                tempPI.healthScript.HitPoints = 50;
            }
        }
    }

    void StartRound()
    {
        Round++;
        OverlordsSpawnedCount = 0;
        OverlordsDead = 0;
        SpawnedOverlords.Clear();

        if (Round == 1)
        {
            SpawnCrates();
        }

        if (Round != 1)
        {
            RespawnPlayers();
        }

        if (Round == 2)
        {
            timer = 55;
            chanceToSpawn = 45;
            OverlordsToSpawnCount = 2;
        }

        if (Round == 3)
        {
            timer = 50;
            chanceToSpawn = 50;
            OverlordsToSpawnCount = 2;
        }

        if (Round == 4)
        {
            timer = 45;
            chanceToSpawn = 55;
            OverlordsToSpawnCount = 3;
        }

        if (Round == 5)
        {
            timer = 40;
            chanceToSpawn = 100;
            OverlordsToSpawnCount = 3;
        }

        //temp for demo
        if (Round == 6)
        {
            //Application.Quit();
            SceneManager.LoadScene(0);
        }

        StartCoroutine(SpawnOverlords());
    }

    public IEnumerator SpawnOverlords()
    {
        foreach (Transform x in OverlordSpawns)
        {
            //demo
            //chanceToSpawn = 35; //+ (generation * 2);
            //chanceToSpawn = 150;
            chanceToSpawnRoll = UnityEngine.Random.Range(0, 100);

            if (chanceToSpawnRoll < chanceToSpawn && OverlordsSpawnedCount < OverlordsToSpawnCount)
            {
                GameObject tempOLGO =
                    Instantiate(OverlordPrefab, x.transform.position, x.transform.rotation);
                OverlordAI tempOL = tempOLGO.GetComponent<OverlordAI>();
                tempOL.TargetShip = OverlordTargetShip;
                tempOL.PlayerEngine = PlayerEngine;
                SpawnedOverlords.Add(tempOL);
                OverlordsSpawnedCount++;
                //generation++;
            }
        }

        yield return new WaitForSeconds(timer);

        if (OverlordsSpawnedCount < OverlordsToSpawnCount)
        {
            StartCoroutine(SpawnOverlords());
        }

        if (OverlordsSpawnedCount >= OverlordsToSpawnCount)
        {
            StartCoroutine(WaitingForEndOfRound());
        }
    }

    IEnumerator WaitingForEndOfRound()
    {
        OverlordsDead = 0;

        foreach (OverlordAI x in SpawnedOverlords)
        {
            if (x.IsDead == true)
            {
                OverlordsDead++;
            }
        }

        // yield return new WaitForSeconds(5);
        yield return new WaitForSeconds(2);

        if (OverlordsDead == OverlordsToSpawnCount)
        {
            //Debug.Log("Round Over");

            foreach (OverlordAI x in SpawnedOverlords)
            {
                Destroy(x.gameObject);
            }

            HordeBoat[] tempArr = FindObjectsOfType<HordeBoat>();

            foreach (HordeBoat x in tempArr)
            {
                Destroy(x.gameObject);
            }

            yield return new WaitForSeconds(2.5f);

            foreach (Player_Inventory x in playerInvens)
            {
                if (x!=null)
                {
                    x.losecan.SetActive(false);
                }
            }

            yield return new WaitForSeconds(60f);

            StartRound();
        }

        else
        {
            StartCoroutine(WaitingForEndOfRound());
        }
    }

    void SpawnCrates()
    {
        foreach (Transform x in crateSpawns)
        {
            float CrateSpawnRoll = UnityEngine.Random.Range(0, 100);
            
            if (ChanceToSpawnCrates > CrateSpawnRoll)
            {
                GameObject tempCrate = Instantiate(CratePrefab, x.position, x.rotation);
                tempCrate.transform.Rotate( new Vector3 (0, UnityEngine.Random.Range(0, 360), 0));
            }
        }
    }
}
