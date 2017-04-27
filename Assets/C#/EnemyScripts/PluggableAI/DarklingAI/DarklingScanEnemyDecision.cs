using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Darkling/Scan Enemy")]
public class DarklingScanEnemyDecision : EnemyDecision
{
    public override bool Decide(EnemyStateController enemy)
    {
        return false;
    }

    private void Scan()
    {

    }
}
