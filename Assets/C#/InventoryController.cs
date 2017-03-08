using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    private float interactCooldown;
	Vector3 cameraAim;
	RaycastHit hitObj;//the hopefully raycast of an item that it finds

	void Start () {
		interactCooldown = 1;
	}
    public void Interact(bool value)
    {
        // If value is true, pick up
        if (value)
        {
            if (Time.timeSinceLevelLoad - interactCooldown > 1)
            {
                interactCooldown = Time.timeSinceLevelLoad;
            }

			cameraAim = GetComponentInChildren<Camera> ().transform.rotation.eulerAngles;
			if (Physics.Raycast (this.transform.position, cameraAim,out hitObj)) {
				string itemTag = hitObj.collider.gameObject.tag;
				if ( itemTag == "Armor") {
					Inventory I = gameObject.GetComponentInParent<Inventory> ();
					Armor Get = hitObj.collider.gameObject.GetComponentInParent<Armor> ();
					I.pickUpItem (Get);
					Debug.Log ("player is Grabbing Armor ");
				}
			}
        } 

    }

    private void OnTriggerEnter(Collider other)
    {
			if(other.gameObject.GetComponentInParent<Pickup>()!= null){
			Pickup Get = other.gameObject.GetComponentInParent<Pickup> ();
			Debug.Log ("Player picked up " + Get.itemType);

		}
    }
}
