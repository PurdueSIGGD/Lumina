using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************************
 * 
 * Skeleton Return Action
 * 
 * skeleton will walk back to the position where it is initially
 * 
 ******************************************************************************/


[CreateAssetMenu(menuName = "PluggableAI/Actions/Skeleton/Return(Walk back)")]
public class SkeletonReturnAction : EnemyAction
{
    public override void Act(EnemyStateController controller)
    {
        SkeletonEnemy skeleton = (SkeletonEnemy)controller.enemy;

        WalkBack(skeleton);
    }


    private void WalkBack(SkeletonEnemy skeleton)
    {
        bool succeed = skeleton.StartWalkingAnimation();
        if (succeed)
        {
            skeleton.MoveToward(skeleton.startTransform.position, skeleton.movementSpeed);
        }
        

        //if close enough, snap darkling to target
        if (skeleton.isCloseEnoughToTarget(skeleton.startTransform.position, 3))
        {
            skeleton.transform.position = skeleton.startTransform.position;
            skeleton.transform.rotation = skeleton.startTransform.rotation;
        }
    }
}
