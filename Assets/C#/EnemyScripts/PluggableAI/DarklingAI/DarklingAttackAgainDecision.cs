using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************
 * 
 * Darkling Atack Again
 * 
 * basically if it has done attack and target not escape, 
 * then this darkling with turn around, chase & attack again
 * 
 ******************************************************************************/


[CreateAssetMenu(menuName = "PluggableAI/Decisions/Darkling/Attack Again")]

public class DarklingAttackAgainDecision : EnemyDecision
{
    public override bool Decide(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;

        bool attackAgain = false;
        if (darkling.target != null && darkling.hasDoneAttacking)
        {
            attackAgain = true;
        }

        return attackAgain;
    }
}
