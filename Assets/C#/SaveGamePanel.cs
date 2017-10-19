using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGamePanel : MonoBehaviour {

    public void SaveGame() {
        // don't open, save
        GameObject.FindGameObjectWithTag("Player").SendMessage("SaveGame");
    }

}
