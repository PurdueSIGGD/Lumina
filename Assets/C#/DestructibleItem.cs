using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DestructibleItem : Hittable {
    /**
     * This is a class I created to help you guys practice out hitting.
     * If your hit method works, it will destroy items such as this.
     * All it does right now is fall apart into a fractured block.
     * Try it out!
     * 
     * Gibs: If you assign gibs, they will be instantiated when the object is destroyed.
     * Otherwise, you can leave it empty and make a gameobject of the gibs as the child of the object, which is better for performance.
     */
    
    public GameObject gibs; 


    public override void Hit(float f, Vector3 direction, Hittable.DamageType damage) {

        //print("was hit");
        this.GetComponent<Collider>().isTrigger = true;
        Rigidbody myRigid = this.GetComponent<Rigidbody>();

        GameObject spawn;
        if (gibs) {
            spawn = (GameObject)GameObject.Instantiate(gibs, transform.position, transform.rotation);
        } else if (transform.childCount == 1) {
            // We use the child instead (much faster than creating a new object)
            spawn = transform.GetChild(0).gameObject;
            spawn.transform.parent = null;
            spawn.SetActive(true);
        } else {
            Debug.LogError("You did not have any gibs assigned! Make a child and disable it, or assign a prefab!");
            return;
        }
        Vector3 myVelocity = myRigid.velocity;
        
        foreach (Rigidbody r in spawn.GetComponentsInChildren<Rigidbody>()) {
            r.velocity = myRigid.GetPointVelocity(r.transform.position);
        }
        GameObject.Destroy(this.gameObject);

    }
    void Start() {
		
        //this.gameObject.GetComponent<Hittable>().Hit();
    }
}
