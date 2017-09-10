using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCanvas : MonoBehaviour {
    public Animator gameOverElement;
    public SceneSelectionCanvas sceneSelectionCanvas;
    void GameOver() {
        gameOverElement.SetTrigger("Show");
        sceneSelectionCanvas.fadeToBlackAnimator.SetFloat("FadeSpeed", 0.2f);
        sceneSelectionCanvas.SendMessage("FadeToBlack");
        GameObject.FindObjectOfType<InputGenerator>().EnableCursors();
    }
    void NotGameOver() { 
        GameObject.FindObjectOfType<InputGenerator>().DisableCursors();
        sceneSelectionCanvas.fadeToWhiteAnimator.SetFloat("FadeSpeed", 1f);
        //sceneSelectionCanvas.SendMessage("FadeFromBlack");
        gameOverElement.SetTrigger("Hide");
        
    }

    public void ExitGame() {
        Application.Quit();
    }
    public void MainMenu() {
        // Delete the player that existed before
        PlayerSingleton.RemoveAndClearPlayer();
        SceneManager.LoadScene(0);
    }

}
