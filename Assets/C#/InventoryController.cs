using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    public Animator viewmodelAnimator;
    private float interactCooldown;

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
                viewmodelAnimator.SetTrigger("RAttack");
                Debug.Log("Boop");
                interactCooldown = Time.timeSinceLevelLoad;
            }
        } 

    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle picking up smaller items (upgrade kits/potions)
    }
}
