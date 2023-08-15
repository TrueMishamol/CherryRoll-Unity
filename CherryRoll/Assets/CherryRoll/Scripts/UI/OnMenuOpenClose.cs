using System;
using UnityEngine;

public class OnMenuOpenClose : MonoBehaviour {


    private void Start() {
        GameInput.Instance.OnMenuOpenCloseAction += GameInput_OnMenuOpenCloseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        // FindAllOppenedMenus
        BaseMenuUI[] baseMenuUIs = FindObjectsOfType(typeof(BaseMenuUI)) as BaseMenuUI[];

        // CloseMenuOnTop
        foreach (BaseMenuUI baseMenuUI in baseMenuUIs) {
            if (baseMenuUI.IsOppened()) {
                // Закрыть то что сверху и выйти из метода
                baseMenuUI.SwitchOpenClose();
                return;
            }
        }
    }

    private void GameInput_OnMenuOpenCloseAction(object sender, EventArgs e) {
        // FindAllOppenedMenus
        BaseMenuUI[] baseMenuUIs = FindObjectsOfType(typeof(BaseMenuUI)) as BaseMenuUI[];

        // CloseMenuOnTop
        foreach (BaseMenuUI baseMenuUI in baseMenuUIs) {
            if (baseMenuUI.IsOppened()) {
                // Закрыть то что сверху и выйти из метода
                baseMenuUI.SwitchOpenClose();
                return;
            }
        }

        // Если не найден ни один открытый baseMenuUI

        // OpenIngameMenu
        IngameMenuUI.Instance.SwitchOpenClose();
    }
}
