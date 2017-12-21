using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Movement : MonoBehaviour {

    public int Speed;
    Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        rb.AddForce(transform.forward * Speed * Time.deltaTime);
        //rb.velocity = new Vector3(0, 0, Speed * Time.deltaTime);
    }
}
