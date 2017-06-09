using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Skeleton/Scan")]
public class SkeletonScanDecision : EnemyDecision {

    public override bool Decide(EnemyStateController controller)
    {
        SkeletonEnemy skeleton = (SkeletonEnemy)controller.enemy;

        bool targetDetected = Scan(skeleton);
        return targetDetected;
    }

    /*
     * Scan(), use OnTriggerEnter to set target
     * return true if (target is detected && target is alive)
     * return false if not
     */ 
    private bool Scan(SkeletonEnemy skeleton)
    {
        //if there is no target
        if (skeleton.target == null)
            return false;

        //see if target is alive
        StatsController stats;

        if (stats = skeleton.target.GetComponent<StatsController>())
        {
            if (stats.GetHealth() <= 0)
                return false;
        }
       
        return true;

    }
   
}
