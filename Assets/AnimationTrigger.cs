using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour {
    public Animator target;
    public string triggerName;
	void OnTriggerEnter(Collider col) {
        if (col.transform.tag == "Player") {
            target.SetTrigger(triggerName);
        }
    }
}
