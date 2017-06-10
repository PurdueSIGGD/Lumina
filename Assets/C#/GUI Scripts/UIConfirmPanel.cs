using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIConfirmPanel : UIPanel {

    public abstract void OnYesClick();

    public abstract void OnNoClick();
}
