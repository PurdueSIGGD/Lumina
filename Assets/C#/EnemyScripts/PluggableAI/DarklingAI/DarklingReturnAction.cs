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

[CreateAssetMenu(menuName = "PluggableAI/Actions/Darkling/ReturnToStartPoint")]
public class DarklingReturnAction : EnemyAction
{
    public override void Act(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;

        GoBack(darkling);
    }

    private void GoBack(DarklingAirEnemy darkling)
    {
      
        darkling.MoveToward(darkling.idlePosition, darkling.movementSpeed);

        //if close enough, snap darkling to target
        if (darkling.isCloseEnoughToTarget(darkling.idlePosition, 3))
        {
            darkling.transform.position = darkling.idlePosition;
            darkling.transform.rotation = darkling.idleRotation;
        }

    }
}
