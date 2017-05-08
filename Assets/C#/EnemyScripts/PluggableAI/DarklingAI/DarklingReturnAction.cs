using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************************
 * 
 * Darkling Return
 * 
 * fly backs to the position where it was initially
 * 
 ******************************************************************************/


public class DarklingReturnAction : EnemyAction
{
    public override void Act(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;

        GoBack(darkling);
    }

    private void GoBack(DarklingAirEnemy darkling)
    {
        //simple check
        if (darkling.idlePosition == null)
        {
            Debug.Log("Darkling Air: Idle Position is not set");
            return;
        }

        darkling.MoveToward(darkling.idlePosition.position, darkling.movementSpeed);

        //if close enough, snap darkling to target
        if (darkling.isCloseEnoughToTarget(darkling.idlePosition.position, 3))
        {
            darkling.transform.position = darkling.idlePosition.position;
            darkling.transform.rotation = darkling.idlePosition.rotation;
        }

    }
}
