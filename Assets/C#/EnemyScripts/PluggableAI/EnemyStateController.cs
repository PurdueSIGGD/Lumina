using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************************************
 * 
 * EnemyStateController()
 * support Pluggable AI
 * 
 ******************************************************************************/


public class EnemyStateController : MonoBehaviour {

    public EnemyState currentState;
    public EnemyState placeHolderState;

    [HideInInspector] public BaseEnemy enemy;
    public void SetupStateController(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateStateController()
    {
        currentState.UpdateState(this);
    }

    public void TransitionToNextState(EnemyState nextState)
    {
        if (nextState == placeHolderState)
            return;

        currentState.OnExitState(this);
        currentState = nextState;
    }
}
