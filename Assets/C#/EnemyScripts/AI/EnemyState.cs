using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/State")]
public class EnemyState : ScriptableObject {

    public EnemyAction[] actions;
    //public EnemyTransition[] transitions;
    public Color sceneGizmoColor = Color.grey;

    public void UpdateState(BaseEnemy enemy)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(enemy);
        }
    }
}
