using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************
 * 
 * Darkling Chase Done: 
 * 
 * simply turn off some variable
 * 
 ******************************************************************************/


[CreateAssetMenu(menuName = "PluggableAI/Actions/Darkling/Chase Done")]
public class DarklingChaseDoneAction : EnemyAction
{
    public override void Act(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;

        darkling.isChasing = false;
        darkling.isAllowedToTeleport = false;
    }
}
