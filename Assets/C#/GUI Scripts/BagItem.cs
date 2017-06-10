using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to hold information of an bag iten (to easily display in UI)
/// </summary>
public class BagItem : MonoBehaviour {

    public string displayName; //name to display in bag/inventory
    [Multiline] public string description; //description in help panel
    public Sprite icon;

    
}
