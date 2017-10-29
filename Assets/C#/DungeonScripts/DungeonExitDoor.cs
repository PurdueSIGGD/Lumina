using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExitDoor : Door {

    public DungeonLevel dungeonLevel;

    public override string getSceneToLoad() {
        return PlayerPrefs.GetString(surfaceIdentifier);
    }

    public override void preSceneSwitch(GameObject player) {
        // Set player camera values, enable movement, etc
        player.SendMessage("ExitDungeon");
        
    }

    public override void sceneSwitchPrep(GameObject player) {
        player.SendMessage("StopMoving");

    }
}
