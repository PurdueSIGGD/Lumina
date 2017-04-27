using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************
 * 
 * DarklingAirEnemy
 * 
 * WanderDoneDecision: change if darkling need to finish his wandering action
 * 
 * and may find new destination
 *
 ******************************************************************************/
[CreateAssetMenu(menuName = "PluggableAI/Decisions/Darkling/Wander Done")]
public class DarklingWanderDoneDecision : EnemyDecision
{
    public override bool Decide(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;
        return darkling.isNearDestination(darkling.destination);
    }
}
