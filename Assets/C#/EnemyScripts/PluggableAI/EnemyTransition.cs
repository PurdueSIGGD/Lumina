using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyTransition  {

    public EnemyDecision decision;
    public EnemyState trueState;    //if decision.decide() return true, transition to this state
    public EnemyState falseState;   //else this state
}
