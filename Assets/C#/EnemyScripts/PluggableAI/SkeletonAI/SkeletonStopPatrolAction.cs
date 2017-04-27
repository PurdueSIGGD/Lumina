using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Skeleton/StopPatrol")]
public class SkeletonStopPatrolAction : EnemyAction
{
    public override void Act(EnemyStateController controller)
    {
        SkeletonEnemy skeleton = (SkeletonEnemy)controller.enemy;
        StopPatrol(skeleton);
    }

    private void StopPatrol(SkeletonEnemy skeleton)
    {
        skeleton.isPatrolling = false;
        skeleton.StopWalkingAnimation();
    }
}
