using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateDestruction : MonoBehaviour
{
    [SerializeField]
    Health health;

    [SerializeField]
    GameObject modelToSwap;

    [SerializeField]
    GameObject[] CrateLoot;

	// Use this for initialization
	void Start ()
    {
        health = GetComponent<Health>();
        health.HitPoints = 50;	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (health != null && health.isDestroyed == true)
        {
            GameObject Loot = Instantiate(modelToSwap, transform.position, transform.rotation);
            Instantiate(CrateLoot[Random.Range(0, CrateLoot.Length)], transform.position, transform.rotation);
            Loot.AddComponent<Rigidbody>();
            Destroy(this.gameObject);
        }
	}
}
