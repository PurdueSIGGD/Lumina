using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************
 * 
 * SkeletonAttackAction()
 * 
 * specifically designed for skeleton
 * 
 ******************************************************************************/

[CreateAssetMenu(menuName = "PluggableAI/Actions/Skeleton/Attack")]
public class SkeletonAttackAction : EnemyAction
{
    public override void Act(EnemyStateController controller)
    {
        //simple cast
        SkeletonEnemy skeleton = (SkeletonEnemy)controller.enemy;

        //move
        Attack(skeleton);
    }

    private void Attack(SkeletonEnemy skeleton)
    {
        skeleton.isAttacking = true;

        //careful with null refence
        if (skeleton.target == null)
            return;

        //narrow down distance to target
        if (!skeleton.isCloseEnoughToTarget(skeleton.target.position, skeleton.distanceMeleeAttack))
        {
            bool succeed = skeleton.StartRunningAnimation();
            if (succeed)
            {
                //running is faster than walking
                float runningSpeed = skeleton.movementSpeed * 2;
                skeleton.MoveToward(skeleton.target.position, runningSpeed);
            }

            //if target get outside zone, skeleton is allowed to attack
            //skeleton.isAllowedToAttack = true;
        }

        //if close enough, 
        else
        {
            //attack him/her
            if (skeleton.isAllowedToAttack)
            {
                Hittable h;
                if (skeleton.health > 0 && (h = skeleton.target.GetComponent<Hittable>())) {
                    //Debug.Log("Skeleton attack");
                    h.Hit(skeleton.attackDamage, skeleton.attackType);
                }
                //play attack animation
                skeleton.animator.SetTrigger(skeleton.HASH_TRIGGER_ATTACK_RUN);               
                skeleton.isAllowedToAttack = false;

                //if target still stay in zone, reset count down
                skeleton.stateTimeElapsed = 0;
            }
                
            //stop skeleton if it is running
            if (skeleton.isRunning)
                skeleton.StopRunningAnimation();
                                  
            //give it some time before attack
            if (skeleton.CheckIfCountDownElapsed(skeleton.timeBetweenAttacks))
            {
                skeleton.isAllowedToAttack = true;
            }
            
            
        }

        

    }
}
