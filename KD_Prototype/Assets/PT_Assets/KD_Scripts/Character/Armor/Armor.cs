using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Health
{
    public int ArmorCode;

    float DestroyTimer = 5;

    void Awake ()
    {
               
	}

    void Update()
    {

    }

    void Start()
    {

    }

    //public override void Setup()
    //{
    //    StartingHitPoints = 100;
    //    HitPoints = StartingHitPoints;
    //}


    public void GetPhysics()
    {
        transform.parent = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.mass = 1;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce(transform.forward);
    }

    public void FallOff()
    {
        this.gameObject.layer = 10;
        GetPhysics();
    }

    public override void Die()
    {
        isDestroyed = true;
        this.gameObject.layer = 14;
        
        GetPhysics();

        StartCoroutine(DestroyTimerRoutine());
    }

    public override void TakeDamage(int Damage)
    {
        HitPoints = HitPoints - Damage;
        if (HitPoints <= 0 && isDestroyed == false)
        {
            Die();
        }
    }

    IEnumerator DestroyTimerRoutine()
    {
        yield return new WaitForSeconds(DestroyTimer);

        Destroy(this.gameObject);
    }
}
