using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BurstMagic : Magic {

    public float magicDraw = 1; //Magic this attack burst takes

    private bool attacking = false;
    private bool bursted = false;
    public override void MagicAttack(bool mouseDown) {
        if (mouseDown) {
            if (attacking) {
                if (!bursted && getPlayerAnim().GetCurrentAnimatorStateInfo(2).IsTag("magicAttack")) {
                    // turn on the burst
                    playerStats.UpdateMagic(-1 * magicDraw);
                    shootParticles.Play();
                    MagicBurstAttack();
                    getPlayerAnim().SetBool(getControllerSide() + "MagicAttack", false);
                    attacking = false;
                    bursted = true;
                }
            } else if (playerStats.GetMagic() >= magicDraw) {
                attacking = true;
                getPlayerAnim().SetBool(getControllerSide() + "MagicAttack", true);
            }
        } else {
            bursted = false;
            if (attacking) {
                getPlayerAnim().SetBool(getControllerSide() + "MagicAttack", false);
                attacking = false;
            }
        }


    }

    public abstract void MagicBurstAttack();

}
