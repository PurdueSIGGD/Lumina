using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBehavior : StateMachineBehaviour {
    public string sourceName;
    bool hit = false;
    public bool playsInMiddle;
    public bool looping;
    public bool muteOnDeath;
    public bool notForceStop;
    public float delay;
    RandomAudioSource target;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        target = animator.transform.GetComponentInChildren<RandomAudioSource>().transform.parent.Find(sourceName).GetComponentInChildren<RandomAudioSource>();
        if ((!muteOnDeath || !GameObject.FindObjectOfType<StatsController>().dead) && !looping) {
            target.looping = false;
            target.delay = delay;
            target.PlayOnce();
        } else if (looping) {
            AudioSource a;
            if (!(a = target.GetComponent<AudioSource>()).isPlaying) {
                target.delay = 1;
                a.loop = true;
                target.PlayOnce();
            }
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (playsInMiddle && stateInfo.normalizedTime > .5f) {
            if (!hit) {
                target.PlayOnce();
                hit = true;
            }
        } else {
            hit = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        RandomAudioSource target = GameObject.Find(sourceName).GetComponent<RandomAudioSource>();
        
        if (target && !looping && !notForceStop) target.Stop();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
