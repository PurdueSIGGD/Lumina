using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameButtonStarter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!GameSaveManager.HasSave())
        {
            this.GetComponent<Button>().interactable = false;
        } else
        {

        }
	}
}
