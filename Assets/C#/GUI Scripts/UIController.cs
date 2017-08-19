using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public Player player;

    public InventoryCanvas inventoryCanvas;
    public SettingsCanvas settingsCanvas;
    public HUDController hudController;

    [HideInInspector] public Dictionary<KeyCode, UICanvas> keyBindings;

    private InputGenerator inputGenerator;

    /// <summary>
    /// current Open Canvas
    /// </summary>
    public UICanvas currentCanvas { get; set; }
    

    private void Awake()
    {
        //init
        keyBindings = new Dictionary<KeyCode, UICanvas>();
        inputGenerator = player.GetComponent<InputGenerator>();

        //add to dict
        keyBindings.Add(KeyCode.Tab, settingsCanvas);
        keyBindings.Add(KeyCode.I, inventoryCanvas);

        //set
        settingsCanvas.uiController = this;
        inventoryCanvas.uiController = this;
        hudController.uiController = this;

        //set HUD
        hudController.statsController = player.GetComponent<StatsController>();

        //set SettingsCanvas
        settingsCanvas.inputGenerator = player.GetComponent<InputGenerator>();
    }

   
    public void ToggleUI(KeyCode key)
    {
        //get the canvas according to key
        UICanvas canvas;
        if (keyBindings.TryGetValue(key, out canvas))
        {
            //no open canvas
            if (currentCanvas == null)
            {
                //open it
                canvas.ToggleCanvas();
                currentCanvas = canvas;
                UpdateUIStatus();
                return;
            }

            //open canvas && different    
            if (currentCanvas != canvas)
            {
                //close old
                currentCanvas.ToggleCanvas();

                //open new
                canvas.ToggleCanvas();

                //set new      
                currentCanvas = canvas;
                UpdateUIStatus();
                return;
            }

            //open canvas && same => close it, set null
            canvas.ToggleCanvas();
            currentCanvas = null;
            UpdateUIStatus();
        }
    }

    /// <summary>
    /// Used to update the running status of game
    /// </summary>
    private void UpdateUIStatus()
    {
        //if there is a UI, pause game
        if (currentCanvas != null && !inputGenerator.isGamePausing)
        {
            inputGenerator.PauseGame();
        }
        
        if (currentCanvas == null)
        {
            inputGenerator.ResumeGame();
        }


    }
}
