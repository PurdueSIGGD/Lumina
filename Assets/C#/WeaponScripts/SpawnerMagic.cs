using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMagic : BurstMagic {
    public GameObject itemToSpawn;
    public GameObject spawnParticles;
    public float spawnHeight = 1; // The height of the object to start at (distance from floor to center of mass)

    public override string getBlurb() {
        return "Magic Draw: " + magicDraw;
    }
    public override void MagicBurstAttack() {
        float minDistance = range;
        Vector3 targetPosition = getLookObj().position + getLookObj().forward * range;
        //Debug.Log(getLookObj().forward);
        //Debug.DrawRay(getLookObj().position, getLookObj().forward * range);
        RaycastHit[] hits = Physics.RaycastAll(new Ray(getLookObj().position, getLookObj().transform.forward), range);
        //RaycastHit[] hits = Physics.CapsuleCastAll(getLookObj().transform.position, getLookObj().transform.position + getLookObj().transform.forward * range, 1, getLookObj().transform.forward);
        foreach (RaycastHit hit in hits) {
            if (hit.distance <= range &&
                hit.collider.gameObject.tag != "Player" &&
                !hit.collider.isTrigger) {
                print(hit.transform.name);
                // Find the minimum distance to travel
                //if (hit.distance < range) 
                //targetPosition = hit.point;    
                targetPosition = getLookObj().position + getLookObj().forward * hit.distance + Vector3.up * spawnHeight; //slightly upwards so no clipping
            }
        }

        //Debug.Log(targetPosition);
        GameObject.Instantiate(itemToSpawn, targetPosition, Quaternion.identity);
        GameObject.Instantiate(spawnParticles, targetPosition, Quaternion.identity);
        //EditorApplication.isPaused = true;
    }
}
