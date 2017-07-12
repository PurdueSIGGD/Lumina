using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLevel : MonoBehaviour {
    public DungeonGenerator generator;
    public DungeonLevel pastLevel, nextLevel;
    public int direction;   // 0 = right, 1 = forward, 2 = left, 3 = backwards


    public GameObject[] walls;
    public GameObject[] enemies;


    void Update() {
        // Check to see if the player is done
    }

    void NextLevel() {
        // Disable last level

        // Enable next level
    }

}
