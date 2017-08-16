using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLevel : MonoBehaviour {
    public DungeonGenerator generator;
    public DungeonLevel pastLevel, nextLevel;
    public int direction;   // 0 = right, 1 = forward, 2 = left, 3 = backwards
    public Animator door;
    public GameObject exitPortal;


    public GameObject[] walls;
    public GameObject[] enemies;

    public bool cleared;


    void Update() {
        // Check to see if the player is done
        if (!cleared) {
            bool dead = true;
            if (enemies != null && enemies.Length > 0) {
                // Check to see if they are all dead
                foreach (GameObject g in enemies) {
                    if (g != null) {
                        if (g.GetComponent<BaseEnemy>().health > 0) {
                            dead = false;
                            break;
                        }
                    }
                }
            }
            if (dead) {
                if (nextLevel != null) {
                    door.SetTrigger("Open");
                    nextLevel.gameObject.SetActive(true); // Stop hiding it from us
                    cleared = true;
                } else {
                    cleared = true;
                    GameObject.Instantiate(exitPortal, transform.position, Quaternion.identity);
                }
            }
        }
        
    }

    void NextLevel() {
        // Disable last level

        // Enable next level
    }

}
