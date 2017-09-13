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
    public bool isCleared;
    public ParticleSystem difficultySmoke;
    public ParticleSystem[] clearedParticles;

    public void Start() {
        if (!isCleared && PlayerPrefs.HasKey(seed.ToString())) {
            isCleared = 1 == PlayerPrefs.GetInt(seed.ToString());
        }

        if (!isCleared) {
            difficultySmoke.Play();
            foreach (ParticleSystem p in clearedParticles)
                p.Stop();
        } else {
            foreach (ParticleSystem p in clearedParticles)
                p.Play();
            difficultySmoke.Stop();
        }
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
