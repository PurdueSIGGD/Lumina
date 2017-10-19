using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    public Sprite image;
	// Use this for initialization
	public void Start () {
        StartCoroutine("TutorialMethod");
	}
	
	IEnumerator TutorialMethod() {
        yield return new WaitForSeconds(1);
        NotificationStackController.PostNotification("Welcome to Lumina!", image);
        yield return new WaitForSeconds(5f);
        NotificationStackController.PostNotification("Clear out dungeons to find better equipment, and explore new worlds!", image);
        yield return new WaitForSeconds(5f);
        NotificationStackController.PostNotification("Press 'tab' to pause the game, save, or quit", image);
        yield return new WaitForSeconds(5f);
        NotificationStackController.PostNotification("Use W,A,S,D to move around. Press shift to sprint, and space to jump.", image);
        yield return new WaitForSeconds(5f);
        NotificationStackController.PostNotification("Press 'F' to equip weapons and interact with objects.", image); 
        yield return new WaitForSeconds(5f);
        NotificationStackController.PostNotification("Use 'E' to switch weapons, and 'Q' to switch magic powers.", image);
        yield return new WaitForSeconds(5f);
        NotificationStackController.PostNotification("You can equip many different items. Press 'I' to look in your inventory.", image);
        yield return new WaitForSeconds(5f);
        NotificationStackController.PostNotification("Your items can wear out, so find a repair kit to fix them.", image);
        yield return new WaitForSeconds(5f);
        NotificationStackController.PostNotification("To upgrade an item, click on it, and click 'Upgrade' to the right", image);
        yield return new WaitForSeconds(5f);
        NotificationStackController.PostNotification("There are also upgrade potions for your max health, magic, and light!", image);
        yield return new WaitForSeconds(5f);
        NotificationStackController.PostNotification("Exploring dungeons consumes LIGHT, so keep an eye on it!", image);
        yield return new WaitForSeconds(5f);
        NotificationStackController.PostNotification("To explore harder islands, press 'F' on your boat.", image);
        yield return new WaitForSeconds(5f);
        NotificationStackController.PostNotification("To see this tutorial again, click on Settings->Replay Tutorial", image);
        Destroy(this.gameObject);
    }
}
