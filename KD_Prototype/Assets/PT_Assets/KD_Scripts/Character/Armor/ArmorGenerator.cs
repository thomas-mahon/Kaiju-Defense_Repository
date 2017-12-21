using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorGenerator : MonoBehaviour
{
    //Possible armors to place
    //[HideInInspector]
    public List<GameObject> ArmorSelection = new List<GameObject>();

    //All Locations to place armor
    public List<Transform> ArmorSpawnLocations = new List<Transform>();

    //Attachment handlers to attach to
    public List<AttachmentHandler> TargetAtatchmentHandlers = new List<AttachmentHandler>();

    float Offset = 0.15f;

    //Percent Chance to spawn armor
    int PercentChanceToSpawn = 100;

    //Amount of possible Rotation of the armor (180 f0r testing);
    int RotationRange = 180;

	public void GenerateArmor()
    {
        AttachmentHandler[] tempAH = GetComponentsInChildren<AttachmentHandler>();

        foreach (AttachmentHandler x in tempAH)
        {
            TargetAtatchmentHandlers.Add(x);
        }

        GetSpawnLocations();
  	}

    void GetSpawnLocations()
    {
        foreach (AttachmentHandler x in TargetAtatchmentHandlers)
        {
            ArmorSpawnLocations.Clear();

            foreach (Transform y in x.gameObject.transform)
            { 
                if (y.gameObject.tag == "ArmorSpawn")
                {
                    ArmorSpawnLocations.Add(y);
                }
            }

            PlaceArmor(x);
        }
    }

    void PlaceArmor(AttachmentHandler handler)
    {
        foreach (Transform x in ArmorSpawnLocations)
        {
            int SpawnChanceRoll = Random.Range(0, 100);

            if (PercentChanceToSpawn > SpawnChanceRoll)
            {
                handler.AddAttachment
                    (ArmorSelection[Random.Range(0, ArmorSelection.Count)], x.transform.position,
                    x.transform.rotation, Vector3.zero, true, new Vector3
                    (Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), 0.05f), true, RotationRange);
            }
        }
    }
}
