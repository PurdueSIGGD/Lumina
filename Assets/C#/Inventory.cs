using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	List<InventoryItem> inventory = new List<InventoryItem>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void addToInventory(InventoryItem iI){
		inventory.Add (iI);
	}

	void printInventory(){
		foreach(InventoryItem iI in inventory){
			Debug.Log(iI);
		}
	}
}
