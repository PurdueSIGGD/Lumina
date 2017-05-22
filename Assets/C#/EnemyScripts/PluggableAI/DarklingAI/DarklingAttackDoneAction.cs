using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************************
 * 
 * Darkling Attack Done
 * 
 * just turn off some variables to make sure
 * 
 ******************************************************************************/

[CreateAssetMenu(menuName = "PluggableAI/Actions/Darkling/Attack Done")]
public class DarklingAttackDoneAction : EnemyAction
{
    public override void Act(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;

        darkling.isAttacking = false;
        darkling.isChasing = false;
        
    }
}
