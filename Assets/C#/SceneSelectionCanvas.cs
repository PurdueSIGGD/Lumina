using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSelectionCanvas : MonoBehaviour {
    public Animator fadeToWhiteAnimator, fadeToBlackAnimator, myAnimator;
    private BoatDoor myBoatDoor;

    public GameObject[] menuOptions;
    public GameObject notFoundMenuOption;


	// Use this for initialization
	void Start () {
        fadeToWhiteAnimator.SetTrigger("Reset");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetBoat(BoatDoor boatDoor) {
        myBoatDoor = boatDoor;
    }
    void Show() {
        myAnimator.SetTrigger("Show");
        GameObject.FindObjectOfType<InputGenerator>().PauseGame();
    }

    void Hide() {
        myAnimator.SetTrigger("Hide");
        GameObject.FindObjectOfType<InputGenerator>().ResumeGame();

    }

    void FadeToBlack() {
        fadeToBlackAnimator.SetTrigger("Fade");
    }
    void FadeFromBlack() {
        fadeToBlackAnimator.SetTrigger("Reset");
    }

    void FadeToWhite() {
        fadeToWhiteAnimator.SetTrigger("Fade");
    }
    void FadeFromWhite() {
        fadeToWhiteAnimator.SetTrigger("Reset");
    }

    void Option1() {
        myBoatDoor.selectScene(0);
        Hide();
    }
    void Option2() {
        myBoatDoor.selectScene(1);
        Hide();
    }
    void Option3() {
        myBoatDoor.selectScene(2);
        Hide();
    }

}
