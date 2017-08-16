using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BurstMagic : Magic {
    // Level restriction used for use on certain levels (like portal spawner only in dungeons)
    public bool useLevelRestriction;
    public string levelRestriction = "Dungeon";

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
            } else if (playerStats.GetMagic() >= magicDraw && (!useLevelRestriction || SceneManager.GetActiveScene().name == levelRestriction)) {
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
