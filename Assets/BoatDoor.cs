using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatDoor : Door {
    [System.Serializable]
    public class SceneChoice {
        public string sceneName;
        public bool unlocked;
        public Vector3 spawnPosition;
    }
    private SceneSelectionCanvas canvas;
    public SceneChoice[] sceneChoices;
    public int sceneChoice;

    public void Start() {

    }
    public void Update() {
        if (!canvas) {
            // Sometimes it won't pick up the canvas if it hasn't been initialized yet, can't put in start unless I mess with start order
            canvas = GameObject.FindObjectOfType<SceneSelectionCanvas>();
            canvas.SetBoat(this);
        }
    }

    public override void Use() {
        // Figure out what we can display or not
        int index = 0;
        bool atLeastOne = false;
        foreach (SceneChoice isceneChoice in sceneChoices) {
            if (isceneChoice.unlocked && !(SceneManager.GetActiveScene().name == isceneChoice.sceneName)) {
                canvas.menuOptions[index].SetActive(true);
                atLeastOne = true;
            } else {
                canvas.menuOptions[index].SetActive(false);
            }
            index++;
        }
        // Display "No levels unlocked!"
        canvas.notFoundMenuOption.SetActive(!atLeastOne);

        canvas.SendMessage("Show");
    }
    public void selectScene(int choice) {
        sceneChoice = choice;

        player = GameObject.FindGameObjectsWithTag("Player")[0];

        if (fadeType == FadeType.Dark) {
            GameObject.FindObjectOfType<SceneSelectionCanvas>().SendMessage("FadeToBlack");
        } else if (fadeType == FadeType.Light) {
            GameObject.FindObjectOfType<SceneSelectionCanvas>().SendMessage("FadeToWhite");
        }

        // Disable player movement
        sceneSwitchPrep(player);
        StartCoroutine(LoadScene());

    }
    
    

    public override string getSceneToLoad() {
        return sceneChoices[sceneChoice].sceneName;
    }

    public override void preSceneSwitch(GameObject player) {
        player.SendMessage("SwitchMap", sceneChoices[sceneChoice]);
    }

    public override void sceneSwitchPrep(GameObject player) {
        player.SendMessage("StopMoving");
    }
}
