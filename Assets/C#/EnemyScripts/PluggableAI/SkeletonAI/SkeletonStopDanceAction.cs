using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Skeleton/StopDance")]
public class SkeletonStopDanceAction : EnemyAction {

    public override void Act(EnemyStateController controller)
    {
        SkeletonEnemy skeleton = (SkeletonEnemy)controller.enemy;
        StopDance(skeleton);
    }

    /*
     * Simple 
     */ 
    private void StopDance(SkeletonEnemy skeleton)
    {
        skeleton.StopDancingAnimation();
    }

}
