using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Darkling/Scan Destination Done")]
public class DarklingScanDestDoneDecision : EnemyDecision
{
    public override bool Decide(EnemyStateController controller)
    {
        DarklingAirEnemy darkling = (DarklingAirEnemy)controller.enemy;
        bool hasFoundDestination = !darkling.destinationPending;
        return hasFoundDestination;
    }

  
}
