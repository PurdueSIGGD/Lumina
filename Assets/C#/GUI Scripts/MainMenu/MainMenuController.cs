using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {


    public GameObject currentSidePanel; //side panel beside MainMenuPanel

    private void Start()
    {
        currentSidePanel = null;
    }


    public void LoadScene(string sceneName)
    {
        Debug.Log("LOAIDING " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int index) {
        Debug.Log("LOAIDING " + index);
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    /*
     * this function is usually used in UnityEditor directly 
     * from the button OnClick event
     */
    public void OpenOrClosePanel(GameObject panel)
    {
       //if the panel is already open, close it
        if (currentSidePanel != null && panel == currentSidePanel)
        {
            ClosePanel(panel);
            currentSidePanel = null;
            return;
        }

        //if the new one is different from old one
        if (currentSidePanel != null)
        {
            ClosePanel(currentSidePanel);
        }

        OpenPanel(panel);
        currentSidePanel = panel;

    }


    private void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);

        Animator animator;
        CanvasGroup canvasGroup;

        //open new side panel
        animator = panel.GetComponent<Animator>();
        animator.SetTrigger("Open");

        canvasGroup = panel.GetComponent<CanvasGroup>();
        canvasGroup.interactable = true;

        
    }

    private void ClosePanel(GameObject panel)
    {
        Animator animator;
        CanvasGroup canvasGroup;

        //close the side panel
        animator = panel.GetComponent<Animator>();
        animator.SetTrigger("Close");

        //set it not interactable
        canvasGroup = panel.GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;

        //panel.SetActive(false);
    } 


}
