using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ItemStats {
    public Hittable.DamageType damageType;
    public StatsController playerStats;
	public float timeToAttack; //Time inbetween pressing button and actually attacking
	public float timeToCooldown; //Time between end of attack and next attack
	public float range;
	public float baseDamage; //The damage the weapon would do if at 100% condition
    public int animationType;
    public bool bothHands = false;
    public float storedAmmo = 10.0f;
    private bool busy; // If busy, like reloading or something
    private float timeSincePress;
    public Animator myAnim;
    private Animator playerAnim;
	private Transform lookObj;
    private string controllerSide; // String determining if we are a R or L item, used in animator
    public abstract void Attack(bool mouseDown);
    public bool getBusy() { return busy; }
    public float getTimeSincePress() { return timeSincePress; }
    public void setTimeSincePress(float f) { timeSincePress = f; }
    public void setLookObj(Transform t) { lookObj = t; }
    public Transform getLookObj() { return lookObj; }
    public string getControllerSide() { return controllerSide; }
    public void setControllerSide(string s) { controllerSide = s; }
    public void setPlayerAnim(Animator a) { playerAnim = a; }
    public Animator getPlayerAnim() { return playerAnim; }
}
