using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public Player player;

    public InventoryCanvas inventoryCanvas;
    public SettingsCanvas settingsCanvas;
    public HUDController hudController;

    [HideInInspector] public Dictionary<KeyCode, UICanvas> keyBindings;

    private UICanvas currentCanvas;

    private void Awake()
    {
        //init
        keyBindings = new Dictionary<KeyCode, UICanvas>();

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
        UICanvas canvas;
        if (keyBindings.TryGetValue(key, out canvas))
        {
            canvas.ToggleCanvas();
        }
    }
}
