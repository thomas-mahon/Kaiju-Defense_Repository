using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatopotato : MonoBehaviour
{
    public int speed = 1;

    Rigidbody rb;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
		//if (Input.GetKey(KeyCode.Y))
        {
            transform.Rotate(0, speed * Time.deltaTime, 0, 0);
            //rb.velocity = Vector3.zero;
        }
	}
}
