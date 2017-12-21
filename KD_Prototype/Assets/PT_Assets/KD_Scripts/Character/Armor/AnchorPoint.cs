using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
    public AttachmentHandler attachmentHandler;
	public bool waitingToDespawn;
    public GameObject attachmentObject;

	void LateUpdate ()
    {
		//if (attachmentHandler == null && waitingToDespawn)
  //      {
  //          Armor[] ArmorToDrop = GetComponentsInChildren<Armor>();

  //          foreach (Armor x in ArmorToDrop)
  //          {
  //              x.FallOff();
  //          }

  //          Destroy(this.gameObject);
  //      }

        if (attachmentObject == null && waitingToDespawn)
        {
            Armor[] ArmorToDrop = GetComponentsInChildren<Armor>();

            foreach (Armor x in ArmorToDrop)
            {
                x.FallOff();
            }

            Destroy(this.gameObject);
        }
    }
}
