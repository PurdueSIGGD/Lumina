using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLooker : MonoBehaviour {
    public float chancePerSecond = 0.1f;
    private Animator myAnim;
    private float lastTime;
	// Use this for initialization
	void Start () {
        myAnim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - lastTime > 1) {
            lastTime = Time.time;
            if (Random.Range(0.0f, 1f) < chancePerSecond) {
                myAnim.SetTrigger("RandomLook");
            }
        }
	}
}
