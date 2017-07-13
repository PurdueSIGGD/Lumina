using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Door : Usable {
    public string sceneToLoad = "Dungeon";
    public string displayText;
    public Animator myAnimator;
    private GameObject player;
    public int seed, depth;
    public void Start() {
        if (displayText == "") {
            displayText = "Enter " + sceneToLoad;
        }
    }
    public override string getInfoText() {
        return displayText;
    }

    public override void Use() {
        player = GameObject.FindGameObjectsWithTag("Player")[0];

        if (myAnimator != null) {
            myAnimator.SetTrigger("Open");
        }
        // Maybe add a custom player animation?
        // Disable player movement
        player.SendMessage("PrepareToEnterDungeon");
        StartCoroutine(LoadScene());
    }
    public IEnumerator LoadScene() {
        yield return new WaitForSeconds(2);
        player.SendMessage("EnterDungeon");
        PlayerPrefs.SetInt("DungeonSeed", seed);
        PlayerPrefs.SetInt("DungeonDepth", depth);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);

    }

}
