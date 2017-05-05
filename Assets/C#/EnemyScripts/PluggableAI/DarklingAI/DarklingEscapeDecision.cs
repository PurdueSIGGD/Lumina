using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************
 * 
 * Darkling Escape Decision
 * if darkling is Allowed to Escape, go to next state
 * 
 ******************************************************************************/


[CreateAssetMenu(menuName = "PluggableAI/Decisions/Darkling/Escape")]
public class DarklingEscapeDecision : EnemyDecision
{
    public override bool Decide(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;
        return darkling.isAllowedToEscape;
    }


}
