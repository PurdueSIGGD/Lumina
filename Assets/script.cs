using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		this.transform.position += Input.GetAxis ("Vertical") * Vector3.forward * Time.deltaTime * 10;
		this.transform.position += Input.GetAxis ("Horizontal") * Vector3.right * Time.deltaTime * 10;
	}
}
