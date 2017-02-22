using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* To add an attack effect, simply find the Hittable 
 * class, and if it exists, call the desired hit method.
 * Example: 
 *      Hittable hit;
 *      if (hit = target.GetComponent<Hittable>()) {
 *              hit.Hit();
 *      }
 */

abstract public class Hittable : MonoBehaviour {
    public enum DamageType { Neutral, Fire, Ice, Electric };

    /* You only have to implement this one function */

    abstract public void Hit(float damage, Vector3 direction, DamageType type);

    /* Overloading takes care of every other instance that could be called */

    public void Hit(float damage) {
        Hit(damage, Vector3.zero, DamageType.Neutral);
    }

    public void Hit(float damage, Vector3 direction) {
        Hit(damage, direction, DamageType.Neutral);
    }

    public void Hit(float damage, DamageType type) {
        Hit(damage, Vector3.zero, type);
    }

    public void Hit(Vector3 direction) {
        Hit(0, direction, DamageType.Neutral);
    }

    public void Hit() {
        Hit(0, Vector3.zero, DamageType.Neutral);
    }

}
