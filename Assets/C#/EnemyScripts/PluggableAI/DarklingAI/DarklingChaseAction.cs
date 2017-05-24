using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************************
 * 
 * Darkling Chase: 
 * only involve chase the target, no attack action involved
 * 
 ******************************************************************************/


[CreateAssetMenu(menuName = "PluggableAI/Actions/Darkling/Chase")]
public class DarklingChaseAction : EnemyAction
{

    public override void Act(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;

        Chase(darkling);
    }

    void Chase(DarklingAirEnemy darkling)
    {
        //if this is first time enter attack-mode
        if (!darkling.isChasing)
        {
            darkling.isChasing = true;
            darkling.flyTimeElapsed = 0; //reset time to target
            darkling.teleTimeElapsed = 0;
            darkling.isAllowedToTeleport = false;
            darkling.isTeleporting = false;
        }

        //careful with null refence
        if (darkling.target == null)
            return;

        //because animation is slow compared with CPU
        if (darkling.isTeleporting && darkling.CheckIfTeleCountDownElapsed(darkling.timeBeforeAppear))
        {

            darkling.transform.position = darkling.destination;
            darkling.StartTeleAnimationStop();
            darkling.isTeleporting = false;
            darkling.isAllowedToTeleport = false;
            GameObject g =
                Instantiate(darkling.teleParticles, darkling.transform.position, Quaternion.identity);
            Destroy(g, 3);
        }

        //if darkling is in teleport mode, don't move or count time
        if (darkling.isTeleporting)
        {
            return;
        }

        //if darkling has fly long enough, allow it to teleport
        if (darkling.CheckIfFlyCountDownElapsed(darkling.timeBetweenTele))
        {
            darkling.isAllowedToTeleport = true;
        }

        //if not close enough, narrow down distance
        if (!darkling.isCloseEnoughToTarget(darkling.target.position, darkling.distanceMeleeAttack))
        {
            //move toward target
            darkling.MoveToward(darkling.target.position, darkling.movementSpeed * 2); //make it fly faster
            
            //give target some seconds to run before teleport
            if (darkling.isAllowedToTeleport)
            {
                //basically fly to front of target
                darkling.transform.LookAt(darkling.target);

                Vector3 newPos =
                    darkling.target.position + (-1) * darkling.transform.forward.normalized * darkling.distanceMeleeAttack * 2;

                //use this way so that target have sometime is run 
                //before darkling pop up
                darkling.StartTeleAnimationStart();
                darkling.destination = newPos;
                darkling.isTeleporting = true;
                darkling.teleTimeElapsed = 0;

                //reset fly countdown
                darkling.flyTimeElapsed = 0;
            }
            return;

        }

      



    }
}
