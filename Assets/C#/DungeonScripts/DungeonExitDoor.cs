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

        // TODO: save the dungeon state into playerPrefs
        // TODO: discuss with team for expected behavior
    }

    public override void sceneSwitchPrep(GameObject player) {
        player.SendMessage("PrepareToExitDungeon");

    }
}
