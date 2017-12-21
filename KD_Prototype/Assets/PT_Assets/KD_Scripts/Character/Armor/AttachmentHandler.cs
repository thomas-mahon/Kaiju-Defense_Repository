using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentHandler : MonoBehaviour
{
    public GameObject MyAnchorPoint = null;
    GameObject ObjectToAttachClone;
    public bool waitingToDie;
    FixedJoint AnchorFixedJoint;

    //0.15f for now
    float Offset = 0.15f;

    void Awake()
    {
        SetUpAnchor();
    }

    void Update()
    {
        if (MyAnchorPoint != null)
        {
            MyAnchorPoint.transform.rotation = transform.rotation;
        }

        if (waitingToDie == true && MyAnchorPoint != null)
        {
            int i = MyAnchorPoint.transform.childCount;

            if (i <= 0)
            {
                Destroy(this);
            }
        }
    }

    public void KillAttachments()
    {
        DropAllArmor();
    }

    void SetUpAnchor()
    {
        GameObject AnchorToCreate;
        Rigidbody AnchorRigidBodyToCreate;

        //Create the Anchor point
        AnchorToCreate = new GameObject("Attachment Anchor - " + this.gameObject.name);
        AnchorPoint AnchorPointScript = AnchorToCreate.AddComponent<AnchorPoint>();

        //temp
        //AnchorPointScript.attachmentHandler = this;
        AnchorPointScript.attachmentObject = this.gameObject;

        AnchorPointScript.waitingToDespawn = true;
        AnchorToCreate.transform.position = transform.position;
        AnchorRigidBodyToCreate = AnchorToCreate.AddComponent<Rigidbody>();
        AnchorRigidBodyToCreate.mass = 0;
        AnchorRigidBodyToCreate.useGravity = false;
        AnchorRigidBodyToCreate.drag = 0;
        AnchorRigidBodyToCreate.angularDrag = 0;

        //Create the Fixed Joint we want to add to target
        AnchorFixedJoint = this.gameObject.AddComponent<FixedJoint>();

        //Double check position and set rotation
        AnchorToCreate.transform.position = transform.position;
        AnchorToCreate.transform.rotation = transform.rotation;

        //Set our AttachTarget Connected Body to be the Anchor
        AnchorFixedJoint.connectedBody = AnchorRigidBodyToCreate;
        AnchorFixedJoint.anchor = AnchorToCreate.transform.position;

        //Set Achor to be the handler's attachment point
        MyAnchorPoint = AnchorToCreate;

        MyAnchorPoint.layer = 13;
    }

    public void AddAttachment
        (GameObject ObjectToAttach, Vector3 PointOfAttachment, Quaternion OriginRotation, 
        Vector3 hitNormal, bool NeedToInstatiate, Vector3 ObjectScale, bool SetUp, int SpinRange)
    {
        //Spawn the object if we need to spawn it, set its layer, and scale
        if (!NeedToInstatiate)
        {
            ObjectToAttachClone = ObjectToAttach;
        }
        else
        {
            ObjectToAttachClone = Instantiate(ObjectToAttach);
            ObjectToAttachClone.layer = 13;
            ObjectToAttachClone.transform.localScale = ObjectScale;
        }

        if (MyAnchorPoint != null)
        {
            //Child the attachment object to the anchor point
            ObjectToAttachClone.transform.parent = MyAnchorPoint.transform;

            //Move the attachment object the location it hit
            ObjectToAttachClone.transform.position = PointOfAttachment;

            //Rotate the object to the rotation it hit the collider with
            ObjectToAttachClone.transform.rotation = OriginRotation;

            if (SetUp)
            {
                Vector3 SpinRotation = new Vector3(0, 0, Random.Range(SpinRange * -1, SpinRange));
                ObjectToAttachClone.transform.Rotate(SpinRotation);
            }

            else
            {
                //Mirror the rotation if the object had to be spawned in
                if (NeedToInstatiate)
                {
                    Vector3 Reflect = Vector3.Reflect(ObjectToAttachClone.transform.forward, hitNormal);
                    ObjectToAttachClone.transform.rotation = Quaternion.LookRotation(Reflect);
                    ObjectToAttachClone.transform.Translate(transform.forward * Offset, Space.Self);
                }
                else
                {
                    //Offset the object away from it's attach point
                    ObjectToAttachClone.transform.Translate(transform.forward * Offset * -1, Space.Self);
                }
            }
        }
    }

    public void SimpleAddAttachment(GameObject ObjectToAttach, Vector3 SavedPos, Quaternion SavedRot)
    {
        if (MyAnchorPoint != null)
        {
            ObjectToAttachClone = ObjectToAttach;

            ObjectToAttachClone.layer = 14;

            foreach (Transform x in ObjectToAttachClone.transform)
            {
                x.gameObject.layer = 14;
            }

            ObjectToAttachClone.transform.parent = MyAnchorPoint.transform;

            ObjectToAttachClone.transform.position = SavedPos;

            ObjectToAttachClone.transform.rotation = SavedRot;
        }
    }

    public void DropAllArmor()
    {
        Armor[] ArmorToDrop = MyAnchorPoint.GetComponentsInChildren<Armor>();

        foreach (Armor x in ArmorToDrop)
        {
            x.FallOff();
        }
    }
}