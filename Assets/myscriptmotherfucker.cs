using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myscriptmotherfucker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.GetComponent <Rigidbody>().AddForce(new Vector3(3,3,3));
	}
}
