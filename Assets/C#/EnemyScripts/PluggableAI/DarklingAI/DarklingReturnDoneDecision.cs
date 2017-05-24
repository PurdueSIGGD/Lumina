using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************************
 * 
 * Darkling Return Decision
 * 
 * return true when Darkling has reach home
 * 
 * so it can go back to Guard State or something else
 ******************************************************************************/

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Darkling/Return Done")]
public class DarklingReturnDoneDecision : EnemyDecision
{
    public override bool Decide(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;

      
        if (darkling.isCloseEnoughToTarget(darkling.startTransform.position, 1))
        {
            return true;
        }
        return false;
    }
}
