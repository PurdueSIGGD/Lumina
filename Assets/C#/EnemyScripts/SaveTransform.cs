using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************************
 * 
 * Simple class to save information of Transform at certain point in game
 * 
 ******************************************************************************/
public class SaveTransform {

    public Vector3 position { get; set; }
    public Quaternion rotation { get; set; }
    public Vector3 scale { get; set; }

    public SaveTransform(Transform transform)
    {
        this.position = transform.position;
        this.rotation = transform.rotation;
        this.scale = transform.localScale;
    }
}
