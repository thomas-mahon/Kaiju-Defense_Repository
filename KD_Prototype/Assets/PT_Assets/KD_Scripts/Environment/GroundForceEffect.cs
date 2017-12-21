using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundForceEffect : MonoBehaviour
{
    public bool Toggle;
    public float ThrowForce;
    int Damage = 50;

    void OnCollisionEnter(Collision collision)
    {
        if (Toggle)
        {
            GameObject x = collision.collider.gameObject;

            Player_Inventory tempPI = x.GetComponent<Player_Inventory>();

            Health tempH = x.GetComponent<Health>();

            if (tempPI != null)
            {
                IDamagable tempDam = x.GetComponent<IDamagable>();
                if (tempDam != null)
                {
                    tempDam.TakeDamage(Damage);
                }
            }

            else
            {
                if (tempH != null)
                {
                    tempH.isDestroyed = true;
                }
            }

            AttachmentHandler tempAH = x.GetComponentInChildren<AttachmentHandler>();
            if (tempAH != null)
            {
                tempAH.KillAttachments();
            }

            ParticleSystem tempPs = collision.collider.gameObject.GetComponent<ParticleSystem>();
            Rigidbody tempRb = collision.collider.gameObject.GetComponent<Rigidbody>();

            if (tempRb != null && tempPs == null && tempPI == false)
            {
                tempRb.AddForce(this.transform.forward * -1 * ThrowForce);
                tempRb.AddTorque(this.transform.right * ThrowForce);
                tempRb.AddForce(this.transform.up * (ThrowForce / 4));
            }

            if (tempPI != null)
            {
                tempPI.gameObject.transform.position = tempPI.RespawnPoint.transform.position;
            }
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        if (Toggle)
        {
            Rigidbody tempRb = collisionInfo.collider.gameObject.GetComponent<Rigidbody>();
            ParticleSystem tempPs = collisionInfo.collider.gameObject.GetComponent<ParticleSystem>();

            if (tempRb != null && tempPs == null)
            {
                tempRb.AddForce(this.transform.up * (ThrowForce / 4));
            }
        }
    }
}
