using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMenuOpenClose : MonoBehaviour {

    private void Start() {
        GameInput.Instance.OnMenuOpenCloseAction += GameInput_OnMenuOpenCloseAction;

        IngameMenuUI.Instance.OnMenuClosed += IngameMenuUI_OnMenuClosed;
        IngameMenuUI.Instance.OnMenuOpened += IngameMenuUI_OnMenuOpened;
    }

    private void IngameMenuUI_OnMenuOpened(object sender, EventArgs e) {
        GamePause.Instance.SetLocalGamePaused(IngameMenuUI.Instance.IsIngameMenuOppened());
    }

    private void IngameMenuUI_OnMenuClosed(object sender, EventArgs e) {
        GamePause.Instance.SetLocalGamePaused(IngameMenuUI.Instance.IsIngameMenuOppened());
    }

    private void GameInput_OnMenuOpenCloseAction(object sender, EventArgs e) {
        IngameMenuUI.Instance.SwitchOpenClose();
    }
}
