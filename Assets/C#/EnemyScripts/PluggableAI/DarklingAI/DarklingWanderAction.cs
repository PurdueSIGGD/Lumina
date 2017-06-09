using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************
 * 
 * UPGRADE NOTE: Use Addforce to make darkling fly more naturally
 *
 ******************************************************************************/


[CreateAssetMenu(menuName = "PluggableAI/Actions/Darkling/Wander")]
public class DarklingWanderAction : EnemyAction
{
    public override void Act(EnemyStateController controller)
    {
       
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;
        Wander(darkling);
        
    }

    /*
     * Handle moving and teleport
     */ 
    private void Wander(DarklingAirEnemy darkling)
    {

        //teleport
        if (darkling.isAllowedToTeleport)
        {
            //reset time for next teleport
            darkling.flyTimeElapsed = 0;
            darkling.isAllowedToTeleport = false;

            //teleport
            darkling.StartTeleAnimationStart();
            darkling.isTeleporting = true;
            darkling.teleTimeElapsed = 0;
            return;
        }

        //because animation is slow compared with CPU
        if (darkling.isTeleporting && darkling.CheckIfTeleCountDownElapsed(darkling.timeBeforeAppear))
        {
            //darkling.darklingModel.SetActive(false);
            //darkling.darklingModel.SetActive(true);
            darkling.transform.position = darkling.destination;
            darkling.StartTeleAnimationStop();
            darkling.isTeleporting = false;
            GameObject g =
                Instantiate(darkling.teleParticles, darkling.transform.position, Quaternion.identity);
            Destroy(g, 3);
        }

        //if darkling is in disappear mode, don't make it move       
        if (darkling.isTeleporting)
        {            
            return;
        }

        //if too low, increase height
        //if (darkling.GetFlyHeightFromGround() < darkling.flyHeight)
        //{
        //    darkling.transform.position = darkling.transform.position + Vector3.up * darkling.movementSpeed * Time.deltaTime;
        //    darkling.destination = 
        //}

        //else move toward destination       
        darkling.MoveToward(darkling.destination, darkling.movementSpeed);




        //count down time for next teleport
        //ex: fly 4 seconds, then Teleport
        if (darkling.CheckIfFlyCountDownElapsed(darkling.timeBetweenTele))
        {
            darkling.isAllowedToTeleport = true;
        }
    }
}
