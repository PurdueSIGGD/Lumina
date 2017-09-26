using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnLevelLoad : MonoBehaviour {
    /**
     * Used for objects that are in the DontDestroyOnLoad scene, but should still be destoryed eventually
     */

	// Use this for initialization
	void Start () {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (this.gameObject) Destroy(this.gameObject);
    }
    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
}
