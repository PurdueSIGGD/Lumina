using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/State")]
public class EnemyState : ScriptableObject {

    public EnemyAction[] actions;   //actions in a state
    public EnemyTransition[] transitions;   //decisions that enemy will do, to decide to go to next state
    public EnemyAction exitAction;  //action enemy perform when transition out
    public Color sceneGizmoColor = Color.grey;  //use this color to tell designer that "This is not set yet!"

    public void UpdateState(EnemyStateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(EnemyStateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    private void CheckTransitions(EnemyStateController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool transitionSucceed = transitions[i].decision.Decide(controller);

            if (transitionSucceed)
            {
                controller.TransitionToNextState(transitions[i].trueState);
            } else
            {
                controller.TransitionToNextState(transitions[i].falseState);
            }
        }
    }

    /*
     * called by EnemyStateController, when transition to new state
     */ 
    public void OnExitState(EnemyStateController controller)
    {
        if (exitAction == null)
            return;

        exitAction.Act(controller);
    }
}
