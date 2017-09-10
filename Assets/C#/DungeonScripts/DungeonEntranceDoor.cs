using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntranceDoor : Door {
    private const string dungeonName = "Dungeon";

    // Seed: the unique value that determines the dungeon layout. 
    // Depth: how many levels the dungeon will have
    public int seed, depth;
    public ParticleSystem difficultySmoke;

    public void Start() {
        
    }

    public override string getSceneToLoad() {
        return dungeonName;
    }

    public override void preSceneSwitch(GameObject player) {
        // Set player camera values, enable movement, etc

        // So we know where to return
        PlayerPrefs.SetString(Door.surfaceIdentifier, SceneManager.GetActiveScene().name);

        player.SendMessage("EnterDungeon");

        PlayerPrefs.SetInt("DungeonSeed", seed);
        PlayerPrefs.SetInt("DungeonDepth", depth);
        PlayerPrefs.SetInt("DungeonDifficulty", SceneManager.GetActiveScene().buildIndex);

    }

    public override void sceneSwitchPrep(GameObject player) {
        player.SendMessage("PrepareToEnterDungeon");
    }
}
