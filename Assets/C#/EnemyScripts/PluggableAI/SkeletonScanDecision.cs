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
     * just use TriggerEnter for now
     */ 
    private bool Scan(SkeletonEnemy skeleton)
    {
        if (skeleton.target != null)
            return true;
        return false;
    }
   
}
