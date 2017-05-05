using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Darkling/MeleeAttack")]
public class DarklingMeleeAttackAction : EnemyAction
{
    public override void Act(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;

        Attack(darkling);
    }

    private void Attack(DarklingAirEnemy darkling)
    {
        //if this is first time enter attack-mode
        if (!darkling.isAttacking)
        {
            darkling.isAttacking = true;
            darkling.flyTimeElapsed = 0; //reset time to target
        }

        //careful with null refence
        if (darkling.target == null)
            return;

        //if not close enough, narrow down distance
        if (!darkling.isCloseEnoughToTarget(darkling.target.position, darkling.distanceMeleeAttack))
        {
            darkling.MoveToward(darkling.target.position, darkling.movementSpeed);
            darkling.isAllowedToAttack = true;

            //give target some seconds to run before teleport
            if (darkling.CheckIfFlyCountDownElapsed(darkling.timeBetweenTele))
            {
                //basically fly to front of target
                Vector3 newPos =
                    darkling.target.position + (-1) * darkling.transform.forward.normalized * darkling.distanceMeleeAttack * 2;
                darkling.transform.position = newPos;

                //reset fly countdown
                darkling.flyTimeElapsed = 0;
            }
          
        }

        //else if close enough
        else
        {
            //attack
            if (darkling.isAllowedToAttack)
            {
                //damage target
                Hittable h;
                if (darkling.health > 0 && (h = darkling.target.GetComponent<Hittable>()))
                {
                    h.Hit(darkling.attackDamage, darkling.attackType);
                }

                //play attack animations
                darkling.StartMeleeAttackAnimation();
                darkling.isAllowedToAttack = false;

                //if target still within Melee zone, reset count down
                darkling.attackTimeElapsed = 0;
            }

            //after reaching ,and attack target, darkling will run away
            darkling.isAllowedToEscape = true;

            //give target some time before next attack.
            //this code will never executed but who knows
            if (darkling.CheckIfAttackCountDownElapsed(darkling.timeBetweenAttacks))
            {
                darkling.isAllowedToAttack = true;
            }
        }                       
    }
        

        
               
    
}
