using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProbabililtyItem {
    /**
     * A class used for describing an item's probability, and the item to spawn
     */
     // The item you want to specify to be spawned
    public GameObject prefab;
    // A non-negative integer describing the chance this item has to be spawned
    // i.e. 1 if normal, 2 if twice as normal, etc.
    public uint chance = 1;
}
