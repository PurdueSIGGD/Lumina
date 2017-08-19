using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine.SceneManagement;

public class PlayerSingleton : MonoBehaviour {
    /**
     * This is a class designed to only have one item spawn where this script is attached, regardless of when the scene is opened or not
     * Good for scenes you return to, to preserve information or items that may have been used/expired
     * 
     * A Player Singleton is where you want only ONE of this item in the ENTIRE GAME. Good for players, cameras, or special entities.
     */

    public static GameObject staticSpawnedItem;
    public static PlayerSingleton staticSelf;

    public GameObject itemToSpawn;
    public String itemName = "Singleton"; // What to call this object
    public bool onlyThisScene = true; // Do we hide it on other scenes?
    public int[] restrictedScenes; // Delete yourself in these scenes

    private int sceneIndex; // When this item was spawned (to bring it back later);

    

    void Start() {

        if (!staticSelf)
            staticSelf = this;

        if (staticSelf == this) {
            DontDestroyOnLoad(this.gameObject);
            if (staticSpawnedItem != null) {
                
               
            } else {
                print("Spawning singleton: " + itemName);
                sceneIndex = SceneManager.GetActiveScene().buildIndex;
                staticSpawnedItem = GameObject.Instantiate(itemToSpawn, transform.position, Quaternion.identity);
                staticSpawnedItem.name = itemName;
                DontDestroyOnLoad(staticSpawnedItem); // So we can delete it manually ourselves
            }
        } else {
            print("nope");
            GameObject.Destroy(this.gameObject);
        }



    }

    void OnLevelWasLoaded() {
       
        if (onlyThisScene) {
            if (SceneManager.GetActiveScene().buildIndex == sceneIndex) {
                staticSpawnedItem.SetActive(true);
            } else {
                staticSpawnedItem.SetActive(false);
            }
        }
        bool inRestrictedScene = Array.FindAll(restrictedScenes, s => s == SceneManager.GetActiveScene().buildIndex).Length > 0;
        if (inRestrictedScene) {
            // Delete player, and this        
            // We can't simply disable, as event systems have hissy fits with that
            GameObject.Destroy(staticSpawnedItem);
            staticSelf = null;
            GameObject.Destroy(this.gameObject);

        }

    }
    public static void RemoveAndClearPlayer() {
        GameObject.Destroy(staticSpawnedItem);
        GameObject staticSelfToDelete = PlayerSingleton.staticSelf.gameObject;
        staticSelf = null;
        GameObject.Destroy(staticSelfToDelete);
    }

}
