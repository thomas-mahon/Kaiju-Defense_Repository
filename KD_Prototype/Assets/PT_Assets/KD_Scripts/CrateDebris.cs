using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateDebris : MonoBehaviour
{
	void Start ()
    {
        StartCoroutine(Killself());
	}

    IEnumerator Killself()
    {
        yield return new WaitForSeconds(15f);
        Destroy(this.gameObject);
    }
}
