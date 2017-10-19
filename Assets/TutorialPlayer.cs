using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour {
    public GameObject tutorialObject;
	// Use this for initialization
	public void PlayTutorial () {
        foreach (Tutorial obj in GameObject.FindObjectsOfType<Tutorial>()) {
            // Stop all pending
            Destroy(obj.gameObject);
        }

        if (Time.timeScale == 1) {

        } else {
            this.GetComponentInParent<UIController>().ToggleUI(KeyCode.Tab);
            Time.timeScale = 1;
        }
        GameObject.Instantiate(tutorialObject);
        
	}
	 
}
