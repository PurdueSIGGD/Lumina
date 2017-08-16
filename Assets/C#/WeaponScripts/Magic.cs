using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Magic : Weapon {
    public ParticleSystem idleParticles;
    public ParticleSystem shootParticles;
    public Transform mesh;

    public override void Attack(bool mouseDown) {
        MagicAttack(mouseDown);
    }

    /**
     * MagicAttack is called when Attack is called
     */
    public abstract void MagicAttack(bool mouseDown);

    // Use this for initialization
    void Start () {
        idleParticles.Play();
    }
    public void pauseParticles() {
        if (shootParticles.isPlaying) {
            shootParticles.Stop();
        }
        if (idleParticles.isPlaying) {
            idleParticles.Stop();
        }
    }
    public void playParticles() {
        idleParticles.Play();
    }
    // Update is called once per frame
    void Update () {
		
	}
}
