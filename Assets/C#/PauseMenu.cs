using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
	public Text healthAmount;
	public Text magicAmount;
	public Text upgradeAmount;
	public HUDController hC;

	public bool changeState;

	bool pausing;

	// Use this for initialization
	void Start () {
		changeState = false;
		pausing = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public bool getPause(){
		return pausing;
	}

	public void setPause(bool b){
		pausing = b;
	}

	public void closeHUDOpenPause(){
		gameObject.SetActive (true);
		hC.gameObject.SetActive (false);
	}

	public void closePauseOpenHUD(){
		gameObject.SetActive (false);
		hC.gameObject.SetActive (true);
	}
}
