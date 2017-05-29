using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Usable : MonoBehaviour {
    /**
     * Do this when the player interacts with this item
     */
    public abstract void Use();
    /**
     * Text to display when the player hovers over the item
     */
    public abstract string getInfoText();

}
