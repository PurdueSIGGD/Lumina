using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour {
    public GameObject player;

	void Start () {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) {
            GameObject spawn = GameObject.Instantiate(player, transform.position, Quaternion.identity);
            spawn.name = "Player";
        }
	}
	
	
}
