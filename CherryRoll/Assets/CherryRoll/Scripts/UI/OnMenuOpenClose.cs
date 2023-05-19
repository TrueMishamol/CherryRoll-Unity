using System;
using UnityEngine;

public class OnMenuOpenClose : MonoBehaviour {

    private void Start() {
        GameInput.Instance.OnMenuOpenCloseAction += GameInput_OnMenuOpenCloseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

        IngameMenuUI.Instance.OnMenuClosed += IngameMenuUI_OnMenuClosed;
        IngameMenuUI.Instance.OnMenuOpened += IngameMenuUI_OnMenuOpened;
    }



    private void IngameMenuUI_OnMenuOpened(object sender, EventArgs e) {
        GamePause.Instance.SetLocalGamePaused(IngameMenuUI.Instance.IsIngameMenuOppened());
    }

    private void IngameMenuUI_OnMenuClosed(object sender, EventArgs e) {
        GamePause.Instance.SetLocalGamePaused(IngameMenuUI.Instance.IsIngameMenuOppened());
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        //! Close Game Choose UI
        //! Close WaitingToStart UI
    }

    private void GameInput_OnMenuOpenCloseAction(object sender, EventArgs e) {
        IngameMenuUI.Instance.SwitchOpenClose();

        //! Close Game Choose UI
        //! Close WaitingToStart UI
    }
}
