using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************************
 * 
 * SkeletonStopAttackAction()
 * 
 * specifically designed for skeleton
 * 
 ******************************************************************************/

[CreateAssetMenu(menuName = "PluggableAI/Actions/Skeleton/StopAttack")]
public class SkeletonStopAttackAction : EnemyAction
{
    public override void Act(EnemyStateController controller)
    {

        //simple cast
        SkeletonEnemy skeleton = (SkeletonEnemy)controller.enemy;

        //stop attack
        StopAttack(skeleton);
    }

    private void StopAttack(SkeletonEnemy skeleton)
    {
        skeleton.StopRunningAnimation();
        skeleton.isAttacking = false;
        
    }
}
