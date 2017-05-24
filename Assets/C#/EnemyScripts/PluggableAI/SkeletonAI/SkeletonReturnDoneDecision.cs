using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************************
 * 
 * Skeleton Return Decision
 * 
 * return true when Darkling has reach home
 * 
 * so it can go back to Guard State or something else
 ******************************************************************************/

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Skeleton/Return Done")]
public class SkeletonReturnDoneDecision : EnemyDecision
{
    public override bool Decide(EnemyStateController controller)
    {
        SkeletonEnemy skeleton = (SkeletonEnemy)controller.enemy;

        if (skeleton.isCloseEnoughToTarget(skeleton.startTransform.position, 1))
        {
            return true;
        }

        return false;
    }
}
