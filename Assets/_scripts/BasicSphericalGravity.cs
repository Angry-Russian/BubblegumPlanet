using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSphericalGravity : MonoBehaviour {

    public Transform gravityCenter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody>().AddForce((gravityCenter.transform.position - transform.position).normalized * 10, ForceMode.Acceleration);
	}
}
