using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************
 * 
 * Darkling Melee Attack decision
 * 
 * if target is in Melee Zone, then switch to Melee Attack State
 * 
 ******************************************************************************/


[CreateAssetMenu(menuName = "PluggableAI/Decisions/Darkling/Melee Zone")]
public class DarklingMeleeZoneDecision : EnemyDecision
{
    public override bool Decide(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;

        bool isTargetInZone = detectTarget(darkling);
        return isTargetInZone;
    }

    private bool detectTarget(DarklingAirEnemy darkling)
    {
        if (darkling.target == null)
            return false;

        bool isTargetInZone = darkling.isCloseEnoughToTarget(darkling.target.position, darkling.distanceMeleeZone);
        return isTargetInZone;
    }
}
