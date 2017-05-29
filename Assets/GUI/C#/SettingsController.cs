using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsController : MonoBehaviour {

    public GameObject settingsCanvas;
    public GameObject navigationPanel;
    public EventSystem eventSystem;
    public InputGenerator inputGenerator;

    [Header("Main Menu")]
    public GameObject mainMenuConfirmPanel;
    public GameObject returnToMainMenuEffect;

    [Header("Quit")]
    public GameObject quitConfirmPanel;
    public GameObject quitEffect;

    private bool isOpen;
    private Animator navAnimator;

    private void Start()
    {
        isOpen = false;
        navAnimator = navigationPanel.GetComponent<Animator>();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        inputGenerator.isGamePausing = true;
    }

    public void ResumeGame()
    {
        inputGenerator.isGamePausing = false;
        Time.timeScale = 1;
    }

    public void ToggleSettingsCanvas()
    {
        //if already open
        if (isOpen)
        {
            isOpen = false;
            CloseNavigationPanel();
        }

        //if is close, then open
        else
        {
            isOpen = true;
            OpenNavigationPanel();
        }
    }

    public void OpenNavigationPanel()
    {
        Time.timeScale = 0;
        inputGenerator.isGamePausing = true;

        navigationPanel.SetActive(true);
        navAnimator = navigationPanel.GetComponent<Animator>();
        navAnimator.SetTrigger("Open");
        
    }

    public void CloseNavigationPanel()
    {
        navAnimator = navigationPanel.GetComponent<Animator>();
        navAnimator.SetTrigger("Close");
        inputGenerator.isGamePausing = false;
        Time.timeScale = 1;

    }

   
    public void SetNavInteractable(bool interactable)
    {
        CanvasGroup canvasGroup = navigationPanel.GetComponent<CanvasGroup>();
        canvasGroup.interactable = interactable;
    }

    public void OnMainMenuButtonClick()
    {
        //no more navigation, have to confirm first
        SetNavInteractable(false);

        //display confirm panel 
        OpenConfirmPanel(mainMenuConfirmPanel);

    }

    public void OnMainMenuNoButtonClick()
    {
        //close the confirm panel, it will disable in the animator
        CloseConfirmPanel(mainMenuConfirmPanel);

        //allow to navigate again
        SetNavInteractable(true);



    }


    public void OnMainMenuYesButtonClick()
    {
        //trigger UI animation
        returnToMainMenuEffect.SetActive(true);
        Animator animator = returnToMainMenuEffect.GetComponent<Animator>();
        animator.SetTrigger("Fade");

        //resume, make sure thing works
        Time.timeScale = 1;
    }

    public void OnQuitButtonClick()
    {
        //disable nav setting
        SetNavInteractable(false);

        //open confirm panel
        OpenConfirmPanel(quitConfirmPanel);
    }

    public void OnQuitButtonNoClick()
    {
        //close the confirm panel, it will disable in the animator
        CloseConfirmPanel(quitConfirmPanel);

        //allow to navigate again
        SetNavInteractable(true);
    }

    public void OnQuitButtonYesClick()
    {
        //trigger UI animation
        quitEffect.SetActive(true);
        Animator anim = quitEffect.GetComponent<Animator>();
        anim.SetTrigger("Fade");
    }

    private void OpenConfirmPanel(GameObject confirmPanel)
    {
       confirmPanel.SetActive(true);
        Animator anim = confirmPanel.GetComponent<Animator>();
        anim.SetTrigger("Open");
    }

    private void CloseConfirmPanel(GameObject confirmPanel)
    {
        //close confirm panel
        Animator anim = confirmPanel.GetComponent<Animator>();
        anim.SetTrigger("Close");

        //deselect everything
        eventSystem.SetSelectedGameObject(null);
    }

}
