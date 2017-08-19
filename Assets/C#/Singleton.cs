using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour {
    /**
     * This is a class designed to only have one item spawn where this script is attached, regardless of when the scene is opened or not
     * Good for scenes you return to, to preserve information or items that may have been used/expired
     * Uses PlayerPrefs: if already spawned, set the pref to 1
     */

    public GameObject itemToSpawn;
    public String itemName = "Singleton"; // What to call this object
    public bool setParent = true; // Keep track of the child throughout scenes, otherwise spawn once and forget
    public Guid guid; // Globally unique identifier
    public String guidString;
    private int sceneIndex; // When this item was spawned (to bring it back later);
    private GameObject spawnedObject;

    [CustomEditor(typeof(Singleton))]
    public class ColliderCreatorEditor : Editor {
        override public void OnInspectorGUI() {
            Singleton sg = (Singleton)target;
            if (GUILayout.Button("New Guid")) {
                sg.guid = Guid.NewGuid();
                sg.guidString = sg.guid.ToString();
            }
            // !!!!!README!!!!! if you aren't having anything spawn, then check this button. It will make sure you get a fresh one when you start the game.
            if (GUILayout.Button("Reset PlayerPrefs")) {
                PlayerPrefs.SetInt(sg.guid.ToString(), 0);
            }
            if (PlayerPrefs.GetInt(sg.guid.ToString()) > 0) {
                GUIStyle myStyle = new GUIStyle();
                GUILayout.Label("***PLEASE HIT 'Reset PlayerPrefs' TO SPAWN***");
            }
            DrawDefaultInspector();
        }
    }


    void Start () {
        if (guidString == null) {
            throw new MissingFieldException("Requires a GUID (press \"New Guid\"");
        } else {
            if (guidString != null && guidString != "") {
                guid = new Guid(guidString);
            }
        }

        DontDestroyOnLoad(this);
        print(PlayerPrefs.GetInt(guid.ToString()) + " can it spawn?");
        if (PlayerPrefs.GetInt(guid.ToString()) == 0) {
            print("Spawning singleton: " + itemName + " " + guid.ToString());
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
            spawnedObject = GameObject.Instantiate(itemToSpawn, transform.position, Quaternion.identity);
            if (setParent) spawnedObject.transform.parent = transform;
            spawnedObject.name = itemName;
            DontDestroyOnLoad(spawnedObject); // So we can delete it manually ourselves
            print("setting int for " + itemName + " " + guid.ToString());
            PlayerPrefs.SetInt(guid.ToString(), 1);
        } else {
            // If you are wondering why your item isn't spawning when you open up your game, click "Reset PlayerPrefs"
            print("Not spawning singleton: " + itemName + " " + guid.ToString());
            if (spawnedObject != null) {
                // If we still own it, take care of replacement. Otherwise, who gives a fuck
                if (spawnedObject.transform.parent == transform) {
                    if (SceneManager.GetActiveScene().buildIndex == sceneIndex) {
                        spawnedObject.SetActive(true);
                    } else {
                        spawnedObject.SetActive(false);
                    }
                }
            }
        }

        
       
    }
    void OnDestroy() {
        print("We ded now");
        PlayerPrefs.SetInt(guid.ToString(), 0);
    }

}
