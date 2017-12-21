using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableAttacher : MonoBehaviour
{
    //Put this on a child of the throwable and that part will become sticky

    public bool isLive = false;
    public GameObject throwable;
    ContactPoint contact;

    public void CollisionEffect(Collider collider, ContactPoint contactpoint)
    { 
        if (isLive)
        {
            Vector3 tempPos = throwable.transform.position;
            Quaternion tempRot = throwable.transform.rotation;

            GameObject ObjectHit = collider.gameObject;

            AttachmentHandler AttachmentHandlerToTarget = ObjectHit.GetComponent<AttachmentHandler>();

            if (AttachmentHandlerToTarget != null)
            {
                contact = contactpoint;

                Rigidbody parentRB = throwable.GetComponent<Rigidbody>();
                parentRB.isKinematic = true;

                Collider tempCol = throwable.GetComponent<Collider>();
                tempCol.isTrigger = true;

                AttachmentHandlerToTarget.SimpleAddAttachment(throwable, tempPos, tempRot);
            }
        }
    }
}
