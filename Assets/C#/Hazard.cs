using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {
    private ArrayList toDamage;
    public Hittable.DamageType type;
    public float damage;
    public float rate;
	
	void Start() {
        toDamage = new ArrayList();
    }
	void OnTriggerEnter(Collider col) {
        if (col.isTrigger) return;
        Hittable h;
        if (h = col.GetComponent<Hittable>()) {
            toDamage.Add(h);
            StartCoroutine(DamagingBehavior(h));
        }
    }
    void OnTriggerExit(Collider col) {
        if (col.isTrigger) return;
        Hittable h;
        if (h = col.GetComponent<Hittable>()) {
            toDamage.Remove(h);
        }
    }

    public IEnumerator DamagingBehavior(Hittable h) {
        do {
            h.Hit(damage, type);
            yield return new WaitForSeconds(rate);
        } while (h != null && toDamage.Contains(h) && h.gameObject != null);
    }
}
