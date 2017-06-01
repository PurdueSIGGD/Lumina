using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour {

	// Use this for initialization
	void Start () {

        //PlayerPrefs.SetInt("MyKey", 1);
        //PlayerPrefs.Save();

        Debug.Log(PlayerPrefs.GetInt("MyKey").ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
