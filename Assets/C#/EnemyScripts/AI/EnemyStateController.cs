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
    public EnemyState remainState;

    [HideInInspector] public BaseEnemy enemy;
    public void SetupStateController(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateStateController()
    {
        currentState.UpdateState(enemy);
    }
}
