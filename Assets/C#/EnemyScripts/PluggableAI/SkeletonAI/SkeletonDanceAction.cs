using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Skeleton/Dance")]
public class SkeletonDanceAction : EnemyAction {


    public override void Act(EnemyStateController controller)
    {
        SkeletonEnemy skeleton = (SkeletonEnemy)controller.enemy;
        Dance(skeleton);
    }

    private void Dance(SkeletonEnemy skeleton)
    {
        //if skeleton is already dancing, don't do anything
        if (skeleton.isDancing)
            return;

        skeleton.StartDancingAnimation();
    }

}
