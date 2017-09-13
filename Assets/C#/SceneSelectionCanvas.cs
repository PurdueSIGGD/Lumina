using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSelectionCanvas : MonoBehaviour {
    public Animator fadeToWhiteAnimator, fadeToBlackAnimator, myAnimator;
    private BoatDoor myBoatDoor;

    public GameObject[] menuOptions;
    public GameObject notFoundMenuOption;

    private string clearedDungeonText = "You need to clear {0} more dungeons!";

    public Text errorText;
    public Animator errorAnimator;

    public Color lockedTextColor;
    public Color unlockedTextColor;

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
    }
    void Option2() {
        myBoatDoor.selectScene(1);
    }
    void Option3() {
        myBoatDoor.selectScene(2);
    }

    void DungeonsLeftError(int dungeonsLeft) {
        errorText.text = string.Format(clearedDungeonText, dungeonsLeft);
        errorAnimator.SetTrigger("Error");
    }
    void DisableButton(int index) {
        //menuOptions[index].GetComponent<Button>().interactable = false;
        menuOptions[index].transform.GetChild(0).GetComponent<Text>().color = lockedTextColor;
    }
    void EnableButton(int index) {
        //menuOptions[index].GetComponent<Button>().interactable = true;
        menuOptions[index].transform.GetChild(0).GetComponent<Text>().color = unlockedTextColor;

    }
}
