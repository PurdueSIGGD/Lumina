using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class Door : Usable {

    public static string surfaceIdentifier = "surfaceName";

    public enum FadeType { Dark, Light };

    public string displayText;
    public Animator myAnimator;
    protected GameObject player;

    public FadeType fadeType;
   
    public override string getInfoText() {
        return displayText;
    } 

    public override void Use() {
        player = GameObject.FindGameObjectsWithTag("Player")[0];

        if (myAnimator != null) {
            myAnimator.SetTrigger("Open");
        }
        // Maybe add a custom player animation?
        if (fadeType == FadeType.Dark) {
            GameObject.FindObjectOfType<SceneSelectionCanvas>().SendMessage("FadeToBlack");
        } else if (fadeType == FadeType.Light) {
            GameObject.FindObjectOfType<SceneSelectionCanvas>().SendMessage("FadeToWhite");
        }
        // Disable player movement
        sceneSwitchPrep(player);
        StartCoroutine(LoadDoorScene());
    }
    public IEnumerator LoadDoorScene() {
        yield return new WaitForSeconds(2);
        preSceneSwitch(player);
        player.SendMessage("PrepSceneSwitchFade", fadeType);
        Debug.Log("LOADING: " + getSceneToLoad());
        Debug.Log(this.gameObject);
        SceneManager.LoadScene(getSceneToLoad(), LoadSceneMode.Single);
       
    }

    // Return the name of the scene to load, used by SceneManager
    public abstract string getSceneToLoad();

    // What needs to be done when the player is frozen, about to leave the dungeon
    public abstract void sceneSwitchPrep(GameObject player);

    // The scene is about to switch this frame. Do whatever you need to do.
    public abstract void preSceneSwitch(GameObject player);

 
}
