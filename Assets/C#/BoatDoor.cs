using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoatDoor : Door {
    [System.Serializable]
    public class SceneChoice {
        public string sceneName;
        public int dungeonsBeforeUnlock;
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
            if (!(SceneManager.GetActiveScene().name == isceneChoice.sceneName)) {
                canvas.menuOptions[index].SetActive(true);
                atLeastOne = true;
                if (sceneChoices[index].dungeonsBeforeUnlock > GameSaveManager.GetClearedDungeonCount()) {
                    // Disable button
                    canvas.SendMessage("DisableButton", index);
                } else {
                    canvas.SendMessage("EnableButton", index);
                }
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
        SceneSelectionCanvas canvas = GameObject.FindObjectOfType<SceneSelectionCanvas>();


        // Error checking
        if (sceneChoices[choice].dungeonsBeforeUnlock > GameSaveManager.GetClearedDungeonCount()) {
            canvas.SendMessage("DungeonsLeftError", sceneChoices[choice].dungeonsBeforeUnlock - GameSaveManager.GetClearedDungeonCount());
            return;
        }


        canvas.SendMessage("Hide");

        sceneChoice = choice;

        player = GameObject.FindGameObjectsWithTag("Player")[0];

        if (fadeType == FadeType.Dark) {
            canvas.SendMessage("FadeToBlack");
        } else if (fadeType == FadeType.Light) {
            canvas.SendMessage("FadeToWhite");
        }

        // Disable player movement
        sceneSwitchPrep(player);
        StartCoroutine(LoadDoorScene());

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
