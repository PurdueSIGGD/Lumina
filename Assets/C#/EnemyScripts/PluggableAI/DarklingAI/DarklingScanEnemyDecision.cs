using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/******************************************************************************
 * 
 * Darkling Simple Scan Enemy
 * 
 * if there is an enemy reach TriggerEnter, return true
 *  
 ******************************************************************************/

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Darkling/Scan Enemy")]
public class DarklingScanEnemyDecision : EnemyDecision
{
    public override bool Decide(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;
        bool targetDetected = Scan(darkling);
        return targetDetected;
    }

    private bool Scan(DarklingAirEnemy darkling)
    {
        if (darkling.target == null)
            return false;
        return true;
    }
}
