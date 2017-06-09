using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************
 * 
 * SkeletonPatrolAction()
 * 
 * specifically designed for skeleton
 * 
 ******************************************************************************/

[CreateAssetMenu(menuName ="PluggableAI/Actions/Skeleton/Patrol")]
public class SkeletonPatrolAction : EnemyAction
{
    public override void Act(EnemyStateController controller)
    {
        SkeletonEnemy skeleton = (SkeletonEnemy)controller.enemy;

        //simple check
        if (skeleton.patrolPositions.Length == 0)
            return;

        //move
        Patrol(skeleton);
    }

    private void Patrol(SkeletonEnemy skeleton)
    {
        Vector3 destination = skeleton.patrolPositions[skeleton.curPatrolIndex].position;
        if (skeleton.isNearDestination(destination))
        {
            int nextPatrolIndex = (skeleton.curPatrolIndex + 1) % skeleton.patrolPositions.Length;
            skeleton.curPatrolIndex = nextPatrolIndex;
            destination = skeleton.patrolPositions[nextPatrolIndex].position;
        }

        skeleton.isPatrolling = true;
        bool succeed = skeleton.StartWalkingAnimation();
        if (succeed)
        {
            skeleton.MoveToward(destination, skeleton.movementSpeed);
        }
        
    }

}
