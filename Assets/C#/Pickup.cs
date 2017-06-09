using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {


	public enum pickUpType { Magic, Health, Light, upgradePotion, upgradeKit, Arrow };

	public float amount;

	public pickUpType itemType;

}
