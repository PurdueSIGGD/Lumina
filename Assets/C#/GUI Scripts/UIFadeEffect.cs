using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFadeEffect : UIEffect
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public override void Play()
    {
        //just in case
        gameObject.SetActive(true);

        //play effect
        anim.SetTrigger("Fade");
    }
}
