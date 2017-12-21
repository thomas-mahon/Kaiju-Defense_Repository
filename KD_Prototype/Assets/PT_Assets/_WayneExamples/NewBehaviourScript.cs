using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
  

    [SerializeField]
    GameObject Platform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position += new Vector3(.01f, 0.01f, 0.01f);
        //if (Platform.GetComponent<NavMeshSourceTag>() == null)
        //{
        //    Platform.AddComponent<NavMeshSourceTag>();
        //}

		if(Input.GetKeyDown(KeyCode.Space))
        {
            while (Platform.GetComponent<NavMeshSourceTag>() == null)
            {
                Platform.AddComponent<NavMeshSourceTag>();
                 
            }
        }
     
        else if(Input.GetKeyDown(KeyCode.A))
        {
            Destroy(Platform.GetComponent<NavMeshSourceTag>());
        }


    }
}
